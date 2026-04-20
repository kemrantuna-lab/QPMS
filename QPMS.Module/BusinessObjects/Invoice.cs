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
    public class Invoice : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Invoice(Session session)
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
            Date = DateTime.Today;
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
                Code = string.Format("INVOICE-{0:D8}", nextSequence);
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


        [RuleRequiredField("RuleRequiredField for Invoice.Company", DefaultContexts.Save, "A company must be specified")]
        [Association("Company-Invoices")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [Association("Customer-Invoices")]
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }

        [Association("Project-Invoices")]
        public XPCollection<Project> Projects
        {
            get
            {
                return GetCollection<Project>(nameof(Projects));
            }
        }

        string paymentInfo;
        DateTime validityDate;
        double currencyRate;
        FileData ınvoiceFile;
        FileData workInstruction;
        FileData workOrder;
        ValidityState validityState;
        PaymentState paymentState;
        string code;
        Cur currency;
        Company company;
        Customer customer;
        double amountDeduction;
        double amountWithoutTaxes;
        double amountWithTaxes;
        double amountGoodsAndServices;
        double invoiceTotal;
        DateTime cancellationDate;
        bool cancelled;
        string invoiceType;
        string scenarioType;
        string receiverEntityName;
        string senderEntityName;
        string receiver;
        string sender;
        string invoiceNo;
        string name;
        DateTime date;

        [RuleRequiredField("RuleRequiredField for Invoice.Date", DefaultContexts.Save, "An invoice date must be specified")]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }


        public DateTime ValidityDate
        {
            get => validityDate;
            set => SetPropertyValue(nameof(ValidityDate), ref validityDate, value);
        }

        
        [Size(SizeAttribute.Unlimited)]
        public string PaymentInfo
        {
            get => paymentInfo;
            set => SetPropertyValue(nameof(PaymentInfo), ref paymentInfo, value);
        }

        [RuleRequiredField("RuleRequiredField for Invoice.Name", DefaultContexts.Save, "A name must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [RuleRequiredField("RuleRequiredField for Invoice.InvoiceNo", DefaultContexts.Save, "An InvoiceNo must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string InvoiceNo
        {
            get => invoiceNo;
            set => SetPropertyValue(nameof(InvoiceNo), ref invoiceNo, value);
        }

        [RuleRequiredField("RuleRequiredField for Invoice.Sender", DefaultContexts.Save, "A sender must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Sender
        {
            get => sender;
            set => SetPropertyValue(nameof(Sender), ref sender, value);
        }

        [RuleRequiredField("RuleRequiredField for Invoice.Receiver", DefaultContexts.Save, "A receiver must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Receiver
        {
            get => receiver;
            set => SetPropertyValue(nameof(Receiver), ref receiver, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SenderEntityName
        {
            get => senderEntityName;
            set => SetPropertyValue(nameof(SenderEntityName), ref senderEntityName, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ReceiverEntityName
        {
            get => receiverEntityName;
            set => SetPropertyValue(nameof(ReceiverEntityName), ref receiverEntityName, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ScenarioType
        {
            get => scenarioType;
            set => SetPropertyValue(nameof(ScenarioType), ref scenarioType, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string InvoiceType
        {
            get => invoiceType;
            set => SetPropertyValue(nameof(InvoiceType), ref invoiceType, value);
        }


        public bool Cancelled
        {
            get => cancelled;
            set => SetPropertyValue(nameof(Cancelled), ref cancelled, value);
        }


        public DateTime CancellationDate
        {
            get => cancellationDate;
            set => SetPropertyValue(nameof(CancellationDate), ref cancellationDate, value);
        }


        public double InvoiceTotal
        {
            get => invoiceTotal;
            set => SetPropertyValue(nameof(InvoiceTotal), ref invoiceTotal, value);
        }


        public double AmountGoodsAndServices
        {
            get => amountGoodsAndServices;
            set => SetPropertyValue(nameof(AmountGoodsAndServices), ref amountGoodsAndServices, value);
        }


        public double AmountWithTaxes
        {
            get => amountWithTaxes;
            set => SetPropertyValue(nameof(AmountWithTaxes), ref amountWithTaxes, value);
        }


        public double AmountWithoutTaxes
        {
            get => amountWithoutTaxes;
            set => SetPropertyValue(nameof(AmountWithoutTaxes), ref amountWithoutTaxes, value);
        }

        MediaDataObject photo;

        [ImageEditor(ListViewImageEditorCustomHeight = 250, DetailViewImageEditorFixedWidth = 500)]
        public MediaDataObject Photo
        {
            get => photo;
            set => SetPropertyValue(nameof(Photo), ref photo, value);
        }
        public double AmountDeduction
        {
            get => amountDeduction;
            set => SetPropertyValue(nameof(AmountDeduction), ref amountDeduction, value);
        }

        
        public double CurrencyRate
        {
            get => currencyRate;
            set => SetPropertyValue(nameof(CurrencyRate), ref currencyRate, value);
        }

        [Association("Cur-Invoices")]
        public Cur Currency
        {
            get => currency;
            set => SetPropertyValue(nameof(Currency), ref currency, value);
        }

        [Association("Invoice-InvoiceProjectPeriods")]
        public XPCollection<InvoiceProjectPeriod> InvoiceProjectPeriods
        {
            get
            {
                return GetCollection<InvoiceProjectPeriod>(nameof(InvoiceProjectPeriods));
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


        [RuleRequiredField("RuleRequiredField for Invoice.PaymentState", DefaultContexts.Save, "Payment State must be selecteds.")]
        public PaymentState PaymentState
        {
            get => paymentState;
            set => SetPropertyValue(nameof(PaymentState), ref paymentState, value);
        }

        [RuleRequiredField("RuleRequiredField for Invoice.ValidityState", DefaultContexts.Save, "Validity State must be selected.")]
        public ValidityState ValidityState
        {
            get => validityState;
            set => SetPropertyValue(nameof(ValidityState), ref validityState, value);
        }


        public FileData WorkOrder
        {
            get => workOrder;
            set => SetPropertyValue(nameof(WorkOrder), ref workOrder, value);
        }


        public FileData WorkInstruction
        {
            get => workInstruction;
            set => SetPropertyValue(nameof(WorkInstruction), ref workInstruction, value);
        }

        
        public FileData InvoiceFile
        {
            get => ınvoiceFile;
            set => SetPropertyValue(nameof(InvoiceFile), ref ınvoiceFile, value);
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
    }

    public enum ValidityState
    {
        ACTIVE,
        CANCELLED
    }

    public enum PaymentState
    {
        PENDING,
        PAID,
        WILLNOTBEPAID
    }


}