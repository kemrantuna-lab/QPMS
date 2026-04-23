using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web;
using System.Web.Routing;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;
using DevExpress.Web;

using Hangfire;
using Hangfire.SqlServer;
using System.Diagnostics;
using System.Collections.Generic;
using DevExpress.Persistent.AuditTrail;
using DevExpress.ExpressApp.Xpo;
using QPMS.Module.BusinessObjects;
using System.Linq;

namespace QPMS.Web {
    public class Global : System.Web.HttpApplication {

        //private BackgroundJobServer _backgroundJobServer;
        public Global() {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e) {
            DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode = DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.Latest;
            RouteTable.Routes.RegisterXafRoutes();
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
            //_backgroundJobServer = new BackgroundJobServer();
#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif

            /**
             * HANGFIRE
             * ***/

            //AreaRegistration.RegisterAllAreas();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);

            HangfireAspNet.Use(GetHangfireServers);

            // Let's also create a sample background job
            //BackgroundJob.Enqueue(() => Debug.WriteLine("Hello world from Hangfire!"));

            BackgroundJob.Enqueue(
                    () => Recalc()
                ) ;



        }
        
        private void Recalc()
        {
            Debug.WriteLine("Entering Routine");
            XPObjectSpaceProvider globalObjectSpace = new XPObjectSpaceProvider(
 QPMS.Module.ConnectionStringProvider.RequireConnectionString(), null);

            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(BranchTimesheet));
                XPObjectSpaceProvider osProvider = globalObjectSpace;
                IObjectSpace objectSpace = osProvider.CreateObjectSpace();

                var branchTimesheetsList = objectSpace.GetObjects<BranchTimesheet>().ToList();

                Debug.WriteLine("Branch Tİmehsheets initialized");

                foreach (var item in branchTimesheetsList)
                {

                    try
                    {
                        DateTime startT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.SH.Val, item.SM.Val, 0);

                        DateTime endT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.EH.Val, item.EM.Val, 0);

                        if (item.EH.Val >= 24)
                        {
                            endT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, 0, 0, 0).AddDays(1);
                            item.EM = item.CreatedBy.Company.BranchTimesheetEndMinutes.Where(p => p.Val == 0 && p.Company.Oid == item.UpdatedBy.Company.Oid).FirstOrDefault();
                        }
                        else
                        {
                            endT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.EH.Val, item.EM.Val, 0);

                            if (endT < startT)
                            {

                                item.EH = item.CreatedBy.Company.BranchTimesheetEndHours.Where(p => p.Val == item.SH.Val && p.Company.Oid == item.UpdatedBy.Company.Oid).FirstOrDefault();
                                item.EM = item.CreatedBy.Company.BranchTimesheetEndMinutes.Where(p => p.Val == item.SM.Val && p.Company.Oid == item.UpdatedBy.Company.Oid).FirstOrDefault();
                                endT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.EH.Val, item.EM.Val, 0);
                            }
                        }
                        //DateTime endT = new DateTime(Date.Year, Date.Month, Date.Day, EH.Val, EM.Val, 0);

                        //item.Duration = endT.Subtract(startT).TotalMinutes;
                        item.Hour = endT.Subtract(startT).TotalHours;

                        objectSpace.CommitChanges();
                        Debug.WriteLine(item.Oid.ToString() + " " + "has been successfully saved with " + item.Hour.ToString());
                    } catch
                    {
                        Debug.WriteLine(item.Oid + " " + item.Date.ToString() + "Problem on timesheet");
                    }

                    
                }
            } catch
            {
                Debug.WriteLine("Problem in recalc routine");
            }
        }

        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
             .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UseSqlServerStorage(QPMS.Module.ConnectionStringProvider.RequireConnectionString(), new SqlServerStorageOptions
             {
                 CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                 SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                 QueuePollInterval = TimeSpan.Zero,
                 UseRecommendedIsolationLevel = true,
                 DisableGlobalLocks = true
             });
            yield return new BackgroundJobServer();
        }

        protected void Session_Start(Object sender, EventArgs e) {
            Tracing.Initialize();
            WebApplication.SetInstance(Session, new QPMSAspNetApplication());
            AuditTrailService.Instance.ObjectAuditingMode = ObjectAuditingMode.Lightweight;
            SecurityStrategy security = WebApplication.Instance.GetSecurityStrategy();
            security.RegisterXPOAdapterProviders();
            DevExpress.ExpressApp.Web.Templates.DefaultVerticalTemplateContentNew.ClearSizeLimit();
            WebApplication.Instance.SwitchToNewStyle();
            WebApplication.Instance.ConnectionString = QPMS.Module.ConnectionStringProvider.RequireConnectionString();
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
        }
        protected void Application_BeginRequest(Object sender, EventArgs e) {
        }
        protected void Application_EndRequest(Object sender, EventArgs e) {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
        }
        protected void Application_Error(Object sender, EventArgs e) {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e) {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e) {

        }
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
        }
        #endregion
    }
}
