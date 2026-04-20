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
    public class Expense : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Expense(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            CreatedBy = GetCurrentUser();
            CreatedOn = DateTime.Now;
            HasUpdate = true;
            Version = 0;
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
                Code = string.Format("EXPENSE-{0:D8}", nextSequence);
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

        [Association("Company-Expenses")]
        public Company OwnerCompany
        {
            get => ownerCompany;
            set => SetPropertyValue(nameof(OwnerCompany), ref ownerCompany, value);
        }

        [RuleRequiredField("RuleRequiredField for Expense.Date", DefaultContexts.Save, "A date must be specified")]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        [RuleRequiredField("RuleRequiredField for Expense.InvoiceNo", DefaultContexts.Save, "A number must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string InvoiceNo
        {
            get => invoiceNo;
            set => SetPropertyValue(nameof(InvoiceNo), ref invoiceNo, value);
        }

        [RuleRequiredField("RuleRequiredField for Expense.Country", DefaultContexts.Save, "A country must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Country
        {
            get => country;
            set => SetPropertyValue(nameof(Country), ref country, value);
        }

        [RuleRequiredField("RuleRequiredField for Expense.City", DefaultContexts.Save, "A city must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string City
        {
            get => city;
            set => SetPropertyValue(nameof(City), ref city, value);
        }

        [RuleRequiredField("RuleRequiredField for Expense.Company", DefaultContexts.Save, "A company must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CompanyName
        {
            get => companyName;
            set => SetPropertyValue(nameof(CompanyName), ref companyName, value);
        }

        [RuleRequiredField("RuleRequiredField for Expense.Description", DefaultContexts.Save, "A description must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }


        public double Total
        {
            get => total;
            set => SetPropertyValue(nameof(Total), ref total, value);
        }


        public double VAT
        {
            get => vAT;
            set => SetPropertyValue(nameof(VAT), ref vAT, value);
        }

        [RuleRequiredField("RuleRequiredField for Expense.GrandTotal", DefaultContexts.Save, "Grand Total must be specified")]
        public double GrandTotal
        {
            get => grandTotal;
            set => SetPropertyValue(nameof(GrandTotal), ref grandTotal, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string BranchCode
        {
            get => branchCode;
            set => SetPropertyValue(nameof(BranchCode), ref branchCode, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string OwnerCompanyCode
        {
            get => ownerCompanyCode;
            set => SetPropertyValue(nameof(OwnerCompanyCode), ref ownerCompanyCode, value);
        }

        [RuleRequiredField("RuleRequiredField for Expense.Currency", DefaultContexts.Save, "A currency must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CostCenterCode
        {
            get => costCenterCode;
            set => SetPropertyValue(nameof(CostCenterCode), ref costCenterCode, value);
        }

        [RuleRequiredField("RuleRequiredField for Expense.IsEquipmentInvoice", DefaultContexts.Save, "If equipment must be specified")]
        public bool IsEquipmentInvoice
        {
            get => isEquipmentInvoice;
            set => SetPropertyValue(nameof(IsEquipmentInvoice), ref isEquipmentInvoice, value);
        }


        #region ORERPRecordDetailsRegion
        string type;
        bool isEquipmentInvoice;
        string code;
        Cur currency;
        Company ownerCompany;
        string costCenterCode;
        string ownerCompanyCode;
        string branchCode;
        string city;
        string country;
        string invoiceNo;
        string description;
        string companyName;
        double grandTotal;
        double vAT;
        double total;
        DateTime date;
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


        [Association("Cur-Expenses")]
        public Cur Currency
        {
            get => currency;
            set => SetPropertyValue(nameof(Currency), ref currency, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Type
        {
            get => type;
            set => SetPropertyValue(nameof(Type), ref type, value);
        }
    }

}