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
    public partial class ViewControllerProjectCost : ViewController
    {
        private NewObjectViewController controller;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public ViewControllerProjectCost()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
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
                ProjectCost pc = e.CreatedObject as ProjectCost;

                if(pc != null)
                {
                    try
                    {
                        PTF parentIsPTF = ((NestedFrame) Frame).ViewItem.CurrentObject as PTF;

                        if (parentIsPTF != null)
                        {
                            var currentParentIsPTF = pc.Session.FindObject<PTF>(
                                    new BinaryOperator("Oid",parentIsPTF.Oid)
                                );

                            if(currentParentIsPTF != null )
                            {
                                pc.Project = currentParentIsPTF.Project;
                            }
                        }
                    } catch (Exception ex)
                    {

                    }
                }
            }
        }
    }
}
