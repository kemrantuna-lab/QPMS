using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace QPMS.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ProjectRisk : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public ProjectRisk(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

            CreatedBy = GetCurrentUser();
            CreatedOn = DateTime.Now;

            //int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
            //Code = string.Format("PR-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + "{0:D8}", nextSequence);
            Version = 0;
            HasUpdate = true;
        }

        protected override void OnSaving()
        {
            UpdatedOn = DateTime.Now;
            UpdatedBy = GetCurrentUser();
            Version = Version + 1;
            HasUpdate = true;

            base.OnSaving();
        }



        ProjectRiskType type;
        string description;
        string name;
        Project project;

        [RuleRequiredField("RuleRequiredField for ProjectRisk.Project", DefaultContexts.Save, "A project must be specified")]
        [Association("Project-Risks")]
        public Project Project
        {
            get => project;
            set => SetPropertyValue(nameof(Project), ref project, value);
        }

        [RuleRequiredField("RuleRequiredField for ProjectRisk.Name", DefaultContexts.Save, "A name must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [RuleRequiredField("RuleRequiredField for ProjectRisk.Description", DefaultContexts.Save, "A description must be specified")]
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        public ProjectRiskType Type
        {
            get => type;
            set => SetPropertyValue(nameof(Type), ref type, value);
        }

        #region ORERPRecordDetailsRegion
        bool hasUpdate;
        int version;
        MediaDataObject photo;

        [ImageEditor(ListViewImageEditorCustomHeight = 250, DetailViewImageEditorFixedWidth = 250)]
        public MediaDataObject Photo
        {
            get => photo;
            set => SetPropertyValue(nameof(Photo), ref photo, value);
        }
        Employee createdBy;
        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public Employee CreatedBy
        {
            get { return createdBy; }
            set { SetPropertyValue("CreatedBy", ref createdBy, value); }
        }

        DateTime createdOn;
        [ModelDefault("AllowEdit", "False"), ModelDefault("DisplayFormat", "G"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { SetPropertyValue("CreatedOn", ref createdOn, value); }
        }

        Employee updatedBy;
        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public Employee UpdatedBy
        {
            get { return createdBy; }
            set { SetPropertyValue("UpdatedBy", ref updatedBy, value); }
        }

        DateTime updatedOn;
        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { SetPropertyValue("UpdatedOn", ref updatedOn, value); }
        }

        Employee GetCurrentUser()
        {
            return Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        }

        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public bool HasUpdate
        {
            get => hasUpdate;
            set => SetPropertyValue(nameof(HasUpdate), ref hasUpdate, value);
        }
        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public int Version
        {
            get => version;
            set => SetPropertyValue(nameof(Version), ref version, value);
        }

        #endregion
    }

    public enum ProjectRiskType
    {
        SAFETY,
        HUMAN,
        MATERIAL,
        LOGISTICS
    }
}