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
    public class CompanyRental : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public CompanyRental(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        FileData rentalDocument;
        DateTime end;
        DateTime start;
        string currency;
        double price;
        string rentingBody;
        string description;
        string name;
        Company company;

        [Association("Company-CompanyRentals")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyRental.Name", DefaultContexts.Save, "A name must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyRental.Description", DefaultContexts.Save, "A description must be specified")]
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyRental.RentingBody", DefaultContexts.Save, "A renting body must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string RentingBody
        {
            get => rentingBody;
            set => SetPropertyValue(nameof(RentingBody), ref rentingBody, value);
        }


        public double Price
        {
            get => price;
            set => SetPropertyValue(nameof(Price), ref price, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyRental.currency", DefaultContexts.Save, "A currency must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Currency
        {
            get => currency;
            set => SetPropertyValue(nameof(Currency), ref currency, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyRental.Start", DefaultContexts.Save, "A start date must be specified")]
        public DateTime Start
        {
            get => start;
            set => SetPropertyValue(nameof(Start), ref start, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyRental.End", DefaultContexts.Save, "An end date must be specified")]
        public DateTime End
        {
            get => end;
            set => SetPropertyValue(nameof(End), ref end, value);
        }

        [RuleRequiredField("RuleRequiredField for CompanyRental.RentalDocument", DefaultContexts.Save, "A document must be uploaded.")]
        public FileData RentalDocument
        {
            get => rentalDocument;
            set => SetPropertyValue(nameof(RentalDocument), ref rentalDocument, value);
        }
    }
}