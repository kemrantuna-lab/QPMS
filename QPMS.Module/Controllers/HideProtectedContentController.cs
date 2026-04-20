using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QPMS.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class HideProtectedContentController : ViewController<ObjectView>
    {
        private AppearanceController appearanceController;
        protected override void OnActivated()
        {
            base.OnActivated();
            appearanceController = Frame.GetController<AppearanceController>();
            if (appearanceController != null)
            {
                appearanceController.CustomApplyAppearance += appearanceController_CustomApplyAppearance;
            }
        }
        protected override void OnDeactivated()
        {
            if (appearanceController != null)
            {
                appearanceController.CustomApplyAppearance -= appearanceController_CustomApplyAppearance;
            }
            base.OnDeactivated();
        }
        void appearanceController_CustomApplyAppearance(object sender, ApplyAppearanceEventArgs e)
        {
            if (e.AppearanceObject.Visibility == null || e.AppearanceObject.Visibility == ViewItemVisibility.Show)
            {
                if (View is ListView)
                {
                    if (e.Item is ColumnWrapper)
                    {
                        if (!DataManipulationRight.CanRead(View.ObjectTypeInfo.Type,
                            ((ColumnWrapper)e.Item).PropertyName, null,
                            ((ListView)View).CollectionSource, View.ObjectSpace))
                        {
                            e.AppearanceObject.Visibility = ViewItemVisibility.Hide;
                        }
                    }
                }
                if (View is DetailView)
                {
                    if (e.Item is PropertyEditor)
                    {
                        if (!DataManipulationRight.CanRead(View.ObjectTypeInfo.Type,
                            ((PropertyEditor)e.Item).PropertyName,
                            e.ContextObjects.Length > 0 ? e.ContextObjects[0] : null, null,
                            View.ObjectSpace))
                        {
                            e.AppearanceObject.Visibility = ViewItemVisibility.Hide;
                        }
                    }
                }
            }
        }
    }
}
