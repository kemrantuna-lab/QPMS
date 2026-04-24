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
    public class EmployeeQuit : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public EmployeeQuit(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

            CreatedBy = GetCurrentUser();
            CreatedOn = DateTime.Now;
            HasUpdate = true;
            Version = 0;

            Date = DateTime.Today;
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


        /***
         *      Letter of resignation       : istifa dilekçesi
         *      Decleration of Resignation  : SGK işten çıkış bildirgesi
         *      Acquittance                 : İbraname
         *      QuitPayroll                 : Çıkış bordrosu
         *      SeverancePayPayroll         : Kıdem Tazminatı Bordrosu
         *      Termination Benefit Payroll : İhbar Tazminatı Bordrosu
         *      Quit Pay Slip               : Çıkış ücret pusulası
         *      Dismissal Report            : İşten çıkış tutanağı veya işten çıkışı bildiren bir yazı
         *      IsRetired                   : Çalışan emeklimidir?
         * ***/


        DateTime date;
        FileData dismissalReport;
        FileData quitPaySlip;
        FileData terminationBenefitPayroll;
        FileData severancePayPayroll;
        FileData quitPayroll;
        FileData declarationOfResignation;
        FileData letterOfResignation;
        string reasonOfQuit;
        FileData acquittance;

        [RuleRequiredField("RuleRequiredField for EmployeeQuit.Date", DefaultContexts.Save, "A date must be specified")]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        //[RuleRequiredField("RuleRequiredField for EmployeeQuit.ReasonOfQuit", DefaultContexts.Save, "A quit reason must be specified")]
        [Size(SizeAttribute.Unlimited)]
        public string ReasonOfQuit
        {
            get => reasonOfQuit;
            set => SetPropertyValue(nameof(ReasonOfQuit), ref reasonOfQuit, value);
        }

        //[RuleRequiredField("RuleRequiredField for EmployeeQuit.LetterOfResignation", DefaultContexts.Save, "A letter of resignation must be specified")]
        public FileData LetterOfResignation
        {
            get => letterOfResignation;
            set => SetPropertyValue(nameof(LetterOfResignation), ref letterOfResignation, value);
        }

        //[RuleRequiredField("RuleRequiredField for EmployeeQuit.DeclarationOfResignation", DefaultContexts.Save, "A declaration of resignation document must be specified")]
        public FileData DeclarationOfResignation
        {
            get => declarationOfResignation;
            set => SetPropertyValue(nameof(DeclarationOfResignation), ref declarationOfResignation, value);
        }

        //[RuleRequiredField("RuleRequiredField for EmployeeQuit.Acquittance", DefaultContexts.Save, "An acquittance document must be specified")]
        public FileData Acquittance
        {
            get => acquittance;
            set => SetPropertyValue(nameof(Acquittance), ref acquittance, value);
        }


        public FileData QuitPaySlip
        {
            get => quitPaySlip;
            set => SetPropertyValue(nameof(QuitPaySlip), ref quitPaySlip, value);
        }

        public FileData QuitPayroll
        {
            get => quitPayroll;
            set => SetPropertyValue(nameof(QuitPayroll), ref quitPayroll, value);
        }


        public FileData SeverancePayPayroll
        {
            get => severancePayPayroll;
            set => SetPropertyValue(nameof(SeverancePayPayroll), ref severancePayPayroll, value);
        }


        public FileData TerminationBenefitPayroll
        {
            get => terminationBenefitPayroll;
            set => SetPropertyValue(nameof(TerminationBenefitPayroll), ref terminationBenefitPayroll, value);
        }


        public FileData DismissalReport
        {
            get => dismissalReport;
            set => SetPropertyValue(nameof(DismissalReport), ref dismissalReport, value);
        }


        Employee employee = null;
        public Employee Employee
        {
            get { return employee; }
            set
            {
                if (employee == value)
                    return;

                // Store a reference to the former owner.
                Employee previousEmployee = employee;
                employee = value;

                if (IsLoading) return;

                // Remove an owner's reference to this building, if exists.
                if (previousEmployee != null && previousEmployee.EmployeeQuit == this)
                    previousEmployee.EmployeeQuit = null;

                // Specify that the building is a new owner's house.
                if (employee != null)
                    employee.EmployeeQuit = this;
                OnChanged(nameof(Employee));
            }
        }


        #region ORERPRecordDetailsRegion


        [ModelDefault("AllowEdit", "False")]
        [Association("Company-EmployeeQuits")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        Company company;
        bool hasUpdate;
        int version;
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
            get { return updatedBy; }
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

        MediaDataObject photo;

        [ImageEditor(ListViewImageEditorCustomHeight = 250, DetailViewImageEditorFixedWidth = 250)]
        public MediaDataObject Photo
        {
            get => photo;
            set => SetPropertyValue(nameof(Photo), ref photo, value);
        }

        #endregion
    }
}
