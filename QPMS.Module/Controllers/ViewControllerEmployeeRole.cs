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
    public partial class ViewControllerEmployeeRole : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public ViewControllerEmployeeRole()
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

        private void simpleActionCopyRole_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var selectedObject = View.CurrentObject as EmployeeRole;

            EmployeeRole role = ObjectSpace.FindObject<EmployeeRole>(
                    new BinaryOperator("Oid",selectedObject.Oid)
                );

            if (role != null)
            {
                var navPermList = role.NavigationPermissions.ToList();
                var typePermList = role.TypePermissions.ToList();
                var actionPermList = role.ActionPermissions.ToList();

                if (navPermList.Count > 0)
                {
                    foreach(var item in navPermList)
                    {
                        Console.WriteLine("Item Path : " + item.ItemPath);
                        Console.WriteLine("Navigate State : " + item.NavigateState);
                    }
                }


                if(typePermList.Count > 0)
                {
                    foreach(var item in typePermList)
                    {
                        var itemMemberPermList = item.MemberPermissions.ToList();

                        Console.WriteLine("Item Write State : " + item.WriteState);
                        Console.WriteLine("Item Read State : " + item.ReadState);
                        Console.WriteLine("Item Create State : " + item.CreateState);
                        Console.WriteLine("Item Delete State : " + item.DeleteState);
                        Console.WriteLine("Item Navigate State : " + item.NavigateState);
                        Console.WriteLine("*************************************************************");

                        foreach (var itemMember in itemMemberPermList)
                        {
                            Console.WriteLine(itemMember.TypePermissionObject + "Criteria : " + itemMember.Criteria);
                            Console.WriteLine("Members : " + itemMember.Members);
                            Console.WriteLine("Item Member Read State : " + itemMember.ReadState);
                            Console.WriteLine("Item Member Write State : " + itemMember.WriteState);
                        }

                        Console.WriteLine("*************************************************************");
                    }
                }
            }
        }
    }
}
