using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.XtraCharts;
using QPMS.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QPMS.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ViewControllerBranchAnalysisDaily : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public ViewControllerBranchAnalysisDaily()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void UpdateBranchDailyAnalysis_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var currentBranchDailyAnalysis = View.CurrentObject as BranchAnalysisDaily;

            if (currentBranchDailyAnalysis != null)
            {
                UpdateStaffQuantity(currentBranchDailyAnalysis);
                UpdateDailyTotalsOfTraceability(currentBranchDailyAnalysis);
                UpdateProjectInfo(currentBranchDailyAnalysis);
                UpdatePTFs(currentBranchDailyAnalysis);
                UpdateDailyTotalsOfBTFs(currentBranchDailyAnalysis);
            }
        }

        private void UpdateDailyTotalsOfBTFs(BranchAnalysisDaily currentBranchDailyAnalysis)
        {
            int btfCount = 0;

            double btCount = 0;
            double btHourCount = 0;

            var branch = currentBranchDailyAnalysis.Branch;

            var btfList = branch.BTFs.Where(p=>p.Date == currentBranchDailyAnalysis.Date).ToList();

            if (btfList.Count() > 0)
            {
                btfCount = btfList.Count();
            }

            var branchTimesheetList = branch.BranchTimesheets.Where(p=>p.Date == currentBranchDailyAnalysis.Date).ToList() ;

            if (branchTimesheetList.Count() > 0)
            {
                btCount= branchTimesheetList.Count();

                foreach(var item in branchTimesheetList)
                {
                    btHourCount+= item.Hour;
                }
            }

            currentBranchDailyAnalysis.TotalBTF = btfCount;
            currentBranchDailyAnalysis.BranchTimesheetQuantity= btCount;
            currentBranchDailyAnalysis.TotalBTFHourOfPresentPeople = btHourCount;
        }

        private void UpdateDailyTotalsOfTraceability(BranchAnalysisDaily currentBranchDailyAnalysis)
        {
            DateTime searchingDate = currentBranchDailyAnalysis.Date;

            int tCount = 0;

            int dailyOK = 0;
            int dailyNOK = 0;
            int dailyROK = 0;
            int dailyRNOK = 0;

            var projectList = currentBranchDailyAnalysis.Branch.Projects.Where(p => p.Start <= searchingDate && p.End >= searchingDate).ToList();

            if (projectList.Count() > 0)
            {
                foreach(var item in projectList)
                {
                    try
                    {
                        var ptfList = item.PTFs.Where(p=>p.Date.Date == searchingDate.Date).ToList();
                        if(ptfList.Count() > 0)
                        {
                            foreach(var item2 in ptfList)
                            {
                                var tList = item2.Traceabilities.ToList();

                                if(tList.Count() > 0)
                                {
                                    foreach(var item3 in tList)
                                    {
                                        tCount = tCount + 1;
                                        dailyOK += item3.OK;
                                        dailyNOK += item3.NOK;
                                        dailyROK+= item3.ROK;
                                        dailyRNOK+= item3.RNOK;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            currentBranchDailyAnalysis.DailyOK = dailyOK;
            currentBranchDailyAnalysis.DailyNOK= dailyNOK;
            currentBranchDailyAnalysis.DailyRNOK = dailyRNOK;
            currentBranchDailyAnalysis.DailyROK= dailyROK;

            currentBranchDailyAnalysis.TraceabilityQuantity = tCount;
        }

        private void UpdatePTFs(BranchAnalysisDaily currentBranchDailyAnalysis)
        {
            int ptfCount = 0;
            double normalHours = 0;
            double extraHours = 0;
            double holidayHours = 0;

            var projectList = currentBranchDailyAnalysis.Branch.Projects.Where(p => p.Start <= currentBranchDailyAnalysis.Date && p.End >= currentBranchDailyAnalysis.Date).ToList();

            if (projectList.Count() > 0)
            {
                try
                {
                    foreach(var project in projectList) { 
                        var ptfList = project.PTFs.Where(p=>p.Date.Date == currentBranchDailyAnalysis.Date.Date).ToList();

                        if(ptfList.Count() > 0)
                        {
                            foreach(var ptf in ptfList)
                            {
                                try
                                {
                                    ptfCount = ptfCount + 1;

                                    normalHours = normalHours + ptf.NH;
                                    extraHours = extraHours + ptf.EH;
                                    holidayHours = holidayHours + ptf.HH;
                                } catch
                                {

                                }

                                
                            }
                        }
                    }
                }
                catch
                {

                }
            }

            currentBranchDailyAnalysis.PTFQuantity= ptfCount;
            currentBranchDailyAnalysis.PTFNormalHours= normalHours;
            currentBranchDailyAnalysis.PTFExtraHours= extraHours;
            currentBranchDailyAnalysis.PTFHolidayHours= holidayHours;  

        }

        private void UpdateProjectInfo(BranchAnalysisDaily currentBranchDailyAnalysis)
        {
            var projectList = currentBranchDailyAnalysis.Branch.Projects.Where(p=>p.Start.Date<=currentBranchDailyAnalysis.Date.Date && p.End>=currentBranchDailyAnalysis.Date.Date).ToList();

            int projectCountOnGoing = 0;
            int projectCountStarting = 0;
            int projectCountEnding = 0;

            if (projectList.Count() > 0)
            {
                projectCountOnGoing = projectList.Count;

                var startingProjectList = projectList.Select(p => p.Start.Date == currentBranchDailyAnalysis.Date.Date).ToList();

                if (startingProjectList.Count() > 0)
                {
                    currentBranchDailyAnalysis.ProjectStarting = startingProjectList.Count();
                }

                var endingProjectList = projectList.Select(p=>p.End.Date == currentBranchDailyAnalysis.Date.Date).ToList();   

                if(endingProjectList.Count() > 0)
                {
                    currentBranchDailyAnalysis.ProjectEnding= endingProjectList.Count();
                }
            }

            currentBranchDailyAnalysis.ProjectQuantity= projectCountOnGoing;
            currentBranchDailyAnalysis.ProjectStarting = projectCountStarting;
            currentBranchDailyAnalysis.ProjectEnding = projectCountEnding;

        }

        private void UpdateStaffQuantity(BranchAnalysisDaily currentBranchDailyAnalysis)
        {
            var listOfEmployees = currentBranchDailyAnalysis.Branch.Employees.Where(p=>p.Start.Date<=currentBranchDailyAnalysis.Date.Date 
            && (p.End.Date<=currentBranchDailyAnalysis.Date.Date || p.End.Date == null)).ToList();

            if(listOfEmployees.Count > 0)
            {
                currentBranchDailyAnalysis.StaffQuantity = listOfEmployees.Count();
            }
        }


    }
}
