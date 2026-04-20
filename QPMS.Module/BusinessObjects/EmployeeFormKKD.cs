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
    public class EmployeeFormKKD : EmployeeForm
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public EmployeeFormKKD(Session session)
            : base(session)
            
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            FormName = "Koruyucu Donanım Teslim Formu";
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

        bool eSDWear;
        bool eSDShoes;
        bool earProtector;
        bool safetyHarnessBelt;
        bool safetyShoes;
        bool safetyHelmet;
        bool workingMask;
        bool workingWear;
        //Employee employee;

        //[Association("Employee-KKDS")]
        //public Employee Employee
        //{
        //    get => employee;
        //    set => SetPropertyValue(nameof(Employee), ref employee, value);
        //}



        public bool WorkingWear
        {
            get => workingWear;
            set => SetPropertyValue(nameof(WorkingWear), ref workingWear, value);
        }


        public bool WorkingMask
        {
            get => workingMask;
            set => SetPropertyValue(nameof(WorkingMask), ref workingMask, value);
        }


        public bool SafetyHelmet
        {
            get => safetyHelmet;
            set => SetPropertyValue(nameof(SafetyHelmet), ref safetyHelmet, value);
        }


        public bool SafetyShoes
        {
            get => safetyShoes;
            set => SetPropertyValue(nameof(SafetyShoes), ref safetyShoes, value);
        }


        public bool SafetyHarnessBelt
        {
            get => safetyHarnessBelt;
            set => SetPropertyValue(nameof(SafetyHarnessBelt), ref safetyHarnessBelt, value);
        }


        public bool EarProtector
        {
            get => earProtector;
            set => SetPropertyValue(nameof(EarProtector), ref earProtector, value);
        }


        public bool ESDShoes
        {
            get => eSDShoes;
            set => SetPropertyValue(nameof(ESDShoes), ref eSDShoes, value);
        }

        
        public bool ESDWear
        {
            get => eSDWear;
            set => SetPropertyValue(nameof(ESDWear), ref eSDWear, value);
        }
    }
}