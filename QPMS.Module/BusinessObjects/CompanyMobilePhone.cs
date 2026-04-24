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
    public class CompanyMobilePhone : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public CompanyMobilePhone(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}


        string purchaseCurrency;
        double purchasePrice;
        string oS;
        string model;
        string brand;
        string invoiceNumber;
        DateTime purchaseDate;
        string iMEI;
        Company company;

        [RuleRequiredField("RuleRequiredField for CompanyMobilePhone.Company", DefaultContexts.Save, "A company must be specified")]
        [Association("Company-MobilePhones")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyMobilePhone.IMEI", DefaultContexts.Save, "An IMEI must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string IMEI
        {
            get => iMEI;
            set => SetPropertyValue(nameof(IMEI), ref iMEI, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyMobilePhone.PurchaseDate", DefaultContexts.Save, "An purchase date must be specified")]
        public DateTime PurchaseDate
        {
            get => purchaseDate;
            set => SetPropertyValue(nameof(PurchaseDate), ref purchaseDate, value);
        }

        public double PurchasePrice
        {
            get => purchasePrice;
            set => SetPropertyValue(nameof(PurchasePrice), ref purchasePrice, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyMobilePhone.PurchaseCurrency", DefaultContexts.Save, "The purchase currency number must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PurchaseCurrency
        {
            get => purchaseCurrency;
            set => SetPropertyValue(nameof(PurchaseCurrency), ref purchaseCurrency, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyMobilePhone.InvoiceNumber", DefaultContexts.Save, "The purchase invoice number must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string InvoiceNumber
        {
            get => invoiceNumber;
            set => SetPropertyValue(nameof(InvoiceNumber), ref invoiceNumber, value);
        }


        [RuleRequiredField("RuleRequiredField for CompanyMobilePhone.Brand", DefaultContexts.Save, "A brand must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Brand
        {
            get => brand;
            set => SetPropertyValue(nameof(Brand), ref brand, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyMobilePhone.Model", DefaultContexts.Save, "A model must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Model
        {
            get => model;
            set => SetPropertyValue(nameof(Model), ref model, value);
        }

        MediaDataObject photo;
        [ImageEditor(ListViewImageEditorCustomHeight = 250, DetailViewImageEditorFixedWidth = 250)]
        public MediaDataObject Photo
        {
            get => photo;
            set => SetPropertyValue(nameof(Photo), ref photo, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string OS
        {
            get => oS;
            set => SetPropertyValue(nameof(OS), ref oS, value);
        }

        [Association("CompanyMobilePhone-ApplicationUserCZTS")]
        public XPCollection<EmployeeFormCZT> ApplicationUserCZTS
        {
            get
            {
                return GetCollection<EmployeeFormCZT>(nameof(ApplicationUserCZTS));
            }
        }


        Employee user = null;

        [DataSourceProperty("Company.Employees")]
        public Employee User
        {
            get { return user; }
            set
            {
                if (user == value)
                    return;

                // Store a reference to the former user.
                Employee prevUser = user;
                user = value;

                if (IsLoading) return;

                // Remove an users's reference to this car, if exists.
                if (prevUser != null && prevUser.CompanyMobile == this)
                    prevUser.CompanyMobile = null;

                // Specify that the car is a new user's car.
                if (user != null)
                    user.CompanyMobile = this;
                OnChanged(nameof(User));
            }
        }
    }
}
