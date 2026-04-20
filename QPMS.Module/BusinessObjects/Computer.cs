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
    public class Computer : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Computer(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        FileData propertiesFile;
        int purchaseYear;
        int rAM;
        string notes;
        string oS;
        string model;
        string brand;
        string sERIAL;
        Company company;

        [RuleRequiredField("RuleRequiredField for Computer.Company", DefaultContexts.Save, "A company must be specified")]
        [Association("Company-Computers")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [RuleRequiredField("RuleRequiredField for Computer.Brand", DefaultContexts.Save, "A brand must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Brand
        {
            get => brand;
            set => SetPropertyValue(nameof(Brand), ref brand, value);
        }

        [RuleRequiredField("RuleRequiredField for Computer.Model", DefaultContexts.Save, "A model must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Model
        {
            get => model;
            set => SetPropertyValue(nameof(Model), ref model, value);
        }


        [RuleRequiredField("RuleRequiredField for Computer.SERIAL", DefaultContexts.Save, "A serial must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SERIAL
        {
            get => sERIAL;
            set => SetPropertyValue(nameof(SERIAL), ref sERIAL, value);
        }


        [RuleRequiredField("RuleRequiredField for Computer.OS", DefaultContexts.Save, "An OS must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string OS
        {
            get => oS;
            set => SetPropertyValue(nameof(OS), ref oS, value);
        }


        public int PurchaseYear
        {
            get => purchaseYear;
            set => SetPropertyValue(nameof(PurchaseYear), ref purchaseYear, value);
        }

        public int RAM
        {
            get => rAM;
            set => SetPropertyValue(nameof(RAM), ref rAM, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }

        
        public FileData PropertiesFile
        {
            get => propertiesFile;
            set => SetPropertyValue(nameof(PropertiesFile), ref propertiesFile, value);
        }
    }
}