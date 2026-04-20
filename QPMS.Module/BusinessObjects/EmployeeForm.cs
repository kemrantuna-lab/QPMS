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
    public class EmployeeForm : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public EmployeeForm(Session session)
            : base(session)
        {
        }

        [RuleRequiredField("RuleRequiredField for EmployeeForm.FormName", DefaultContexts.Save, "A form name must be specified")]
        [ModelDefault("AllowEdit", "False")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string FormName
        {
            get => formName;
            set => SetPropertyValue(nameof(FormName), ref formName, value);
        }

        [RuleRequiredField("RuleRequiredField for EmployeeForm.Employee", DefaultContexts.Save, "An employee must be specified")]
        [Association("Employee-Forms")]
        public Employee Employee
        {
            get => employee;
            set => SetPropertyValue(nameof(Employee), ref employee, value);
        }

        [RuleRequiredField("RuleRequiredField for EmployeeForm.Date", DefaultContexts.Save, "A date must be specified")]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            CreatedBy = GetCurrentUser();
            CreatedOn = DateTime.Now;
            HasUpdate = true;
            Version = 0;

            Date = DateTime.Today;

            Employee = GetCurrentUser();

            try
            {
                var userCurrent = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);

                if(userCurrent != null)
                {
                    Company = userCurrent.Company;
                }
            } catch (Exception ex)
            {

            }

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
                Code = string.Format("EFO-{0:D8}", nextSequence);
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

        
        public MediaDataObject Document
        {
            get => document;
            set => SetPropertyValue(nameof(Document), ref document, value);
        }

        #region ORERPRecordDetailsRegion


        [ModelDefault("AllowEdit", "False")]
        [Association("Company-EmployeeForms")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [DataSourceProperty("Company.Branches")]
        [Association("Branch-EmployeeForms")]
        public Branch Branch
        {
            get => branch;
            set => SetPropertyValue(nameof(Branch), ref branch, value);
        }

        Branch branch;
        Company company;
        MediaDataObject document;
        string code;
        DateTime date;
        Employee employee;
        string formName;
        bool hasUpdate;
        int version;
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

        MediaDataObject photo;

        [ImageEditor(ListViewImageEditorCustomHeight = 250, DetailViewImageEditorFixedWidth = 250)]
        public MediaDataObject Photo
        {
            get => photo;
            set => SetPropertyValue(nameof(Photo), ref photo, value);
        }

        #endregion
    }
}