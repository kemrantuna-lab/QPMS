using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.XtraCharts.Design;
using QPMS.Module.BusinessObjects;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace QPMS.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ViewControllerProject : ViewController
    {
        private NewObjectViewController controller;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public ViewControllerProject()
        {
            //TargetObjectType = typeof(Project);
            //TargetViewType = ViewType.DetailView;
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            controller = Frame.GetController<NewObjectViewController>();
            if (controller != null)
            {
                controller.ObjectCreated += controller_ObjectCreated;
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            if (controller != null)
            {
                controller.ObjectCreated -= controller_ObjectCreated;
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void controller_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            NestedFrame frame = Frame as NestedFrame;

            if (frame != null) 
            {
                //Debug.WriteLine("Frame is not null and is a nested frame");
                Project project = e.CreatedObject as Project;

                if (project != null)
                {
                    //Debug.WriteLine("Created project is not null");

                    if (project.Code == null)
                    {
                        string m = GetYearCode(DateTime.Now) + GetMonthCode(DateTime.Now) + GetDayCode(DateTime.Now) + GetSequenceCode();
                        project.Code = m;

                        //try
                        //{
                        //    Employee currentUser = ObjectSpace.FindObject<Employee>(
                        //            new BinaryOperator("Oid", SecuritySystem.CurrentUserId)
                        //        ) ;

                        //    if(currentUser!=null && currentUser.Company != null)
                        //    {
                        //        project.Company = currentUser.Company;
                        //    }

                        //}
                        //catch
                        //{

                        //}
                    }
                    //View.Refresh();
                }
            }

            Project proj = e.CreatedObject as Project;

            if (proj != null)
            {
                proj.Code = GetYearCode(DateTime.Now) + GetMonthCode(DateTime.Now) + GetDayCode(DateTime.Now) + GetSequenceCode();
            }
        }

        #region RegionProjectCode
        /**
         * This region is dedicated for code generation and can be used freely for other code generation activities.
         * 
         * Idea behind is to avoid using a seperate module and to be dependent only to devexpress xaf libraries.
         * 
         */

        public string GetSequenceCode()
        {
            DevExpress.Xpo.IDataLayer dataLayer = ((DevExpress.ExpressApp.Xpo.XPObjectSpace)View.ObjectSpace).Session.DataLayer;

            int nextSequence = DistributedIdGeneratorHelper.Generate(dataLayer, this.GetType().FullName, "MyServerPrefix");

            string codString = String.Format("{0:0000}", nextSequence);

            string cod = codString.ToString(); 

            return cod;
        }

        public string GetYearCode(DateTime Date)
        {
            string yearCode = string.Empty;

            int year = Date.Year;

            switch (year)
            {
                case 2018:
                    yearCode = "Y";
                    break;
                case 2019:
                    yearCode = "W";
                    break;
                case 2020:
                    yearCode = "X";
                    break;
                case 2021:
                    yearCode = "XA";
                    break;
                case 2022:
                    yearCode = "XB";
                    break;
                case 2023:
                    yearCode = "XC";
                    break;
                case 2024:
                    yearCode = "XD";
                    break;
                case 2025:
                    yearCode = "XE";
                    break;
                case 2026:
                    yearCode = "XF";
                    break;
                case 2027:
                    yearCode = "XG";
                    break;
                case 2028:
                    yearCode = "XH";
                    break;
                case 2029:
                    yearCode = "XJ";
                    break;
                case 2030:
                    yearCode = "XK";
                    break;
                default:
                    yearCode = "%%";
                    break;
            }

            return yearCode;
        }

        public string GetMonthCode(DateTime Date)
        {
            string monthCode = string.Empty;

            int month = Date.Month;

            switch (month)
            {
                case 1:
                    monthCode = "A";
                    break;
                case 2:
                    monthCode = "B";
                    break;
                case 3:
                    monthCode = "C";
                    break;
                case 4:
                    monthCode = "D";
                    break;
                case 5:
                    monthCode = "R";
                    break;
                case 6:
                    monthCode = "F";
                    break;
                case 7:
                    monthCode = "G";
                    break;
                case 8:
                    monthCode = "H";
                    break;
                case 9:
                    monthCode = "J";
                    break;
                case 10:
                    monthCode = "K";
                    break;
                case 11:
                    monthCode = "M";
                    break;
                case 12:
                    monthCode = "N";
                    break;
                default:
                    monthCode = "+";
                    break;
            }

            return monthCode;
        }

        public string GetDayCode(DateTime Date)
        {
            string dayCode = string.Empty;

            int day = Date.Day;

            switch (day)
            {
                case 1:
                    dayCode = "A";
                    break;
                case 2:
                    dayCode = "B";
                    break;
                case 3:
                    dayCode = "C";
                    break;
                case 4:
                    dayCode = "D";
                    break;
                case 5:
                    dayCode = "E";
                    break;
                case 6:
                    dayCode = "F";
                    break;
                case 7:
                    dayCode = "G";
                    break;
                case 8:
                    dayCode = "H";
                    break;
                case 9:
                    dayCode = "J";
                    break;
                case 10:
                    dayCode = "K";
                    break;
                case 11:
                    dayCode = "L";
                    break;
                case 12:
                    dayCode = "M";
                    break;
                case 13:
                    dayCode = "N";
                    break;
                case 14:
                    dayCode = "O";
                    break;
                case 15:
                    dayCode = "P";
                    break;
                case 16:
                    dayCode = "R";
                    break;
                case 17:
                    dayCode = "S";
                    break;
                case 18:
                    dayCode = "T";
                    break;
                case 19:
                    dayCode = "U";
                    break;
                case 20:
                    dayCode = "V";
                    break;
                case 21:
                    dayCode = "Y";
                    break;
                case 22:
                    dayCode = "Z";
                    break;
                case 23:
                    dayCode = "X";
                    break;
                case 24:
                    dayCode = "4";
                    break;
                case 25:
                    dayCode = "5";
                    break;
                case 26:
                    dayCode = "6";
                    break;
                case 27:
                    dayCode = "7";
                    break;
                case 28:
                    dayCode = "8";
                    break;
                case 29:
                    dayCode = "9";
                    break;
                case 30:
                    dayCode = "0";
                    break;
                case 31:
                    dayCode = "1";
                    break;
                default:
                    dayCode = "*";
                    break;
            }

            return dayCode;
        }


        #endregion

        private void simpleActionAlignProject_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            var currentProject = View.CurrentObject as Project;

            if(currentProject != null)
            {
                CheckProjectLocationsAndCreateDefaultLocationIfDoesNotExist(currentProject);
                CheckProjectPartNumbersAndCreateDefaultPartNumberIfDoesNotExist(currentProject);
                CheckProjectTimesheetTypesAndCreateDefaultTimesheetTypesIfDoesNotExist(currentProject);
                CheckIfAllTimesheetsTraceabilitiesExpensesHasAssignedPTFIfNotCreateAndAssignThem(currentProject);
                CheckProjectPTFUnassignAllPTFfromTraceabilityAndTimesheetAndExpenseAndProjectCostsIfPTFIsErased(currentProject);
                CheckProjectFailureModesAndCreateDefaultFailureModeIfDoesNotExist(currentProject);
                CheckProjectRisksAndCreateDefaultRisksIfDoesNotExist(currentProject);
                CheckProjectAnalysisAndCreateIfDoesNotExist(currentProject);
                CheckProjectCurrencyAndSyncronizeAllExpenseAndCostCurrency(currentProject);
            }

            View.Refresh();
        }

        private void CheckProjectPTFUnassignAllPTFfromTraceabilityAndTimesheetAndExpenseAndProjectCostsIfPTFIsErased(Project currentProject)
        {
            try
            {
               
            } catch
            {

            }
        }

        private void CheckProjectCurrencyAndSyncronizeAllExpenseAndCostCurrency(Project currentProject)
        {
            Console.WriteLine("Currency syncronization started....");
            try
            {
                if(currentProject.Currency!= null)
                {
                    var projecttimesheettypelist = currentProject.TimesheetTypes.ToList();


                    var costList = currentProject.ProjectCosts.ToList();
                    var expenseList = currentProject.Expenses.ToList();

                    if (currentProject.Currency != null)
                    {
                        var currencyTemp = ObjectSpace.FindObject<Cur>(
                           new BinaryOperator("Oid", currentProject.Currency.Oid)
                           );

                        if (currencyTemp != null)
                        {
                            if (projecttimesheettypelist.Count > 0)
                            {
                                foreach (var item in projecttimesheettypelist)
                                {
                                    item.Currency= currencyTemp;
                                    currencyTemp.ProjectTimesheetTypes.Add(item);
                                    ObjectSpace.CommitChanges();
                                }
                            }

                            foreach (var item in costList)
                            {
                                try
                                {
                                    item.Currency = currencyTemp;
                                    currencyTemp.ProjectCosts.Add(item);
                                    ObjectSpace.CommitChanges();
                                } catch { 
                                }
                            }

                            //foreach(var item in expenseList)
                            //{
                            //    try
                            //    {
                            //        item.Currency = currencyTemp;
                            //        currencyTemp.Expenses.Add(item);
                            //        ObjectSpace.CommitChanges();
                            //    } catch { 
                            //    }
                            //}
                        }
                    }

                   

                    if (costList.Count > 0)
                    {
                        
                    }
                }
            } catch (Exception)
            {

            }
        }

        private void CheckProjectAnalysisAndCreateIfDoesNotExist(Project currentProject)
        {
            try
            {
                int daysofProject = int.Parse(currentProject.End.Subtract(currentProject.Start).TotalDays.ToString());
                if (daysofProject >= 0)
                {
                    for (int i = 0; i < daysofProject; i++)
                    {
                        ProjectAnalysisDaily pad = currentProject.PDA.Where(p => p.Date == currentProject.Start.AddDays(i)).FirstOrDefault();

                        if (pad == null)
                        {
                            pad = ObjectSpace.CreateObject<ProjectAnalysisDaily>();
                            pad.Project = currentProject;
                            currentProject.PDA.Add(pad);
                            pad.Date = currentProject.Start.AddDays(i);
                            ObjectSpace.CommitChanges();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void CheckProjectRisksAndCreateDefaultRisksIfDoesNotExist(Project currentProject)
        {
            var projectRiskList = currentProject.Risks.ToList();

            if (projectRiskList.Count == 0)
            {
                ProjectRisk prSafetyShoes = ObjectSpace.CreateObject<ProjectRisk>();
                prSafetyShoes.Type = ProjectRiskType.SAFETY;
                prSafetyShoes.Name = "Wear Safety Shoes";
                prSafetyShoes.Description = "Auto-generated";
                prSafetyShoes.Project = currentProject;
                currentProject.Risks.Add(prSafetyShoes);
                ObjectSpace.CommitChanges();

                ProjectRisk prSafetyGlasses = ObjectSpace.CreateObject<ProjectRisk>();
                prSafetyGlasses.Type = ProjectRiskType.SAFETY;
                prSafetyGlasses.Name = "Wear Safety Glasses";
                prSafetyGlasses.Description ="Auto-generated";
                prSafetyGlasses.Project = currentProject;
                currentProject.Risks.Add(prSafetyGlasses);
                ObjectSpace.CommitChanges();
            }
        }

        private void CheckProjectFailureModesAndCreateDefaultFailureModeIfDoesNotExist(Project currentProject)
        {
            var projectFailureModeList = currentProject.FailureModes.ToList();

            if (projectFailureModeList.Count == 0)
            {
                FailureMode fm = ObjectSpace.CreateObject<FailureMode>();
                fm.Name = "Default Failure Mode";
                fm.Description = "Auto-generated";
                fm.Project = currentProject;
                currentProject.FailureModes.Add(fm);
                ObjectSpace.CommitChanges();
            }
        }

        private void CheckProjectTimesheetTypesAndCreateDefaultTimesheetTypesIfDoesNotExist(Project currentProject)
        {
            var projectTimesheetTypesList = currentProject.TimesheetTypes.ToList();

            if (projectTimesheetTypesList.Count == 0)
            {
                ProjectTimesheetType pttSorting = ObjectSpace.CreateObject<ProjectTimesheetType>();
                pttSorting.Name = "Sorting";
                pttSorting.Description = "Auto-generated";
                pttSorting.Project = currentProject;
                currentProject.TimesheetTypes.Add(pttSorting);

                ProjectTimesheetType pttWarehouseSorting = ObjectSpace.CreateObject<ProjectTimesheetType>();
                pttWarehouseSorting.Name = "Sorting - Warehouse";
                pttWarehouseSorting.Description = "Auto-generated";
                pttWarehouseSorting.Project = currentProject;
                currentProject.TimesheetTypes.Add(pttWarehouseSorting);

                ProjectTimesheetType pttVehicleParkSorting = ObjectSpace.CreateObject<ProjectTimesheetType>();
                pttVehicleParkSorting.Name = "Sorting - Vehicle Park";
                pttVehicleParkSorting.Description = "Auto-generated";
                pttVehicleParkSorting.Project = currentProject;
                currentProject.TimesheetTypes.Add(pttVehicleParkSorting);

                ProjectTimesheetType pttRework = ObjectSpace.CreateObject<ProjectTimesheetType>();
                pttRework.Name = "Rework";
                pttRework.Description = "Auto-generated";
                pttRework.Project = currentProject;
                currentProject.TimesheetTypes.Add(pttRework);

                ProjectTimesheetType pttForklift = ObjectSpace.CreateObject<ProjectTimesheetType>();
                pttForklift.Name = "Forklift";
                pttForklift.Description = "Auto-generated";
                pttForklift.Project = currentProject;
                currentProject.TimesheetTypes.Add(pttForklift);

                ProjectTimesheetType pttReporting = ObjectSpace.CreateObject<ProjectTimesheetType>();
                pttReporting.Name = "Reporting";
                pttReporting.Description = "Auto-generated";
                pttReporting.Project = currentProject;
                currentProject.TimesheetTypes.Add(pttReporting);

                ProjectTimesheetType pttSpecialReporting = ObjectSpace.CreateObject<ProjectTimesheetType>();
                pttSpecialReporting.Name = "Reporting - Special";
                pttSpecialReporting.Description = "Auto-generated";
                pttSpecialReporting.Project = currentProject;
                currentProject.TimesheetTypes.Add(pttSpecialReporting);

                ProjectTimesheetType pttEngineering = ObjectSpace.CreateObject<ProjectTimesheetType>();
                pttEngineering.Project = currentProject;
                pttEngineering.Name = "Engineering";
                pttEngineering.Description = "Auto-generated.";
                currentProject.TimesheetTypes.Add(pttEngineering);
                                
                ProjectTimesheetType pttTransportation = ObjectSpace.CreateObject<ProjectTimesheetType>();
                pttTransportation.Name = "Transportation";
                pttTransportation.Description = "Description";
                pttTransportation.Project = currentProject;
                currentProject.TimesheetTypes.Add(pttTransportation);

                ProjectTimesheetType pttAccomodation = ObjectSpace.CreateObject<ProjectTimesheetType>();
                pttAccomodation.Name = "Accomodation";
                pttAccomodation.Description = "Auto-generated";
                pttAccomodation.Project = currentProject;
                currentProject.TimesheetTypes.Add(pttAccomodation);

                ProjectTimesheetType pttWorkInstructionCreation = ObjectSpace.CreateObject<ProjectTimesheetType>();
                pttWorkInstructionCreation.Name = "Work Instruction Creation";
                pttWorkInstructionCreation.Description = "Auto-generated";
                pttWorkInstructionCreation.Project = currentProject;
                currentProject.TimesheetTypes.Add(pttWorkInstructionCreation);


            }
        }

        private void CheckProjectPartNumbersAndCreateDefaultPartNumberIfDoesNotExist(Project currentProject)
        {
            var projectPartNumbersList = currentProject.Parts.ToList();
            if(projectPartNumbersList.Count == 0)
            {
                Part part = ObjectSpace.CreateObject<Part>();
                part.Name = "Default";
                part.SupplierCode = "Default";
                part.SupplierName = "Default";
                part.OEMCode = "Default";
                part.OEMName = "Default";
                part.Project = currentProject;
                currentProject.Parts.Add(part);
                ObjectSpace.CommitChanges();
            }

        }

        private void CheckProjectLocationsAndCreateDefaultLocationIfDoesNotExist(Project currentProject)
        {
            var projectLocationList = currentProject.Locations.ToList();

            if(projectLocationList.Count == 0)
            {
                ProjectLocation loc = ObjectSpace.CreateObject<ProjectLocation>();
                loc.Name = "Default";
                loc.Description = "Auto generated during project alignment routine. Should be corrected!";
                loc.Project = currentProject;
                currentProject.Locations.Add(loc);
                ObjectSpace.CommitChanges();

                ProjectLocation locWarehouse = ObjectSpace.CreateObject<ProjectLocation>();
                locWarehouse.Name = "Warehouse";
                locWarehouse.Description = "Auto generated during project alignment routine. Should be corrected!";
                locWarehouse.Project = currentProject;
                currentProject.Locations.Add(locWarehouse);
                ObjectSpace.CommitChanges();

                ProjectLocation locAssemblyLine = ObjectSpace.CreateObject<ProjectLocation>();
                locAssemblyLine.Name = "Assembly Line";
                locAssemblyLine.Description = "Auto generated during project alignment routine. Should be corrected!";
                locAssemblyLine.Project = currentProject;
                currentProject.Locations.Add(locAssemblyLine);
                ObjectSpace.CommitChanges();

                ProjectLocation locEOL = ObjectSpace.CreateObject<ProjectLocation>();
                locEOL.Name = "End of Line - EOL";
                locEOL.Description = "Auto generated during project alignment routine. Should be corrected!";
                locEOL.Project = currentProject;
                currentProject.Locations.Add(locEOL);
                ObjectSpace.CommitChanges();

                ProjectLocation locCarPark = ObjectSpace.CreateObject<ProjectLocation>();
                locCarPark.Name = "Car Park";
                locCarPark.Description = "Auto generated during project alignment routine. Should be corrected!";
                locCarPark.Project = currentProject;
                currentProject.Locations.Add(locCarPark);
                ObjectSpace.CommitChanges();

            }
        }

        private void CheckIfAllTimesheetsTraceabilitiesExpensesHasAssignedPTFIfNotCreateAndAssignThem(Project currentProject)
        {
            //var timesheetList = currentProject.Timesheets.ToList();
            var timesheetListWithOutPTF = currentProject.Timesheets.Where(p => p.PTF == null).ToList();
            var traceabilityDateListWithoutPTF = currentProject.Traceabilities.Where(p => p.PTF == null).Select(p => p.Date).Distinct().ToList();
            var timesheetDateListWithOutPTF = currentProject.Timesheets.Where(p => p.PTF == null).Select(p => p.Date).ToList();
            if (timesheetListWithOutPTF.Count > 0)
            {
                var timesheetWithoutPTFDateList = timesheetListWithOutPTF.Select(p => p.Date).Distinct().ToList();

                if (timesheetWithoutPTFDateList.Count > 0)
                {
                    foreach (var item in timesheetWithoutPTFDateList)
                    {
                        var ptfListOfDate = currentProject.PTFs.Where(p => p.Date == item).ToList();

                        if (ptfListOfDate.Count == 0)
                        {
                            PTF ptfOfDate = ObjectSpace.CreateObject<PTF>();
                            ptfOfDate.Date = item.Date;
                            ptfOfDate.Project = currentProject;
                            currentProject.PTFs.Add(ptfOfDate);
                        }
                        ObjectSpace.CommitChanges();
                    }
                }

                foreach (var item in timesheetListWithOutPTF)
                {
                    PTF itemDatePTF = currentProject.PTFs.Where(p => p.Date == item.Date).FirstOrDefault();

                    if (itemDatePTF != null)
                    {
                        itemDatePTF.Timesheets.Add(item);
                        item.PTF = itemDatePTF;
                    }
                    ObjectSpace.CommitChanges();
                }
            }

            AttachTraceabilityToPTF(currentProject);
            //UpdateTraceabilityResults(currentProject);
            AttachExpenseToPTF(currentProject);

            var ptfList = currentProject.PTFs.ToList();

            double tempHours = 0;

            foreach (var item in ptfList)
            {
                double tempHours2 = 0;
                try
                {
                    var ptfTimesheetList = item.Timesheets.ToList();


                    foreach (var ptfTimesheetItem in ptfTimesheetList)
                    {
                        tempHours2 = tempHours2 + ptfTimesheetItem.Hours;
                    }



                }
                catch
                {

                }

                tempHours = tempHours + tempHours2;

            }


            currentProject.THours = tempHours;

            ObjectSpace.CommitChanges();

            CheckUpdatesOfDetails(currentProject);
        }

        private void CheckUpdatesOfDetails(Project currentProject)
        {
            var timesheetList = currentProject.Timesheets.ToList();

            if (timesheetList.Count > 0)
            {
                 foreach(var item in timesheetList)
                  {
                                item.HasUpdate = false;
                                ObjectSpace.CommitChanges();
                  }
            }

            var traceabilityList = currentProject.Traceabilities.ToList();

            if(traceabilityList.Count> 0)
            {
                foreach (var item in traceabilityList)
                {
                    item.HasUpdate = false;
                    ObjectSpace.CommitChanges();
                }
            }

            var expenseList = currentProject.Expenses.ToList();

            if (expenseList.Count > 0)
            {

                foreach (var item in expenseList)
                {
                    item.HasUpdate = false;
                    ObjectSpace.CommitChanges();
                }
            }

            var partList = currentProject.Parts.ToList();

            if(partList.Count > 0)
            {
                foreach (var item in partList)
                {
                    item.HasUpdate = false;
                    ObjectSpace.CommitChanges();
                }
            }

            currentProject.HasUpdate = false;
            ObjectSpace.CommitChanges();
        }

        private void AttachExpenseToPTF(Project currentProject)
        {
            var expenseList = currentProject.Expenses.ToList();
            var expenseListWithoutPTF = currentProject.Expenses.Where(p => p.PTF == null).ToList();
            var expenseDateListWithoutPTF = currentProject.Expenses.Where(p => p.PTF == null).Select(p => p.Date).Distinct().ToList();

            if (expenseDateListWithoutPTF.Count > 0)
            {
                foreach(var item in expenseDateListWithoutPTF)
                {
                    PTF missingDatePTF = ObjectSpace.FindObject<PTF>(
                        new BinaryOperator("Date", item)
                        );

                    if (missingDatePTF == null)
                    {
                        missingDatePTF = ObjectSpace.CreateObject<PTF>();
                        missingDatePTF.Date = item.Date;
                        missingDatePTF.Project = currentProject;
                        currentProject.PTFs.Add(missingDatePTF);
                    }
                    ObjectSpace.CommitChanges();
                }

                foreach(var item in expenseListWithoutPTF)
                {
                    PTF attachExpenseToPTF = ObjectSpace.FindObject<PTF>(
                        new BinaryOperator("Date", item.Date)
                        );

                    if(attachExpenseToPTF == null)
                    {
                        attachExpenseToPTF.Expenses.Add(item);
                        item.PTF = attachExpenseToPTF;
                    }

                    ObjectSpace.CommitChanges();
                }
            }
        }

        private void AttachTraceabilityToPTF(Project currentProject)
        {
            var traceabilityList = currentProject.Traceabilities.ToList();
            var traceabilityDateListWithoutPTF = currentProject.Traceabilities.Where(p => p.PTF == null).Select(p=>p.Date).Distinct().ToList();

            if (traceabilityDateListWithoutPTF.Count > 0)
            {
                foreach(var item in traceabilityDateListWithoutPTF)
                {
                    PTF missingDatePTF = ObjectSpace.FindObject<PTF>(
                            new BinaryOperator("Date",item)
                        );

                    if (missingDatePTF == null)
                    {
                        missingDatePTF = ObjectSpace.CreateObject<PTF>();
                        missingDatePTF.Date = item.Date;
                        missingDatePTF.Project = currentProject;
                        currentProject.PTFs.Add(missingDatePTF);
                    }

                    ObjectSpace.CommitChanges();
                }
            }
        }
    }
}
