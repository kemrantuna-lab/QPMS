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
    public class ProjectEndInfo : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public ProjectEndInfo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

            CreatedBy = GetCurrentUser();
            CreatedOn = DateTime.Now;
            Version = 0;
            HasUpdate = true;

            if (CreatedBy != null)
            {
                try
                {
                    Company = CreatedBy.Company;
                } catch
                {

                }
            }

        }


        //[RuleRequiredField("RuleRequiredField for ProjectEndInfo.Company", DefaultContexts.Save, "A company must be specified")]
        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        [Association("Company-ProjectEndInformations")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        //[RuleRequiredField("RuleRequiredField for ProjectEndInfo.Project", DefaultContexts.Save, "A project must be selected.")]
        [ModelDefault("AllowEdit", "True"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        [DataSourceProperty("Company.Projects")]
        public Project Project
        {
            get => project;
            set
            {
                if (project == null)
                    return;
                project = value;

                Project previousProject = project;


                if (IsLoading) return;

                if (previousProject != null && previousProject.PEI == this)
                    previousProject.PEI = null;

                if (project != null)
                    project.PEI = this;

                OnChanged(nameof(Project));

            }
        }



        [RuleRequiredField("RuleRequiredField for ProjectEndInfo.Description", DefaultContexts.Save, "A description must be specified")]
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        [RuleRequiredField("RuleRequiredField for ProjectEndInfo.ClosingDate", DefaultContexts.Save, "A date must be specified")]
        public DateTime ClosingDate
        {
            get => closingDate;
            set => SetPropertyValue(nameof(ClosingDate), ref closingDate, value);
        }

        DateTime closingDate;
        string description;
        Company company;
        Project project = null;


        private XPCollection<AuditDataItemPersistent> changeHistory;
        [CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
        public XPCollection<AuditDataItemPersistent> ChangeHistory
        {
            get
            {
                if (changeHistory == null)
                {
                    changeHistory = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return changeHistory;
            }
        }

        #region ORERPRecordDetailsRegion
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
        [ModelDefault("AllowEdit", "False"), ModelDefault("DisplayFormat", "G"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { SetPropertyValue("UpdatedOn", ref updatedOn, value); }
        }

        Employee GetCurrentUser()
        {
            return Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        }

        bool hasUpdate;
        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public bool HasUpdate
        {
            get => hasUpdate;
            set => SetPropertyValue(nameof(HasUpdate), ref hasUpdate, value);
        }

        int version;
        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public int Version
        {
            get => version;
            set => SetPropertyValue(nameof(Version), ref version, value);
        }
        #endregion
    }
}