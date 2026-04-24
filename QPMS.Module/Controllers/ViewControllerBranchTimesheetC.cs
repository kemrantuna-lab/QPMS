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
    public partial class ViewControllerBranchTimesheetC : ViewController
    {
        private NewObjectViewController controller;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public ViewControllerBranchTimesheetC()
        {
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
            NestedFrame nestedFrame = Frame as NestedFrame;

            if (nestedFrame != null)
            {
                BranchTimesheet createdBranchTimesheet = e.CreatedObject as BranchTimesheet;

                if (createdBranchTimesheet != null)
                {
                    try
                    {
                        BTF parentIsBTF = ((NestedFrame)Frame).ViewItem.CurrentObject as BTF;

                        if (parentIsBTF != null)
                        {
                            var currentParentBTF = createdBranchTimesheet.Session.FindObject<BTF>(
                                    new BinaryOperator("Oid", parentIsBTF.Oid)
                                );

                            if (currentParentBTF != null)
                            {
                                createdBranchTimesheet.BTF = currentParentBTF;
                                createdBranchTimesheet.Date = currentParentBTF.Date;

                                var currentParentBTFBranch = createdBranchTimesheet.Session.FindObject<Branch>(
                                        new BinaryOperator("Oid", currentParentBTF.Branch.Oid)
                                    );

                                if (currentParentBTFBranch != null)
                                {
                                    createdBranchTimesheet.Branch = currentParentBTF.Branch;
                                }
                            }
                        }

                    }
                    catch
                    {

                    }

                    try
                    {
                        Branch parentIsBranch = ((NestedFrame)Frame).ViewItem.CurrentObject as Branch;

                        if (parentIsBranch != null)
                        {
                            var currentParentIsBranch = createdBranchTimesheet.Session.FindObject<Branch>(
                                    new BinaryOperator("Oid", parentIsBranch.Oid)
                                );

                            if (currentParentIsBranch != null)
                            {
                                createdBranchTimesheet.Branch = currentParentIsBranch;
                            }
                        }
                    } catch
                    {

                    }

                   
                }
            }
        }

        private void InitializeBranchTimesheet_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var createdBranchTimesheet = e.CurrentObject as BranchTimesheet;

            try
            {

                if (createdBranchTimesheet != null)
                {
                    try
                    {
                        if (createdBranchTimesheet.Absence == null)
                        {
                            Employee emp = ObjectSpace.FindObject<Employee>(
                                    new BinaryOperator("Oid", SecuritySystem.CurrentUserId)
                                );
                            if (emp != null && emp.Company != null)
                            {
                                createdBranchTimesheet.Absence = emp.Company.CompanyAbsences.FirstOrDefault();
                                //ObjectSpace.CommitChanges();
                            }
                        }
                    }
                    catch
                    {

                    }

                    try
                    {
                        if (createdBranchTimesheet.Presence == null)
                        {
                            Employee emp = ObjectSpace.FindObject<Employee>(
                                    new BinaryOperator("Oid", SecuritySystem.CurrentUserId)
                                );
                            if (emp != null && emp.Company != null)
                            {
                                createdBranchTimesheet.Presence = emp.Company.CompanyPresences.FirstOrDefault();
                                //ObjectSpace.CommitChanges();
                            }
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (createdBranchTimesheet.SH == null)
                        {
                            Employee emp = ObjectSpace.FindObject<Employee>(
                                    new BinaryOperator("Oid", SecuritySystem.CurrentUserId)
                                );
                            if (emp != null && emp.Company != null)
                            {
                                createdBranchTimesheet.SH = emp.Company.BranchTimesheetStartHours.Where(p => p.Val == 8).FirstOrDefault();
                                //ObjectSpace.CommitChanges();
                            }
                        }

                    }
                    catch
                    {

                    }

                    try
                    {
                        if (createdBranchTimesheet.SM == null)
                        {
                            Employee emp = ObjectSpace.FindObject<Employee>(
                                    new BinaryOperator("Oid", SecuritySystem.CurrentUserId)
                                );
                            if (emp != null && emp.Company != null)
                            {
                                createdBranchTimesheet.SM = emp.Company.BranchTimesheetStartMinutes.Where(p => p.Val == 0).FirstOrDefault();
                                //ObjectSpace.CommitChanges();
                            }
                        }
                    }
                    catch
                    {

                    }

                    try
                    {
                        if (createdBranchTimesheet.EH == null)
                        {
                            Employee emp = ObjectSpace.FindObject<Employee>(
                                    new BinaryOperator("Oid", SecuritySystem.CurrentUserId)
                                );
                            if (emp != null && emp.Company != null)
                            {
                                createdBranchTimesheet.EH = emp.Company.BranchTimesheetEndHours.Where(p => p.Val == 16).FirstOrDefault();
                                //ObjectSpace.CommitChanges();
                            }
                        }
                    }
                    catch
                    {

                    }

                    try
                    {
                        if (createdBranchTimesheet.EM == null)
                        {
                            Employee emp = ObjectSpace.FindObject<Employee>(
                                    new BinaryOperator("Oid", SecuritySystem.CurrentUserId)
                                );
                            if (emp != null && emp.Company != null)
                            {
                                createdBranchTimesheet.EM = emp.Company.BranchTimesheetEndMinutes.Where(p => p.Val == 0).FirstOrDefault();
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }
    }
}
