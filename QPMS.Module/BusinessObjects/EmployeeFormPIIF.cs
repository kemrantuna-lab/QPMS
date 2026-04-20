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
    /**
     * PERSONEL İZİN İSTEK FORMU
     * 
     * **/
    public class EmployeeFormPIIF : EmployeeForm
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public EmployeeFormPIIF(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            FormName = "Personel İzin İstek Formu";
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



        double totalDaysOfHoliday2;
        CompanyAbsence absenceType;
        string description;
        int totalDaysOfHoliday;
        DateTime end;
        DateTime start;
        //Employee employee;

        [RuleRequiredField("RuleRequiredField for EmployeeFormPIIF.Absence", DefaultContexts.Save, "An absence type must be specified")]
        [Association("CompanyAbsence-PIIFS")]
        public CompanyAbsence AbsenceType
        {
            get => absenceType;
            set => SetPropertyValue(nameof(AbsenceType), ref absenceType, value);
        }

        [RuleRequiredField("RuleRequiredField for EmployeeFormPIIF.Start", DefaultContexts.Save, "A start date and time must be specified")]
        public DateTime Start
        {
            get => start;
            set => SetPropertyValue(nameof(Start), ref start, value);
        }

        [RuleRequiredField("RuleRequiredField for EmployeeFormPIIF.End", DefaultContexts.Save, "An end date time must be specified")]
        public DateTime End
        {
            get => end;
            set => SetPropertyValue(nameof(End), ref end, value);
        }


        public int TotalDaysOfHoliday
        {
            get => totalDaysOfHoliday;
            set => SetPropertyValue(nameof(TotalDaysOfHoliday), ref totalDaysOfHoliday, value);
        }

        
        public double TotalDaysOfHoliday2
        {
            get => totalDaysOfHoliday2;
            set => SetPropertyValue(nameof(TotalDaysOfHoliday2), ref totalDaysOfHoliday2, value);
        }

        [RuleRequiredField("RuleRequiredField for EmployeeFormPIIF.Description", DefaultContexts.Save, "A description must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }
    }
}