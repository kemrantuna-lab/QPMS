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
    /***
     * PERSONEL DIŞ GÖREVLENDİRME FORMU
     * 
     * 
     * ***/


    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EmployeeFormELD : EmployeeForm
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public EmployeeFormELD(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

            FormName = "External Location Duty Form";
        }


        string locationAddress;
        string locationName;

        [RuleRequiredField("RuleRequiredField for EmployeeFormELD.LocationName", DefaultContexts.Save, "A mobile phone must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LocationName
        {
            get => locationName;
            set => SetPropertyValue(nameof(LocationName), ref locationName, value);
        }

        [RuleRequiredField("RuleRequiredField for EmployeeFormELD.LocaitonAddress", DefaultContexts.Save, "A mobile phone must be specified")]
        [Size(SizeAttribute.Unlimited)]
        public string LocationAddress
        {
            get => locationAddress;
            set => SetPropertyValue(nameof(LocationAddress), ref locationAddress, value);
        }

    }
}