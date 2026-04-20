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
    public class Cur : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Cur(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork)
            && (Session.DataLayer != null)
            && Session.IsNewObject(this)
            && (Session.ObjectLayer is SimpleObjectLayer)
            //OR
            //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
            && string.IsNullOrEmpty(Code))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("CUR-{0:D8}", nextSequence);
            }
            base.OnSaving();
        }


        [Association("Cur-Expenses")]
        public XPCollection<Expense> Expenses
        {
            get
            {
                return GetCollection<Expense>(nameof(Expenses));
            }
        }


        [Association("Company-CompanyCurrencies")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
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
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }



        [Association("Cur-Invoices")]
        public XPCollection<Invoice> Invoices
        {
            get
            {
                return GetCollection<Invoice>(nameof(Invoices));
            }
        }


        [Association("Cur-ProjectTimesheetTypes")]
        public XPCollection<ProjectTimesheetType> ProjectTimesheetTypes
        {
            get
            {
                return GetCollection<ProjectTimesheetType>(nameof(ProjectTimesheetTypes));
            }
        }

        [Association("Cur-Parts")]
        public XPCollection<Part> Parts
        {
            get
            {
                return GetCollection<Part>(nameof(Parts));
            }
        }

        [Association("Cur-EmployeeSalaries")]
        public XPCollection<Employee> EmployeeSalaries
        {
            get
            {
                return GetCollection<Employee>(nameof(EmployeeSalaries));
            }
        }

        [Association("Cur-CarMaintenances")]
        public XPCollection<CarMaintenance> CarMaintenances
        {
            get
            {
                return GetCollection<CarMaintenance>(nameof(CarMaintenances));
            }
        }

        [Association("Cur-Projects")]
        public XPCollection<Project> Projects
        {
            get
            {
                return GetCollection<Project>(nameof(Projects));
            }
        }

        [Association("Cur-ProjectCosts")]
        public XPCollection<ProjectCost> ProjectCosts
        {
            get
            {
                return GetCollection<ProjectCost>(nameof(ProjectCosts));
            }
        }
    }
}