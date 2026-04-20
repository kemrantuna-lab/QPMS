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
    /***
     * 
     * 
     * 
     * *Allowance Request Form
     * ***/
    public class EmployeeFormARF : EmployeeForm
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public EmployeeFormARF(Session session)
            : base(session)
        {
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            FormName = "Allowance Request Form";
        }


        string currency;
        double amount;



        //[RuleRequiredField("RuleRequiredField for EmployeeFormARF.Amount", DefaultContexts.Save, "Amount must be specified")]
        public double Amount
        {
            get => amount;
            set => SetPropertyValue(nameof(Amount), ref amount, value);
        }

        [RuleRequiredField("RuleRequiredField for EmployeeFormARF.Currency", DefaultContexts.Save, "A currency must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Currency
        {
            get => currency;
            set => SetPropertyValue(nameof(Currency), ref currency, value);
        }

        #region ORERPRecordDetailsRegion
        //double approval;


        #endregion
    }
}