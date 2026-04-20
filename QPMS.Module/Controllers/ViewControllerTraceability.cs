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
    public partial class ViewControllerTraceability : ViewController
    {
        private NewObjectViewController controller;
        public ViewControllerTraceability()
        {
            InitializeComponent();
            TargetObjectType = typeof(Traceability);
            TargetViewType = ViewType.Any;
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


        #region asilkod

        private void controller_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            NestedFrame nestedFrame = Frame as NestedFrame;
            if (nestedFrame != null)
            {
                Traceability createdItem = e.CreatedObject as Traceability;

                if (createdItem != null)
                {
                    PTF parentIsPTF = ((NestedFrame)Frame).ViewItem.CurrentObject as PTF;
                    if (parentIsPTF != null)
                    {
                        var currentParentPTF = createdItem.Session.FindObject<PTF>(
                                new BinaryOperator("Oid", parentIsPTF.Oid)
                            );

                        if (currentParentPTF != null)
                        {
                            createdItem.PTF = currentParentPTF;
                            createdItem.Date = currentParentPTF.Date;

                            var currentParentPTFProject = createdItem.Session.FindObject<Project>(
                                    new BinaryOperator("Oid", currentParentPTF.Project.Oid)
                                );
                            if (currentParentPTFProject != null)
                            {
                                createdItem.Project = currentParentPTFProject;
                                //var createdItemsList = currentParentPTFProject.PTFTraceabilities.ToList();
                                //int count = createdItemsList.Count;

                                //if (count == 0)
                                //{
                                //    createdItem.Name = currentParentPTFProject.Name + "-" + "T" + "-" + (count + 1).ToString();
                                //}
                                //else if (count > 0)
                                //{
                                //    int i = count + 1;
                                //    string iString = String.Format("{0:000000", i);
                                //    createdItem.Name = currentParentPTFProject.Name + "-" + "T" + "-" + iString;
                                //}
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
