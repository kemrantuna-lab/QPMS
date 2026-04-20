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
    public class Offer : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Offer(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

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
                Code = string.Format("OFFER-{0:D8}", nextSequence);
            }

            base.OnSaving();
        }

        string customerContactName;
        string hourlyRateSupplierInternationalCurrency;
        double hourlyRateSupplierInternational;
        double hourlyRateSupplierLocal;
        string currency;
        double forkliftRate;
        double kMRate;
        double mealRate;
        double hourlyRate;
        string code;
        MediaDataObject document;
        DateTime date;
        Customer customer;

        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "True"), ModelDefault("IsVisibleInDetailView", "True")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        [RuleRequiredField("RuleRequiredField for Offer.Customer", DefaultContexts.Save, "A customer must be specified")]
        [Association("Customer-Offers")]
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }

        [RuleRequiredField("RuleRequiredField for Offer.CustomerContactName", DefaultContexts.Save, "A customer contact name must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CustomerContactName
        {
            get => customerContactName;
            set => SetPropertyValue(nameof(CustomerContactName), ref customerContactName, value);
        }

        [RuleRequiredField("RuleRequiredField for Offer.Date", DefaultContexts.Save, "A date must be specified")]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        [RuleRequiredField("RuleRequiredField for Offer.Currency", DefaultContexts.Save, "A currency must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Currency
        {
            get => currency;
            set => SetPropertyValue(nameof(Currency), ref currency, value);
        }


        public double HourlyRate
        {
            get => hourlyRate;
            set => SetPropertyValue(nameof(HourlyRate), ref hourlyRate, value);
        }


        public double HourlyRateSupplierLocal
        {
            get => hourlyRateSupplierLocal;
            set => SetPropertyValue(nameof(HourlyRateSupplierLocal), ref hourlyRateSupplierLocal, value);
        }


        public double HourlyRateSupplierInternational
        {
            get => hourlyRateSupplierInternational;
            set => SetPropertyValue(nameof(HourlyRateSupplierInternational), ref hourlyRateSupplierInternational, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string HourlyRateSupplierInternationalCurrency
        {
            get => hourlyRateSupplierInternationalCurrency;
            set => SetPropertyValue(nameof(HourlyRateSupplierInternationalCurrency), ref hourlyRateSupplierInternationalCurrency, value);
        }

        public double MealRate
        {
            get => mealRate;
            set => SetPropertyValue(nameof(MealRate), ref mealRate, value);
        }


        public double KMRate
        {
            get => kMRate;
            set => SetPropertyValue(nameof(KMRate), ref kMRate, value);
        }

        
        public double ForkliftRate
        {
            get => forkliftRate;
            set => SetPropertyValue(nameof(ForkliftRate), ref forkliftRate, value);
        }

        public MediaDataObject Document
        {
            get => document;
            set => SetPropertyValue(nameof(Document), ref document, value);
        }

        [Association("Offer-OfferDetails")]
        public XPCollection<OfferDetail> OfferDetails
        {
            get
            {
                return GetCollection<OfferDetail>(nameof(OfferDetails));
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
}