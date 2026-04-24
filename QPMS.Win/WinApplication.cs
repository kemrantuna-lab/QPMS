using System;
using System.ComponentModel;
using System.Configuration;
using System.ServiceModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using System.Collections.Generic;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using QPMS.Module.BusinessObjects;

namespace QPMS.Win {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Win.WinApplication._members
    public partial class QPMSWindowsFormsApplication : WinApplication {
        private bool useMiddleTier;
        private string middleTierEndpoint;
        private WcfSecuredDataServerClient middleTierDataServerClient;
        private ServerSecurityClient middleTierSecurityClient;

        public QPMSWindowsFormsApplication() {
			InitializeComponent();
            InitializeDefaults();
			SplashScreen = new DXSplashScreen(typeof(XafSplashScreen), new DefaultOverlayFormOptions());
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
        }

        private void InitializeDefaults()
        {
            LinkNewObjectToParentImmediately = true;
            OptimizedControllersCreation = true;
            UseLightStyle = true;
            //use svg images in the application.
            DevExpress.ExpressApp.Utils.ImageLoader.Instance.UseSvgImages = true;
        }

        public bool UseMiddleTier => useMiddleTier;

        public void ConfigureDataAccessFromConfiguration() {
            useMiddleTier = bool.TryParse(ConfigurationManager.AppSettings["UseMiddleTier"], out bool enabled) && enabled;
            middleTierEndpoint = ConfigurationManager.AppSettings["MiddleTierEndpoint"];

            if(!useMiddleTier) {
                middleTierDataServerClient = null;
                middleTierSecurityClient = null;
                ConnectionString = QPMS.Module.ConnectionStringProvider.RequireConnectionString();
                return;
            }

            if(string.IsNullOrWhiteSpace(middleTierEndpoint)) {
                throw new ConfigurationErrorsException("MiddleTierEndpoint is not configured.");
            }

            middleTierDataServerClient = new WcfSecuredDataServerClient(
                WcfDataServerHelper.CreateNetTcpBinding(),
                new EndpointAddress(middleTierEndpoint));
            ServerSecurityClient.CanUseCache = false;
            middleTierSecurityClient = new ServerSecurityClient(middleTierDataServerClient, TypesInfo) {
                IsSupportChangePassword = true
            };
            Security = middleTierSecurityClient;
            ConnectionString = middleTierEndpoint;
        }

        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            if(useMiddleTier) {
                if(middleTierDataServerClient == null || middleTierSecurityClient == null) {
                    throw new InvalidOperationException("Middle tier mode is enabled but the client is not configured.");
                }

                DataServerObjectSpaceProvider objectSpaceProvider =
                    new DataServerObjectSpaceProvider(middleTierDataServerClient, middleTierSecurityClient) {
                        CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema,
                        ConnectionString = middleTierEndpoint
                    };
                args.ObjectSpaceProviders.Add(objectSpaceProvider);
            }
            else {
                args.ObjectSpaceProviders.Add(new SecuredObjectSpaceProvider((SecurityStrategyComplex)Security, XPObjectSpaceProvider.GetDataStoreProvider(args.ConnectionString, args.Connection, true), false));
            }
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }
        private void QPMSWindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e) {
            string userLanguageName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            if(userLanguageName != "en-US" && e.Languages.IndexOf(userLanguageName) == -1) {
                e.Languages.Add(userLanguageName);
            }
        }
        private void QPMSWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
            if(System.Diagnostics.Debugger.IsAttached) {
                e.Updater.Update();
                e.Handled = true;
            }
            else {
				string message = "The application cannot connect to the specified database, " +
					"because the database doesn't exist, its version is older " +
					"than that of the application or its schema does not match " +
					"the ORM data model structure. To avoid this error, use one " +
					"of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";

				if(e.CompatibilityError != null && e.CompatibilityError.Exception != null) {
					message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
				}
				throw new InvalidOperationException(message);
            }
#endif
        }
    }
}
