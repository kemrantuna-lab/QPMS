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
    public partial class ViewControllerProjectTimesheetDetailView : ObjectViewController<DetailView,ProjectTimesheet>
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public ViewControllerProjectTimesheetDetailView()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            try
            {
                var currentProjectTimesheet = View.CurrentObject as ProjectTimesheet;
                if(currentProjectTimesheet != null)
                {
                    if(currentProjectTimesheet.Project!=null && currentProjectTimesheet.Project.State != State.ON)
                    {
                        var editors = View.GetItems<PropertyEditor>();
                        foreach (var editor in editors)
                        {
                            editor.AllowEdit["CustomReason"] = false;
                        }
                    } else if(currentProjectTimesheet.Project != null && currentProjectTimesheet.Project.State == State.ON)
                    {
                        if(currentProjectTimesheet.CreatedOn!= null && currentProjectTimesheet.CreatedOn > DateTime.Today.AddDays(-5))
                        {
                            var editors = View.GetItems<PropertyEditor>();
                            foreach (var editor in editors)
                            {
                                editor.AllowEdit["CustomReason"] = true;
                            }
                        } else
                        {
                            var editors = View.GetItems<PropertyEditor>();
                            foreach (var editor in editors)
                            {
                                editor.AllowEdit["CustomReason"] = false;
                            }
                        }
                    }
                }
            }
            catch
            {

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
    }
}
