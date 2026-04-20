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
    public class Car : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Car(Session session)
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
            Name = LicenceTag;
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
                            Code = string.Format("VEHICLE-{0:D8}", nextSequence);
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


        string fuelType;
        string code;
        string name;
        string documentNumber;
        string saleCurrency;
        double salePrice;
        string purchaseCurrency;
        double purchasePrice;
        string vehicleTraceabilityNumber;
        DateTime saleDate;
        DateTime purchaseDate;
        string costCenterCode;
        Company company;
        int year;
        MediaDataObject document;
        string brand;
        string model;
        string licenceTag;

        [RuleRequiredField("RuleRequiredField for Car.Company", DefaultContexts.Save, "A company must be specified")]
        [Association("Company-Cars")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [RuleRequiredField("RuleRequiredField for Car.DocumentNumber", DefaultContexts.Save, "A document number must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string DocumentNumber
        {
            get => documentNumber;
            set => SetPropertyValue(nameof(DocumentNumber), ref documentNumber, value);
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
                if (prevUser != null && prevUser.Car == this)
                    prevUser.Car = null;

                // Specify that the car is a new user's car.
                if (user != null)
                    user.Car = this;
                OnChanged(nameof(User));
            }
        }


        [RuleRequiredField("RuleRequiredField for Car.LicenceTag", DefaultContexts.Save, "A licence tag must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LicenceTag
        {
            get => licenceTag;
            set => SetPropertyValue(nameof(LicenceTag), ref licenceTag, value);
        }

        [RuleRequiredField("RuleRequiredField for Car.Brand", DefaultContexts.Save, "A brand must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Brand
        {
            get => brand;
            set => SetPropertyValue(nameof(Brand), ref brand, value);
        }

        [RuleRequiredField("RuleRequiredField for Car.Model", DefaultContexts.Save, "A model must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Model
        {
            get => model;
            set => SetPropertyValue(nameof(Model), ref model, value);
        }

        [RuleRequiredField("RuleRequiredField for Car.Year", DefaultContexts.Save, "A year must be specified")]
        public int Year
        {
            get => year;
            set => SetPropertyValue(nameof(Year), ref year, value);
        }

        [RuleRequiredField("RuleRequiredField for Car.PurchaseDate", DefaultContexts.Save, "A purchase date must be specified")]
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

        [RuleRequiredField("RuleRequiredField for Car.PurchaseCurrency", DefaultContexts.Save, "A currency must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PurchaseCurrency
        {
            get => purchaseCurrency;
            set => SetPropertyValue(nameof(PurchaseCurrency), ref purchaseCurrency, value);
        }

        public DateTime SaleDate
        {
            get => saleDate;
            set => SetPropertyValue(nameof(SaleDate), ref saleDate, value);
        }


        public double SalePrice
        {
            get => salePrice;
            set => SetPropertyValue(nameof(SalePrice), ref salePrice, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SaleCurrency
        {
            get => saleCurrency;
            set => SetPropertyValue(nameof(SaleCurrency), ref saleCurrency, value);
        }



        [RuleRequiredField("RuleRequiredField for Car.Document", DefaultContexts.Save, "A document must be specified")]
        public MediaDataObject Document
        {
            get => document;
            set => SetPropertyValue(nameof(Document), ref document, value);
        }

        [RuleRequiredField("RuleRequiredField for Car.VehicleTraceabilityNumber", DefaultContexts.Save, "A VIN must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string VehicleTraceabilityNumber
        {
            get => vehicleTraceabilityNumber;
            set => SetPropertyValue(nameof(VehicleTraceabilityNumber), ref vehicleTraceabilityNumber, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string FuelType
        {
            get => fuelType;
            set => SetPropertyValue(nameof(FuelType), ref fuelType, value);
        }

        [Association("Car-AZTS")]
        public XPCollection<EmployeeFormAZT> AZTS
        {
            get
            {
                return GetCollection<EmployeeFormAZT>(nameof(AZTS));
            }
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

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CostCenterCode
        {
            get => costCenterCode;
            set => SetPropertyValue(nameof(CostCenterCode), ref costCenterCode, value);
        }

        #endregion

        [Association("Car-CarMaintenances")]
        public XPCollection<CarMaintenance> CarMaintenances
        {
            get
            {
                return GetCollection<CarMaintenance>(nameof(CarMaintenances));
            }
        }

        [Association("Car-CarSituationForms")]
        public XPCollection<CarSituationForm> EmployeeRoles
        {
            get
            {
                return GetCollection<CarSituationForm>(nameof(EmployeeRoles));
            }
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
}