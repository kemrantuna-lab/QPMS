using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using Hangfire;
using QPMS.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QPMS.Web
{
    public partial class HangFireJobs : System.Web.UI.Page
    {
        private XPObjectSpaceProvider globalObjectSpace = new XPObjectSpaceProvider(
QPMS.Module.ConnectionStringProvider.RequireConnectionString(), null);



        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            try
            {
                BackgroundJob.Enqueue<ReCalculateValues>(
                            x => x.BranchTimesheetRecalculate()
                        ) ;
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Branch Timesheet recalculation have been started')", true);
            }
            catch
            {

            }
        }

    }

    public class ReCalculateValues{
        private XPObjectSpaceProvider globalObjectSpace = new XPObjectSpaceProvider(
QPMS.Module.ConnectionStringProvider.RequireConnectionString(), null);

        public void BranchTimesheetRecalculate()
        {
            ReCalculateBranchTimesheetHours();
        }

        private void ReCalculateBranchTimesheetHours()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(BranchTimesheet));
                XPObjectSpaceProvider osProvider = globalObjectSpace;
                IObjectSpace objectSpace = osProvider.CreateObjectSpace();

                var branchTimesheetsList = objectSpace.GetObjects<BranchTimesheet>().ToList();

                foreach(var item in branchTimesheetsList )
                {
                    
                    DateTime startT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.SH.Val, item.SM.Val, 0);

                    DateTime endT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.EH.Val, item.EM.Val, 0);

                    if (item.EH.Val >= 24)
                    {
                        endT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, 0, 0, 0).AddDays(1);
                        item.EM = item.CreatedBy.Company.BranchTimesheetEndMinutes.Where(p => p.Val == 0 && p.Company.Oid ==item.UpdatedBy.Company.Oid).FirstOrDefault();
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
                }
            } catch
            {

            }
        }
    }
}
