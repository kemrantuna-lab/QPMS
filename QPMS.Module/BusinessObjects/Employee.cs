using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace QPMS.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(UserName))]
    public class Employee : Person, ISecurityUser, IAuthenticationStandardUser, IAuthenticationActiveDirectoryUser, ISecurityUserWithRoles, IPermissionPolicyUser, ICanInitialize
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Employee(Session session)
            : base(session)
        {

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            base.AfterConstruction();
            CreatedBy = GetCurrentUser();
            CreatedOn = DateTime.Now;

            byte[] randomBytes = new byte[9];
            new RNGCryptoServiceProvider().GetBytes(randomBytes);
            string password = Convert.ToBase64String(randomBytes);
            this.SetPassword(password);

            Start = DateTime.Today;
            ChangePasswordOnFirstLogon = false;
            IsActive = true;

            try
            {
                if (this.Company == null)
                {
                    Employee currentUser = Session.FindObject<Employee>(
                   new BinaryOperator("Oid", SecuritySystem.CurrentUserId)
                );

                    if (currentUser != null)
                    {
                        try
                        {
                            Company company = Session.FindObject<Company>(
                                new BinaryOperator("Oid", currentUser.Company.Oid)
                            );

                            if (company != null)
                            {
                                Company = company;
                            }
                        } catch
                        {

                        }

                        
                    }
                }
            }
            catch
            {

            }


        }

        protected override void OnSaving()
        {


            if (Country != null)
            {
                Country = Regex.Replace(Country, @"\s", string.Empty).ToUpper();
            }

            if (FirstName != null)
            {
                FirstName = Regex.Replace(FirstName, @"\s", string.Empty).ToUpper();
            }

            if (LastName != null)
            {
                LastName = Regex.Replace(LastName, @"\s", string.Empty).ToUpper();
            }

            if (City != null)
            {
                City = Regex.Replace(City, @"\s", string.Empty).ToUpper();
            }

            if (Address != null)
            {
                Address = Address.ToUpper();
            }

            if (CitizenNo != null)
            {
                CitizenNo = Regex.Replace(CitizenNo, @"\s", string.Empty).ToUpper();
            }


            if (!(Session is NestedUnitOfWork)
                && (Session.DataLayer != null)
                    && Session.IsNewObject(this)
                        && (Session.ObjectLayer is SimpleObjectLayer)
                        //OR
                        //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
                        && string.IsNullOrEmpty(Code))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("EMP-N{0:D6}", nextSequence);
            }

            base.OnSaving();
        }



        #region EmployeeDirectMembers

        [ModelDefault("AllowEdit", "False")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CitizenNo
        {
            get => citizenNo;
            set => SetPropertyValue(nameof(CitizenNo), ref citizenNo, value);
        }

        [ImageEditor(ListViewImageEditorCustomHeight = 250)]
        public MediaDataObject EmployeePhoto
        {
            get => employeePhoto;
            set => SetPropertyValue(nameof(EmployeePhoto), ref employeePhoto, value);
        }

        public DateTime Start
        {
            get => start;
            set => SetPropertyValue(nameof(Start), ref start, value);
        }


        public DateTime End
        {
            get => end;
            set => SetPropertyValue(nameof(End), ref end, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Mobile
        {
            get => mobile;
            set => SetPropertyValue(nameof(Mobile), ref mobile, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LandLine
        {
            get => landLine;
            set => SetPropertyValue(nameof(LandLine), ref landLine, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string EMail
        {
            get => eMail;
            set => SetPropertyValue(nameof(EMail), ref eMail, value);
        }


        [Size(SizeAttribute.Unlimited)]
        public string Address
        {
            get => address;
            set => SetPropertyValue(nameof(Address), ref address, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Zip
        {
            get => zip;
            set => SetPropertyValue(nameof(Zip), ref zip, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string City
        {
            get => city;
            set => SetPropertyValue(nameof(City), ref city, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Country
        {
            get => country;
            set => SetPropertyValue(nameof(Country), ref country, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string EmergencyContactName
        {
            get => emergencyContactName;
            set => SetPropertyValue(nameof(EmergencyContactName), ref emergencyContactName, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string EmergencyContactMobile
        {
            get => emergencyContactMobile;
            set => SetPropertyValue(nameof(EmergencyContactMobile), ref emergencyContactMobile, value);
        }


        public double GrossSalaryMonthly
        {
            get => grossSalaryMonthly;
            set => SetPropertyValue(nameof(GrossSalaryMonthly), ref grossSalaryMonthly, value);
        }

        [DataSourceProperty("Company.CompanyCurrencies")]
        [Association("Cur-EmployeeSalaries")]
        public Cur SalaryCurrency
        {
            get => salaryCurrency;
            set => SetPropertyValue(nameof(SalaryCurrency), ref salaryCurrency, value);
        }


        public double MonthlyHoursOfWork
        {
            get => monthlyHoursOfWork;
            set => SetPropertyValue(nameof(MonthlyHoursOfWork), ref monthlyHoursOfWork, value);
        }

        #endregion

        #region ISecurityUser Members
        [RuleRequiredField("EmployeeUserNameRequired", DefaultContexts.Save)]
        [RuleUniqueValue("EmployeeUserNameIsUnique", DefaultContexts.Save, "The login already exists in the system. Please try another username for the employee")]
        public string UserName
        {
            get => userName;
            set => SetPropertyValue(nameof(UserName), ref userName, value);
        }

        Collar collar;
        Education education;
        EmployeeType employeeType;
        string socialSecurityCode;
        string ıBAN;
        string socialSecurityBranchOfCompany;
        string companyEmail;
        string companyPhoneOffice;
        string companyPhoneMobile;
        string socialSecurityNumber;
        string socialSecurityPositionName;
        MediaDataObject employeePhoto;
        string code;
        Cur salaryCurrency;
        Company company;
        double monthlyHoursOfWork;
        double grossSalaryMonthly;
        string emergencyContactMobile;
        string emergencyContactName;
        string country;
        string city;
        string zip;
        string address;
        string eMail;
        string landLine;
        string mobile;
        DateTime end;
        string citizenNo;
        DateTime start;
        private string userName;
        private bool isActive;

        public bool IsActive
        {
            get => isActive;
            set => SetPropertyValue(nameof(IsActive), ref isActive, value);
        }
        #endregion

        #region IAutehticationStandartUser Members
        private bool changePasswordOnFirstLogon;
        private string storedPassword;



        public bool ChangePasswordOnFirstLogon
        {
            get => changePasswordOnFirstLogon;
            set => SetPropertyValue(nameof(ChangePasswordOnFirstLogon), ref changePasswordOnFirstLogon, value);
        }


        [Browsable(false), Size(SizeAttribute.Unlimited), Persistent, SecurityBrowsable]
        protected string StoredPassword
        {
            get { return storedPassword; }
            set { storedPassword = value; }
        }


        public bool ComparePassword(string password)
        {
            return PasswordCryptographer.VerifyHashedPasswordDelegate(this.storedPassword, password);
        }

        public void SetPassword(string password)
        {
            this.storedPassword = PasswordCryptographer.HashPasswordDelegate(password);
            OnChanged(nameof(StoredPassword));
        }
        #endregion

        #region IAuthenticationActiveDirectoryUser Members

        #endregion

        #region ISecurityUserWithRoles Members

        IList<ISecurityRole> ISecurityUserWithRoles.Roles
        {
            get
            {
                IList<ISecurityRole> result = new List<ISecurityRole>();
                foreach (EmployeeRole role in EmployeeRoles)
                {
                    result.Add(role);
                }

                return result;
            }
        }

        [Association("Employee-EmployeeRoles")]
        [RuleRequiredField("EmployeeRoleIsRequired", DefaultContexts.Save, TargetCriteria = "IsActive", CustomMessageTemplate = "An active employee must have at least one role assigned.")]
        public XPCollection<EmployeeRole> EmployeeRoles
        {
            get
            {
                return GetCollection<EmployeeRole>(nameof(EmployeeRoles));
            }
        }

        #endregion

        #region IPermissionPolicyUser Members
        IEnumerable<IPermissionPolicyRole> IPermissionPolicyUser.Roles
        {
            get
            {
                return EmployeeRoles.OfType<EmployeeRole>();
            }

        }
        #endregion

        #region ICANInitialize Members
        void ICanInitialize.Initialize(IObjectSpace objectSpace, SecurityStrategyComplex security)
        {
            EmployeeRole newUserRole = objectSpace.FirstOrDefault<EmployeeRole>(role => role.Name == security.NewUserRoleName);
            if (newUserRole == null)
            {
                newUserRole = objectSpace.CreateObject<EmployeeRole>();
                newUserRole.Name = security.NewUserRoleName;
                newUserRole.IsAdministrative = true;
                newUserRole.Employees.Add(this);
            }
        }
        #endregion




        #region ORERPRecordDetailsRegion
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
        #endregion


        #region Branch Related Members

        [Association("Employee-Branches")]
        public XPCollection<Branch> Branches
        {
            get
            {
                return GetCollection<Branch>(nameof(Branches));
            }
        }

        [Association("Employee-BranchTimesheets")]
        public XPCollection<BranchTimesheet> BranchTimesheets
        {
            get
            {
                return GetCollection<BranchTimesheet>(nameof(BranchTimesheets));
            }
        }

        #endregion


        #region EmployeeRelated Members
        [Association("Employee-Files")]
        public XPCollection<EmployeeFile> Files
        {
            get
            {
                return GetCollection<EmployeeFile>(nameof(Files));
            }
        }


        [Association("Employee-Forms")]
        public XPCollection<EmployeeForm> Forms
        {
            get
            {
                return GetCollection<EmployeeForm>(nameof(Forms));
            }
        }

        [Association("Employee-NotesAboutEmloyee")]
        public XPCollection<EmployeeAboutNote> NotesAboutEmloyee
        {
            get
            {
                return GetCollection<EmployeeAboutNote>(nameof(NotesAboutEmloyee));
            }
        }

        [Association("Employee-MyNotes")]
        public XPCollection<MyNote> MyNotes
        {
            get
            {
                return GetCollection<MyNote>(nameof(MyNotes));
            }
        }

        [Association("Employee-Tasks")]
        public XPCollection<MyTask> Tasks
        {
            get
            {
                return GetCollection<MyTask>(nameof(Tasks));
            }
        }

        #endregion

        #region ProjectMembers
        [Association("Employee-Projects")]
        public XPCollection<Project> Projects
        {
            get
            {
                return GetCollection<Project>(nameof(Projects));
            }
        }

        [Association("Employee-ProjectTimesheets")]
        public XPCollection<ProjectTimesheet> ProjectTimesheets
        {
            get
            {
                return GetCollection<ProjectTimesheet>(nameof(ProjectTimesheets));
            }
        }

        #endregion

        #region CompanyMembers

        [Association("Company-Employees")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        CompanyMobilePhone companyMobile;
        [DataSourceProperty("Company.MobilePhones")]
        public CompanyMobilePhone CompanyMobile
        {
            get
            {
                return companyMobile;
            }
            set
            {
                if (companyMobile == value)
                    return;

                // Store a reference to the former user.
                CompanyMobilePhone previousMobile = companyMobile;
                previousMobile = value;

                if (IsLoading) return;

                if (previousMobile != null && previousMobile.User == this)
                {
                    previousMobile.User = null;
                }

                if (companyMobile != null)
                {
                    companyMobile.User = this;
                }
                OnChanged(nameof(CompanyMobile));
            }
        }

        Car car;
        [DataSourceProperty("Company.Cars")]
        //[Association("Car-ApplicationUsers")]
        public Car Car
        {
            get { return car; }
            set
            {
                if (car == value)
                    return;

                // Store a reference to the former user.
                Car prevCar = car;
                car = value;

                if (IsLoading) return;

                // Remove an users's reference to this car, if exists.
                if (prevCar != null && prevCar.User == this)
                    prevCar.User = null;

                // Specify that the car is a new user's car.
                if (car != null)
                    car.User = this;
                OnChanged(nameof(Car));
            }
        }
        #endregion

        [Association("Employee-Events")]
        public XPCollection<EmployeeEvent> Events
        {
            get
            {
                return GetCollection<EmployeeEvent>(nameof(Events));
            }
        }


        [Association("Employee-Expenses")]
        public XPCollection<EmployeeExpense> Expenses
        {
            get
            {
                return GetCollection<EmployeeExpense>(nameof(Expenses));
            }
        }


        EmployeeQuit employeeQuit = null;
        public EmployeeQuit EmployeeQuit
        {
            get { return employeeQuit; }
            set
            {
                if (employeeQuit == value)
                    return;

                // Store a reference to the person's former house.
                EmployeeQuit previousQuit = employeeQuit;
                employeeQuit = value;

                if (IsLoading) return;

                // Remove a reference to the house's owner, if the person is its owner.
                if (previousQuit != null && previousQuit.Employee == this)
                    previousQuit.Employee = null;

                // Specify the person as a new owner of the house.
                if (employeeQuit != null)
                    employeeQuit.Employee = this;

                OnChanged(nameof(EmployeeQuit));
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SocialSecurityPositionName
        {
            get => socialSecurityPositionName;
            set => SetPropertyValue(nameof(SocialSecurityPositionName), ref socialSecurityPositionName, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SocialSecurityNumber
        {
            get => socialSecurityNumber;
            set => SetPropertyValue(nameof(SocialSecurityNumber), ref socialSecurityNumber, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SocialSecurityCode
        {
            get => socialSecurityCode;
            set => SetPropertyValue(nameof(SocialSecurityCode), ref socialSecurityCode, value);
        }

        /***
         * An employee can have only one social security branch
         * 
         * **/
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SocialSecurityBranchOfCompany
        {
            get => socialSecurityBranchOfCompany;
            set => SetPropertyValue(nameof(SocialSecurityBranchOfCompany), ref socialSecurityBranchOfCompany, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CompanyPhoneMobile
        {
            get => companyPhoneMobile;
            set => SetPropertyValue(nameof(CompanyPhoneMobile), ref companyPhoneMobile, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CompanyPhoneOffice
        {
            get => companyPhoneOffice;
            set => SetPropertyValue(nameof(CompanyPhoneOffice), ref companyPhoneOffice, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CompanyEmail
        {
            get => companyEmail;
            set => SetPropertyValue(nameof(CompanyEmail), ref companyEmail, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string IBAN
        {
            get => ıBAN;
            set => SetPropertyValue(nameof(IBAN), ref ıBAN, value);
        }

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


        public EmployeeType EmployeeType
        {
            get => employeeType;
            set => SetPropertyValue(nameof(EmployeeType), ref employeeType, value);
        }


        public Education Education
        {
            get => education;
            set => SetPropertyValue(nameof(Education), ref education, value);
        }

        
        public Collar Collar
        {
            get => collar;
            set => SetPropertyValue(nameof(Collar), ref collar, value);
        }
    }

    public enum Collar
    {
        MAVI,
        BEYAZ,
        TURUNCU
    }

    public enum EmployeeType
    {
        NORMAL,
        EMEKLI,
        ENGELLI
    }

    public enum Education
    {
        LISANS,
        ONLISANS,
        MESLEKLISESI,
        DUZLISE,
        LISEDIGER,
        ORTAOKUL,
        ILKOKUL
    }
}
