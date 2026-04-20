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
    public class Company : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Company(Session session)
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

            IsDefaultRolesCreated = false;

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

            && string.IsNullOrEmpty(Code))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("COM-{0:D8}", nextSequence);
            }

            if(Code == null)
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("COM-{0:D8}", nextSequence);
            }

            base.OnSaving();
        }
       

        bool isDefaultRolesCreated;
        string fullName;
        MediaDataObject photo;

        [ImageEditor(ListViewImageEditorCustomHeight = 250, DetailViewImageEditorFixedWidth = 250)]
        public MediaDataObject Photo
        {
            get => photo;
            set => SetPropertyValue(nameof(Photo), ref photo, value);
        }

        MediaDataObject logo;


        [ImageEditor(ListViewImageEditorCustomHeight = 250, DetailViewImageEditorFixedWidth = 250)]
        public MediaDataObject Logo
        {
            get => photo;
            set => SetPropertyValue(nameof(Logo), ref logo, value);
        }

        string code;
        string name;

        [RuleRequiredField("RuleRequiredField for Company.Name", DefaultContexts.Save, "A name must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [RuleRequiredField("RuleRequiredField for Company.FullName", DefaultContexts.Save, "A full name must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string FullName
        {
            get => fullName;
            set => SetPropertyValue(nameof(FullName), ref fullName, value);
        }

        [ModelDefault("AllowEdit", "False")]
        //[RuleRequiredField("RuleRequiredField for Company.Code", DefaultContexts.Save, "A code must be specified for company")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        [Association("Company-Employees")]
        public XPCollection<Employee> Employees
        {
            get
            {
                return GetCollection<Employee>(nameof(Employees));
            }
        }

        [Association("Company-Branches")]
        public XPCollection<Branch> Branches
        {
            get
            {
                return GetCollection<Branch>(nameof(Branches));
            }
        }

        [Association("Company-SocialSecurityBranchesOfCompany")]
        public XPCollection<SocialSecurityBranchOfCompany> SocialSecurityBranchesOfCompany
        {
            get
            {
                return GetCollection<SocialSecurityBranchOfCompany>(nameof(SocialSecurityBranchesOfCompany));
            }
        }

        [Association("Company-Projects")]
        public XPCollection<Project> Projects
        {
            get
            {
                return GetCollection<Project>(nameof(Projects));
            }
        }


        [Association("Company-Customers")]
        public XPCollection<Customer> Customers
        {
            get
            {
                return GetCollection<Customer>(nameof(Customers));
            }
        }

        [Association("Company-Invoices")]
        public XPCollection<Invoice> Invoices
        {
            get
            {
                return GetCollection<Invoice>(nameof(Invoices));
            }
        }

        [Association("Company-CostCenters")]
        public XPCollection<CostCenter> CostCenters
        {
            get
            {
                return GetCollection<CostCenter>(nameof(CostCenters));
            }
        }

        [Association("Company-Cars")]
        public XPCollection<Car> Cars
        {
            get
            {
                return GetCollection<Car>(nameof(Cars));
            }
        }

        [Association("Company-Expenses")]
        public XPCollection<Expense> Expenses
        {
            get
            {
                return GetCollection<Expense>(nameof(Expenses));
            }
        }

        [Association("Company-Offices")]
        public XPCollection<CompanyOffice> Offices
        {
            get
            {
                return GetCollection<CompanyOffice>(nameof(Offices));
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

        [Association("Company-Interviews")]
        public XPCollection<Interview> Interviews
        {
            get
            {
                return GetCollection<Interview>(nameof(Interviews));
            }
        }

        #endregion

        [Association("Company-CompanyPresences")]
        public XPCollection<CompanyPresence> CompanyPresences
        {
            get
            {
                return GetCollection<CompanyPresence>(nameof(CompanyPresences));
            }
        }

        [Association("Company-CompanyAbsences")]
        public XPCollection<CompanyAbsence> CompanyAbsences
        {
            get
            {
                return GetCollection<CompanyAbsence>(nameof(CompanyAbsences));
            }
        }

        [Association("Company-MobilePhones")]
        public XPCollection<CompanyMobilePhone> MobilePhones
        {
            get
            {
                return GetCollection<CompanyMobilePhone>(nameof(MobilePhones));
            }
        }

        [Association("Company-Departments")]
        public XPCollection<Department> Departments
        {
            get
            {
                return GetCollection<Department>(nameof(Departments));
            }
        }

        [Association("Company-CompanyCurrencies")]
        public XPCollection<Cur> CompanyCurrencies
        {
            get
            {
                return GetCollection<Cur>(nameof(CompanyCurrencies));
            }
        }


        [Association("Company-EmployeeRoles")]
        public XPCollection<EmployeeRole> EmployeeRoles
        {
            get
            {
                return GetCollection<EmployeeRole>(nameof(EmployeeRoles));
            }
        }

        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public bool IsDefaultRolesCreated
        {
            get => isDefaultRolesCreated;
            set => SetPropertyValue(nameof(IsDefaultRolesCreated), ref isDefaultRolesCreated, value);
        }

        [Association("Company-Properties")]
        public XPCollection<CompanyProperty> Properties
        {
            get
            {
                return GetCollection<CompanyProperty>(nameof(Properties));
            }
        }

        [Association("Company-BranchTimesheetStartHours")]
        public XPCollection<BranchTimesheetStartHour> BranchTimesheetStartHours
        {
            get
            {
                return GetCollection<BranchTimesheetStartHour>(nameof(BranchTimesheetStartHours));
            }
        }

        [Association("Company-BranchTimesheetStartMinutes")]
        public XPCollection<BranchTimesheetStartMinute> BranchTimesheetStartMinutes
        {
            get
            {
                return GetCollection<BranchTimesheetStartMinute>(nameof(BranchTimesheetStartMinutes));
            }
        }

        [Association("Company-BranchTimesheetEndHours")]
        public XPCollection<BranchTimesheetEndHour> BranchTimesheetEndHours
        {
            get
            {
                return GetCollection<BranchTimesheetEndHour>(nameof(BranchTimesheetEndHours));
            }
        }

        [Association("Company-BranchTimesheetEndMinutes")]
        public XPCollection<BranchTimesheetEndMinute> BranchTimesheetEndMinutes
        {
            get
            {
                return GetCollection<BranchTimesheetEndMinute>(nameof(BranchTimesheetEndMinutes));
            }
        }


        [Association("Company-ProjectTimesheetStartHours")]
        public XPCollection<ProjectTimesheetStartHour> ProjectTimesheetStartHours
        {
            get
            {
                return GetCollection<ProjectTimesheetStartHour>(nameof(ProjectTimesheetStartHours));
            }
        }

        [Association("Company-ProjectTimesheetStartMinutes")]
        public XPCollection<ProjectTimesheetStartMinute> ProjectTimesheetStartMinutes
        {
            get
            {
                return GetCollection<ProjectTimesheetStartMinute>(nameof(ProjectTimesheetStartMinutes));
            }
        }

        [Association("Company-ProjectTimesheetEndHours")]
        public XPCollection<ProjectTimesheetEndHour> ProjectTimesheetEndHours
        {
            get
            {
                return GetCollection<ProjectTimesheetEndHour>(nameof(ProjectTimesheetEndHours));
            }
        }

        [Association("Company-ProjectTimesheetEndMinutes")]
        public XPCollection<ProjectTimesheetEndMinute> ProjectTimesheetEndMinutes
        {
            get
            {
                return GetCollection<ProjectTimesheetEndMinute>(nameof(ProjectTimesheetEndMinutes));
            }
        }

        [Association("Company-Reports")]
        public XPCollection<QPMSReport> Reports
        {
            get
            {
                return GetCollection<QPMSReport>(nameof(Reports));
            }
        }

        [Association("Company-EmployeeForms")]
        public XPCollection<EmployeeForm> EmployeeForms
        {
            get
            {
                return GetCollection<EmployeeForm>(nameof(EmployeeForms));
            }
        }

        [Association("Company-EmployeeQuits")]
        public XPCollection<EmployeeQuit> EmployeeQuits
        {
            get
            {
                return GetCollection<EmployeeQuit>(nameof(EmployeeQuits));
            }
        }

        [Association("Company-ProjectEndInformations")]
        public XPCollection<ProjectEndInfo> ProjectEndInformations
        {
            get
            {
                return GetCollection<ProjectEndInfo>(nameof(ProjectEndInformations));
            }
        }

        [Association("Company-Computers")]
        public XPCollection<Computer> Computers
        {
            get
            {
                return GetCollection<Computer>(nameof(Computers));
            }
        }

        [Association("Company-CompanyRentals")]
        public XPCollection<CompanyRental> CompanyRentals
        {
            get
            {
                return GetCollection<CompanyRental>(nameof(CompanyRentals));
            }
        }
    }
}