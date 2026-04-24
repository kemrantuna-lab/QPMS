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
using QPMS.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QPMS.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ViewControllerBranchTimesheetDetailView : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public ViewControllerBranchTimesheetDetailView()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged; ;
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            NestedFrame nestedFrame = Frame as NestedFrame; 
            
            if (nestedFrame != null)
            {
                try
                {
                    BranchTimesheet newBTValue = e.NewValue as BranchTimesheet;
                    BranchTimesheet oldBTvalue = e.OldValue as BranchTimesheet;

                    DateTime startT = new DateTime(newBTValue.Date.Year, newBTValue.Date.Month, newBTValue.Date.Day, newBTValue.SH.Val, newBTValue.SM.Val, 0);
                    DateTime endT = startT;
                    
                    if (endT < startT)
                    {
                        newBTValue.EH.Val = newBTValue.SH.Val;
                        newBTValue.EM.Val = newBTValue.SM.Val;
                    }

                    if (oldBTvalue.EH.Val != 24)
                    {
                        endT = new DateTime(newBTValue.Date.Year, newBTValue.Date.Month, newBTValue.Date.Day, newBTValue.EH.Val, newBTValue.EM.Val, 0);
                    } else
                    {
                        endT = new DateTime(newBTValue.Date.Year, newBTValue.Date.Month, newBTValue.Date.Day,0,0,0).AddDays(1);
                    }
                    
                    newBTValue.Duration = endT.Subtract(startT).TotalMinutes;

                    View.Refresh();
                }
                catch
                {
                    
                }
            }
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


        private void controller_ObjectChanged(object sender, ObjectCreatedEventArgs e)
        {
            NestedFrame nestedFrame = Frame as NestedFrame; 
            if (nestedFrame != null) {
                
                BranchTimesheet createdBranchTimesheet = e.CreatedObject as BranchTimesheet;

                if (createdBranchTimesheet != null)
                {
                    try
                    {
                        Employee emp = View.ObjectSpace.FirstOrDefault<Employee>(p => p.Oid == (Guid) SecuritySystem.CurrentUserId);

                        if (emp != null)
                        {
                            int startH = createdBranchTimesheet.SH.Val;
                            int startM = createdBranchTimesheet.SM.Val;
                            int endH = createdBranchTimesheet.EH.Val;
                            int endM = createdBranchTimesheet.EM.Val;

                            DateTime startT = new DateTime(createdBranchTimesheet.Date.Year, createdBranchTimesheet.Date.Month, createdBranchTimesheet.Date.Day, startH, startM, 0);
                            DateTime endT = new DateTime(createdBranchTimesheet.Date.Year, createdBranchTimesheet.Date.Month, createdBranchTimesheet.Date.Day, endH, endM, 0);

                            if (startT > endT)
                            {
                                BranchTimesheetEndHour bteh = View.ObjectSpace.FirstOrDefault<BranchTimesheetEndHour>(p => p.Val == startH && p.Company.Oid == emp.Company.Oid);

                                BranchTimesheetEndMinute btem = View.ObjectSpace.FirstOrDefault<BranchTimesheetEndMinute>(p=>p.Val == startM && p.Company.Oid == emp.Company.Oid);

                                if(bteh!= null && btem!=null) {
                                    createdBranchTimesheet.EH = bteh;
                                    createdBranchTimesheet.EM = btem;
                                } else
                                {
                                    throw new Exception("Can not find the related end time information of the company. Contact with your administrator");
                                }
                            }

                            TimeSpan durationT = endT.Subtract(startT);

                            createdBranchTimesheet.Duration = durationT.TotalHours;

                            View.Refresh();
                        }


                    }
                    catch
                    {
                        throw new Exception("Problem during calculation of duration. Please contact with your administrator for solution of the issue.");
                    }

                    View.Refresh();
                }

            }
        }
    }
}
