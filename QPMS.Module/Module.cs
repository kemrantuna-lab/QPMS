using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using QPMS.Module.BusinessObjects;
using QPMS.Module.Reports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QPMS.Module
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class QPMSModule : ModuleBase
    {
        public QPMSModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            PredefinedReportsUpdater predefinedReportsUpdater =
                new PredefinedReportsUpdater(Application, objectSpace, versionFromDB);
            predefinedReportsUpdater.AddPredefinedReport<Project_Report>("Project Report", typeof(Project));

            return new ModuleUpdater[] { updater, predefinedReportsUpdater };
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
            application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }

        private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        {
            CompositeObjectSpace compositeObjectSpace = e.ObjectSpace as CompositeObjectSpace;
            if (compositeObjectSpace != null)
            {
                if (!(compositeObjectSpace.Owner is CompositeObjectSpace))
                {
                    compositeObjectSpace.PopulateAdditionalObjectSpaces((XafApplication)sender);
                }
            }
        }
    }
}
