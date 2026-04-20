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
    public class Interview : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Interview(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

            CreatedBy = GetCurrentUser();
            CreatedOn = DateTime.Now;
            Version = 1;
            HasUpdate = true;
        }

        protected override void OnSaving()
        {

            UpdatedBy = GetCurrentUser();
            UpdatedOn = DateTime.Now;
            HasUpdate = true;
            Version = Version + 1;


            if (!(Session is NestedUnitOfWork)
               && (Session.DataLayer != null)
                   && Session.IsNewObject(this)
                   && (Session.ObjectLayer is SimpleObjectLayer)
           //OR
           //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
           && string.IsNullOrEmpty(Code))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("INTERVIEW-{0:D8}", nextSequence);
            }

            base.OnSaving();
        }


        [ModelDefault("AllowEdit", "False")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }


        string code;
        Company company;
        Result result;
        string eMail;
        string phone;
        string mobile;
        string surname;
        string name;
        string note;
        DateTime date;

        //[RuleRequiredField("RuleRequiredField for Interview.Company", DefaultContexts.Save, "An company must be specified")]
        [Association("Company-Interviews")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [Association("Branch-Interviews")]
        public XPCollection<Branch> Branch
        {
            get
            {
                return GetCollection<Branch>(nameof(Branch));
            }
        }

        [RuleRequiredField("RuleRequiredField for Interview.Date", DefaultContexts.Save, "An date must be specified")]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        [RuleRequiredField("RuleRequiredField for Interview.Name", DefaultContexts.Save, "A name must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [RuleRequiredField("RuleRequiredField for Interview.Surname", DefaultContexts.Save, "A name must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Surname
        {
            get => surname;
            set => SetPropertyValue(nameof(Surname), ref surname, value);
        }

        [RuleRequiredField("RuleRequiredField for Interview.Mobile", DefaultContexts.Save, "A name must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Mobile
        {
            get => mobile;
            set => SetPropertyValue(nameof(Mobile), ref mobile, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Phone
        {
            get => phone;
            set => SetPropertyValue(nameof(Phone), ref phone, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string EMail
        {
            get => eMail;
            set => SetPropertyValue(nameof(EMail), ref eMail, value);
        }

        [RuleRequiredField("RuleRequiredField for Interview.Note", DefaultContexts.Save, "A name must be specified")]
        [Size(SizeAttribute.Unlimited)]
        public string Note
        {
            get => note;
            set => SetPropertyValue(nameof(Note), ref note, value);
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
        [ModelDefault("AllowEdit", "False"), ModelDefault("DisplayFormat", "G"), ModelDefault("IsVisibleInListView", "False")]
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

        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False")]
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

        public Result Result
        {
            get => result;
            set => SetPropertyValue(nameof(Result), ref result, value);
        }

        [Association("Interview-Files")]
        public XPCollection<InterviewFile> Files
        {
            get
            {
                return GetCollection<InterviewFile>(nameof(Files));
            }
        }
    }

    public enum Result
    {
        BAD,
        OK,
        GOOD,
        PERFECT,
        TALENT
    }
}