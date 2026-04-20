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
    public class CompanyOffice : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public CompanyOffice(Session session)
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

            OwnProperty = false;
        }

        protected override void OnSaving()
        {
            UpdatedBy = GetCurrentUser();
            UpdatedOn = DateTime.Now;
            Version = Version + 1;
            HasUpdate = true;
            base.OnSaving();

            if (!(Session is NestedUnitOfWork)
            && (Session.DataLayer != null)
            && Session.IsNewObject(this)
            && (Session.ObjectLayer is SimpleObjectLayer)

            && string.IsNullOrEmpty(Code))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("COMOFFICE-{0:D8}", nextSequence);
            }

            if (Code == null)
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("COMOFFICE-{0:D8}", nextSequence);
            }
        }

        
        public bool OwnProperty
        {
            get => ownProperty;
            set => SetPropertyValue(nameof(OwnProperty), ref ownProperty, value);
        }

        
        public DateTime BuyOrRentalDate
        {
            get => buyOrRentalDate;
            set => SetPropertyValue(nameof(BuyOrRentalDate), ref buyOrRentalDate, value);
        }

        
        public MediaDataObject RelatedDocument
        {
            get => relatedDocument;
            set => SetPropertyValue(nameof(RelatedDocument), ref relatedDocument, value);
        }

        MediaDataObject relatedDocument;
        DateTime buyOrRentalDate;
        bool ownProperty;
        string code;
        string name;
        string facilityResponsible;
        string phone;
        string address;
        string zip;
        string city;
        string country;
        Company company;

        [RuleRequiredField("RuleRequiredField for CompanyOffice.Company", DefaultContexts.Save, "A company must be specified")]
        [Association("Company-Offices")]
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

        [RuleRequiredField("RuleRequiredField for CompanyOffice.Name", DefaultContexts.Save, "A name must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyOffice.Country", DefaultContexts.Save, "A country must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Country
        {
            get => country;
            set => SetPropertyValue(nameof(Country), ref country, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyOffice.City", DefaultContexts.Save, "A city must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Pos
        {
            get => city;
            set => SetPropertyValue(nameof(Pos), ref city, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Zip
        {
            get => zip;
            set => SetPropertyValue(nameof(Zip), ref zip, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyOffice.Address", DefaultContexts.Save, "An address must be specified")]
        [Size(SizeAttribute.Unlimited)]
        public string Address
        {
            get => address;
            set => SetPropertyValue(nameof(Address), ref address, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Phone
        {
            get => phone;
            set => SetPropertyValue(nameof(Phone), ref phone, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string FacilityResponsible
        {
            get => facilityResponsible;
            set => SetPropertyValue(nameof(FacilityResponsible), ref facilityResponsible, value);
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
    }
}