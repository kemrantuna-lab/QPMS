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
using System.Diagnostics;
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
    public class Project : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Project(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            CreatedBy = GetCurrentUser();
            CreatedOn = DateTime.Now;

            Start = DateTime.Today;

            int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, daysInCurrentMonth);

            //int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
            //Code = string.Format("PR-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + "{0:D8}", nextSequence);
            Version = 0;
            HasUpdate = true;

            try
            {
                Employee currentUser = Session.FindObject<Employee>(
                        new BinaryOperator("Oid",SecuritySystem.CurrentUserId)
                    );    

                if(currentUser!=null && currentUser.Company != null)
                {
                    this.Company = currentUser.Company;
                }
            } catch
            {

            }

        }

        protected override void OnSaving()
        {
            UpdatedOn = DateTime.Now;
            UpdatedBy = GetCurrentUser();
            Version = Version + 1;
            HasUpdate = true;

            if (Branch != null)
            {
                if (Branch.Company != null)
                {
                    this.Company = this.Branch.Company;
                }
            }

            //if (Session != null)
            //{
            //    Debug.WriteLine("Session is not null");
            //} else
            //{
            //    Debug.WriteLine("Session is null, find another way to access datalayer");
            //}


            //if(Code == null)
            //{
                
            //    int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer,
            //                                                             this.GetType().FullName,
            //                                                             "P");

            //    Code = GetYearCode(DateTime.Now) + GetMonthCode(DateTime.Now) + GetDayCode(DateTime.Now) + nextSequence.ToString();
            //}


            //https://supportcenter.devexpress.com/ticket/details/t755305/adapting-the-distributedidgeneratorhelper
            //if (!(Session is NestedUnitOfWork) &&
            //    (Session.DataLayer != null) &&
            //    Session.IsNewObject(this) &&
            //    (Session.ObjectLayer is SimpleObjectLayer)
            //            //OR  
            //            //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)  
            //            &&
            //    string.IsNullOrEmpty(Code))
            //{
            //    int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer,
            //                                                             this.GetType().FullName,
            //                                                             "P");

            //    Code = GetYearCode(DateTime.Now) + GetMonthCode(DateTime.Now) + GetDayCode(DateTime.Now) +nextSequence.ToString();
            //}
            base.OnSaving();
        }

        
        public State State
        {
            get => state;
            set => SetPropertyValue(nameof(State), ref state, value);
        }

        
        public InvoiceState InvoiceState
        {
            get => invoiceState;
            set => SetPropertyValue(nameof(InvoiceState), ref invoiceState, value);
        }

        //[RuleRequiredField("RuleRequiredField for Project.CSL", DefaultContexts.Save, "A CSL level must be specified")]
        public CSL CSL
        {
            get => cSL;
            set => SetPropertyValue(nameof(CSL), ref cSL, value);
        }

        //[RuleRequiredField("RuleRequiredField for Project.Language", DefaultContexts.Save, "A Language must be specified")]
        public Lang Language
        {
            get => language;
            set => SetPropertyValue(nameof(Language), ref language, value);
        }

        //[RuleRequiredField("RuleRequiredField for Project.Currency", DefaultContexts.Save, "A currency must be specified")]
        [Association("Cur-Projects")]
        public Cur Currency
        {
            get => currency;
            set => SetPropertyValue(nameof(Currency), ref currency, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Association("Company-Projects")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }


        [Association("Customer-Projects")]
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }

        [RuleRequiredField("RuleRequiredField for Project.Branch", DefaultContexts.Save, "A branch must be specified")]
        [DataSourceProperty("Company.Branches")]
        [Association("Branch-Projects")]
        public Branch Branch
        {
            get => branch;
            set => SetPropertyValue(nameof(Branch), ref branch, value);
        }

        //[RuleRequiredField("RuleRequiredField for Project.Report", DefaultContexts.Save, "A report must be specified")]
        [DataSourceProperty("Company.Reports")]
        [Association("QPMSReport-Projects")]
        public QPMSReport Report
        {
            get => report;
            set => SetPropertyValue(nameof(Report), ref report, value);
        }



        Cur currency;
        InvoiceState invoiceState;
        QPMSReport report;
        CSL cSL;
        Lang language;
        FileData workInstruction;
        double eH;
        double nH;
        double tHours;
        FileData workOrder;
        State state;
        Company company;
        Customer customer;
        Branch branch;
        Project main;
        DateTime end;
        DateTime start;
        string code;
        string name;

        [RuleRequiredField("RuleRequiredField for Project.Name", DefaultContexts.Save, "A name must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        [RuleRequiredField("RuleRequiredField for Project.Start", DefaultContexts.Save, "A start date must be specified")]
        public DateTime Start
        {
            get => start;
            set => SetPropertyValue(nameof(Start), ref start, value);
        }

        [RuleRequiredField("RuleRequiredField for Project.End", DefaultContexts.Save, "An end date must be specified")]
        public DateTime End
        {
            get => end;
            set => SetPropertyValue(nameof(End), ref end, value);
        }


        [Association("Project-Members")]
        public XPCollection<ProjectMember> Members
        {
            get
            {
                return GetCollection<ProjectMember>(nameof(Members));
            }
        }

        #region ProjectPredefinitions

        [DataSourceCriteria("Oid!='@This.Oid'")]
        [Association("Project-Projects")]
        public Project Main
        {
            get => main;
            set => SetPropertyValue(nameof(Main), ref main, value);
        }

        [DataSourceCriteria("Oid!='@This.Oid'")]
        [Association("Project-Projects")]
        public XPCollection<Project> SubProjects
        {
            get
            {
                return GetCollection<Project>(nameof(SubProjects));
            }
        }

        [DevExpress.ExpressApp.DC.Aggregated, Association("Project-Parts")]
        public XPCollection<Part> Parts
        {
            get
            {
                return GetCollection<Part>(nameof(Parts));
            }
        }

        [DevExpress.ExpressApp.DC.Aggregated, Association("Project-Failures")]
        public XPCollection<Failure> Failures
        {
            get
            {
                return GetCollection<Failure>(nameof(Failures));
            }
        }

        [DevExpress.ExpressApp.DC.Aggregated, Association("Project-FailureModes")]
        public XPCollection<FailureMode> FailureModes
        {
            get
            {
                return GetCollection<FailureMode>(nameof(FailureModes));
            }
        }

        [DevExpress.ExpressApp.DC.Aggregated, Association("Project-TimesheetTypes")]
        public XPCollection<ProjectTimesheetType> TimesheetTypes
        {
            get
            {
                return GetCollection<ProjectTimesheetType>(nameof(TimesheetTypes));
            }
        }

        [Association("Employee-Projects")]
        public XPCollection<Employee> Users
        {
            get
            {
                return GetCollection<Employee>(nameof(Users));
            }
        }

        [Association("Project-Locations")]
        public XPCollection<ProjectLocation> Locations
        {
            get
            {
                return GetCollection<ProjectLocation>(nameof(Locations));
            }
        }

        #endregion

        [DevExpress.ExpressApp.DC.Aggregated, Association("Project-PTFs")]
        public XPCollection<PTF> PTFs
        {
            get
            {
                return GetCollection<PTF>(nameof(PTFs));
            }
        }


        [DevExpress.ExpressApp.DC.Aggregated, Association("Project-Timesheets")]
        public XPCollection<ProjectTimesheet> Timesheets
        {
            get
            {
                return GetCollection<ProjectTimesheet>(nameof(Timesheets));
            }
        }

        [DevExpress.ExpressApp.DC.Aggregated, Association("Project-Traceabilities")]
        public XPCollection<Traceability> Traceabilities
        {
            get
            {
                return GetCollection<Traceability>(nameof(Traceabilities));
            }
        }

        [Association("Project-Documentation")]
        public XPCollection<Documentation> Documentation
        {
            get
            {
                return GetCollection<Documentation>(nameof(Documentation));
            }
        }

        [Association("Project-Invoices")]
        public XPCollection<Invoice> Invoices
        {
            get
            {
                return GetCollection<Invoice>(nameof(Invoices));
            }
        }

        [Association("Project-PDA")]
        public XPCollection<ProjectAnalysisDaily> PDA
        {
            get
            {
                return GetCollection<ProjectAnalysisDaily>(nameof(PDA));
            }
        }

        [DevExpress.ExpressApp.DC.Aggregated]
        [Association("Project-Docs")]
        public XPCollection<ProjectDocument> Docs
        {
            get
            {
                return GetCollection<ProjectDocument>(nameof(Docs));
            }
        }

        [Association("Project-Files")]
        public XPCollection<ProjectFile> Files
        {
            get
            {
                return GetCollection<ProjectFile>(nameof(Files));
            }
        }

        public FileData WorkOrder
        {
            get => workOrder;
            set => SetPropertyValue(nameof(WorkOrder), ref workOrder, value);
        }

        
        public FileData WorkInstruction
        {
            get => workInstruction;
            set => SetPropertyValue(nameof(WorkInstruction), ref workInstruction, value);
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
        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
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

        #endregion


        [DevExpress.ExpressApp.DC.Aggregated, Association("Project-Notes")]
        public XPCollection<ProjectNote> Notes
        {
            get
            {
                return GetCollection<ProjectNote>(nameof(Notes));
            }
        }

        [Association("Project-Expenses")]
        public XPCollection<ProjectExpense> Expenses
        {
            get
            {
                return GetCollection<ProjectExpense>(nameof(Expenses));
            }
        }

        [Association("Project-Risks")]
        public XPCollection<ProjectRisk> Risks
        {
            get
            {
                return GetCollection<ProjectRisk>(nameof(Risks));
            }
        }

        [Association("Project-Excels")]
        public XPCollection<ProjectExcel> Excels
        {
            get
            {
                return GetCollection<ProjectExcel>(nameof(Excels));
            }
        }

        [Association("Project-Events")]
        public XPCollection<ProjectEvent> Events
        {
            get
            {
                return GetCollection<ProjectEvent>(nameof(Events));
            }
        }

        [Association("CustomerUser-AssignedProjects")]
        public XPCollection<CustomerUser> CustomerUsers
        {
            get
            {
                return GetCollection<CustomerUser>(nameof(CustomerUsers));
            }
        }

        #region projectcoderegion
        public string GetSequenceCode()
        {
            DevExpress.Xpo.IDataLayer dataLayer = Session.DataLayer; //((DevExpress.ExpressApp.Xpo.XPObjectSpace)View.ObjectSpace).Session.DataLayer;

            int nextSequence = DistributedIdGeneratorHelper.Generate(dataLayer, this.GetType().FullName, "MyServerPrefix");

            string codString = String.Format("{0:000}", nextSequence);

            string cod = nextSequence.ToString(); //nextSequence.ToString();

            return cod;
        }

        public string GetYearCode(DateTime Date)
        {
            string yearCode = string.Empty;

            int year = Date.Year;

            switch (year)
            {
                case 2018:
                    yearCode = "Y";
                    break;
                case 2019:
                    yearCode = "W";
                    break;
                case 2020:
                    yearCode = "X";
                    break;
                case 2021:
                    yearCode = "XA";
                    break;
                case 2022:
                    yearCode = "XB";
                    break;
                case 2023:
                    yearCode = "XC";
                    break;
                case 2024:
                    yearCode = "XD";
                    break;
                case 2025:
                    yearCode = "XE";
                    break;
                case 2026:
                    yearCode = "XF";
                    break;
                case 2027:
                    yearCode = "XG";
                    break;
                case 2028:
                    yearCode = "XH";
                    break;
                case 2029:
                    yearCode = "XJ";
                    break;
                case 2030:
                    yearCode = "XK";
                    break;
                default:
                    yearCode = "%%";
                    break;
            }

            return yearCode;
        }

        public string GetMonthCode(DateTime Date)
        {
            string monthCode = string.Empty;

            int month = Date.Month;

            switch (month)
            {
                case 1:
                    monthCode = "A";
                    break;
                case 2:
                    monthCode = "B";
                    break;
                case 3:
                    monthCode = "C";
                    break;
                case 4:
                    monthCode = "D";
                    break;
                case 5:
                    monthCode = "R";
                    break;
                case 6:
                    monthCode = "F";
                    break;
                case 7:
                    monthCode = "G";
                    break;
                case 8:
                    monthCode = "H";
                    break;
                case 9:
                    monthCode = "J";
                    break;
                case 10:
                    monthCode = "K";
                    break;
                case 11:
                    monthCode = "M";
                    break;
                case 12:
                    monthCode = "N";
                    break;
                default:
                    monthCode = "+";
                    break;
            }

            return monthCode;
        }

        public string GetDayCode(DateTime Date)
        {
            string dayCode = string.Empty;

            int day = Date.Day;

            switch (day)
            {
                case 1:
                    dayCode = "A";
                    break;
                case 2:
                    dayCode = "B";
                    break;
                case 3:
                    dayCode = "C";
                    break;
                case 4:
                    dayCode = "D";
                    break;
                case 5:
                    dayCode = "E";
                    break;
                case 6:
                    dayCode = "F";
                    break;
                case 7:
                    dayCode = "G";
                    break;
                case 8:
                    dayCode = "H";
                    break;
                case 9:
                    dayCode = "J";
                    break;
                case 10:
                    dayCode = "K";
                    break;
                case 11:
                    dayCode = "L";
                    break;
                case 12:
                    dayCode = "M";
                    break;
                case 13:
                    dayCode = "N";
                    break;
                case 14:
                    dayCode = "O";
                    break;
                case 15:
                    dayCode = "P";
                    break;
                case 16:
                    dayCode = "R";
                    break;
                case 17:
                    dayCode = "S";
                    break;
                case 18:
                    dayCode = "T";
                    break;
                case 19:
                    dayCode = "U";
                    break;
                case 20:
                    dayCode = "V";
                    break;
                case 21:
                    dayCode = "Y";
                    break;
                case 22:
                    dayCode = "Z";
                    break;
                case 23:
                    dayCode = "X";
                    break;
                case 24:
                    dayCode = "4";
                    break;
                case 25:
                    dayCode = "5";
                    break;
                case 26:
                    dayCode = "6";
                    break;
                case 27:
                    dayCode = "7";
                    break;
                case 28:
                    dayCode = "8";
                    break;
                case 29:
                    dayCode = "9";
                    break;
                case 30:
                    dayCode = "0";
                    break;
                case 31:
                    dayCode = "1";
                    break;
                default:
                    dayCode = "*";
                    break;
            }

            return dayCode;
        }
        #endregion

        #region TOTALSRegion


        private int? fOK = null;
        private int? fNOK = null;
        private int? fROK = null;
        private int? fRNOK = null;
        private int? fTOTAL = null;

        private int? fSerial = null;
        private int? fBox = null;
        private int? fPalette = null;
        private int? fDelivery = null;
        private int? fOrder = null;

        [Persistent("OK")]
        public int? OK
        {
            get
            {
                if (!IsLoading && !IsSaving && fOK == null)
                    UpdateQuantities(false);// UpdateTotalNOK(false);
                return fOK;
            }
        }

        [Persistent("NOK")]
        public int? NOK
        {
            get
            {
                if (!IsLoading && !IsSaving && fNOK == null)
                    UpdateQuantities(false);// UpdateTotalNOK(false);
                return fNOK;
            }
        }

        [Persistent("ROK")]
        public int? ROK
        {
            get
            {
                if (!IsLoading && !IsSaving && fROK == null)
                    UpdateQuantities(false);// UpdateTotalNOK(false);
                return fROK;
            }
        }

        [Persistent("RNOK")]
        public int? RNOK
        {
            get
            {
                if (!IsLoading && !IsSaving && fRNOK == null)
                    UpdateQuantities(false);// UpdateTotalNOK(false);
                return fRNOK;
            }
        }

        [Persistent("TOTAL")]
        public int? TOTAL
        {
            get
            {
                if (!IsLoading && !IsSaving && fTOTAL == null)
                    UpdateQuantities(false);// UpdateTotalNOK(false);
                return fTOTAL;
            }
        }

        private void UpdateQuantities(bool forceChangeEvents)
        {
            int? oldOK = fOK;
            int? oldNOK = fNOK;
            int? oldROK = fNOK;
            int? oldRNOK = fRNOK;
            int? oldTOTAL = fTOTAL;
            int? oldSerial = fSerial;
            int? oldBox = fBox;
            int? oldPalette = fPalette;
            int? oldDelivery = fDelivery;
            int? oldOrder = fOrder;


            int tempOK = 0;
            int tempNOK = 0;
            int tempROK = 0;
            int tempRNOK = 0;
            int tempSerial = 0;
            int tempBox = 0;
            int tempPalette = 0;
            int tempDelivery = 0;
            int tempOrder = 0;

            var traces = this.Traceabilities.ToList();

            if (traces.Count > 0)
            {
                foreach (Traceability trace in traces)
                {
                    tempOK = tempOK + trace.OK;
                    tempNOK = tempNOK + trace.NOK;
                    tempROK = tempROK + trace.ROK;
                    tempRNOK = tempRNOK + trace.RNOK;

                    if (trace.SERIAL != null)
                    {
                        tempSerial = tempSerial + 1;
                    }

                    if (trace.BOX != null)
                    {
                        tempBox = tempBox + 1;
                    }

                    if (trace.PALETTE != null)
                    {
                        tempPalette = tempPalette + 1;
                    }

                    if (trace.DELIVERY != null)
                    {
                        tempDelivery = tempDelivery + 1;
                    }

                    if (trace.ORDER != null)
                    {
                        tempOrder = tempOrder + 1;
                    }
                }
            }
            else
            {
                tempOK = 0;
                tempNOK = 0;
                tempROK = 0;
                tempRNOK = 0;
                tempSerial = 0;
                tempBox = 0;
                tempPalette = 0;
                tempDelivery = 0;
                tempOrder = 0;

            }
            fOK = tempOK;
            fNOK = tempNOK;
            fROK = tempROK;
            fRNOK = tempRNOK;
            fTOTAL = fOK + fNOK + fROK + fRNOK;
            fSerial = tempSerial;
            fBox = tempBox;
            fPalette = tempPalette;
            fDelivery = tempDelivery;
            fOrder = tempOrder;


            if (forceChangeEvents)
            {
                OnChanged("OK", oldOK, fOK);
                OnChanged("NOK", oldNOK, fNOK);
                OnChanged("ROK", oldROK, fROK);
                OnChanged("RNOK", oldRNOK, fRNOK);
                OnChanged("TOTAL", oldTOTAL, fTOTAL);
                OnChanged("Serial", oldSerial, fSerial);
                OnChanged("Box", oldBox, fBox);
                OnChanged("Palette", oldPalette, fPalette);
                OnChanged("Delivery", oldDelivery, fDelivery);
                OnChanged("Order", oldOrder, fOrder);
            }
        }

        private int? fTPTFs = null;

        public int? TPTF
        {
            get
            {
                if (!IsLoading && !IsSaving && fTPTFs == null)
                    UpdateTPTFs(false);// UpdateTotalNOK(false);
                return fTPTFs;

            }
        }

        private void UpdateTPTFs(bool forceChangeEvents)
        {
            int? oldTPTFs = fTPTFs;
            int tempTPTFs = 0;

            try
            {
                tempTPTFs = this.PTFs.Count;
            }
            catch
            {
                tempTPTFs = 0;
            }

            fTPTFs = tempTPTFs;

            if (forceChangeEvents)
            {
                OnChanged("TPTF", oldTPTFs, fTPTFs);
            }
        }

        private int? fTParts = null;

        public int? TParts
        {
            get
            {
                if (!IsLoading && !IsSaving && fTParts == null)
                    UpdateTParts(false);
                return fTParts;
            }
        }

        private void UpdateTParts(bool forceChangeEvents)
        {
            int? oldTParts = fTParts;

            int tempTParts = 0;

            try
            {
                tempTParts = this.Parts.Count;
            }
            catch
            {
                tempTParts = 0;
            }

            fTParts = tempTParts;

            if (forceChangeEvents)
            {
                OnChanged("TParts", oldTParts, fTParts);
            }
        }

        private int? fTFailureModes = null;

        public int? TFailureModes
        {
            get
            {
                if (!IsLoading && !IsSaving && fTFailureModes == null)
                    UpdateTFailureModes(false);
                return fTFailureModes;
            }
        }

        private void UpdateTFailureModes(bool forceChangeEvents)
        {
            int? oldTFailureModes = fTFailureModes;
            int tempTFailureModes = 0;

            if (this.FailureModes.Count > 0)
            {
                tempTFailureModes = this.FailureModes.Count;
            }
            else
            {
                tempTFailureModes = 0;
            }

            fTFailureModes = tempTFailureModes;

            if (forceChangeEvents)
            {
                OnChanged("TFailureModes", oldTFailureModes, fTFailureModes);
            }
        }

        private int? fTFailures = null;

        public int? TFailures
        {
            get
            {
                if (!IsLoading && !IsSaving && fTFailures == null)
                    UpdateTFailures(false);
                return fTFailures;
            }
        }

        private void UpdateTFailures(bool forceChangeEvents)
        {
            int? oldTFailures = fTFailures;
            int temptTFailures = 0;

            if (this.Failures.Count > 0)
            {
                temptTFailures = this.Failures.Count;
            }
            else
            {
                temptTFailures = 0;
            }

            fTFailures = temptTFailures;

            if (forceChangeEvents)
            {
                OnChanged("TFailures", oldTFailures, fTFailures);
            }
        }

        private int? fTTimesheets = null;

        public int? TTimesheets
        {
            get
            {
                if (!IsSaving && !IsLoading && fTTimesheets == null)
                    UpdateTTimesheets(false);
                return fTTimesheets;
            }
        }

        private void UpdateTTimesheets(bool forceChangeEvents)
        {
            int? oldTTimesheets = fTTimesheets;
            int tempTTimesheets = 0;

            if (this.Timesheets.Count > 0)
            {
                tempTTimesheets = this.Timesheets.Count;
            }
            else
            {
                tempTTimesheets = 0;
            }

            fTTimesheets = tempTTimesheets;

            if (forceChangeEvents)
            {
                OnChanged("TTimesheets", oldTTimesheets, fTTimesheets);
            }
        }

        private int? fTTraces = null;

        public int? TTraces
        {
            get
            {
                if (!IsSaving && !IsLoading && fTTraces == null)
                    UpdateTTraces(false);
                return fTTraces;
            }
        }

        private void UpdateTTraces(bool v)
        {
            int? oldTTraces = fTTraces;
            int tempTTraces = 0;

            if (this.Traceabilities.Count > 0)
            {
                tempTTraces = this.Traceabilities.Count;
            }
            else
            {
                tempTTraces = 0;
            }

            fTTraces = tempTTraces;

            if (v)
            {
                OnChanged("TTraces", oldTTraces, fTTraces);
            }

        }

        private int? fTInvoices = null;

        public int? TInvoices
        {
            get
            {
                if (!IsLoading && !IsSaving && fTInvoices == null)
                    UpdateTInvoices(false);
                return fTInvoices;
            }
        }

        private void UpdateTInvoices(bool v)
        {
            int? oldTInvoices = fTInvoices;
            int tempTInvoices = 0;

            if (this.Invoices.Count > 0)
            {
                tempTInvoices = this.Invoices.Count;
            }
            else
            {
                tempTInvoices = 0;
            }

            fTInvoices = tempTInvoices;

            if (v)
            {
                OnChanged("TInvoices", oldTInvoices, fTInvoices);
            }
        }

        private double? fHours = null;

        [Persistent("Hours")]
        public double? Hours
        {
            get
            {
                if (!IsLoading && !IsSaving && fHours == null)
                    UpdateHours(false);
                return fHours;
            }
        }

        private void UpdateHours(bool forceChangeEvents)
        {
            double? oldHours = fHours;
            double tempHours = 0;


            var timesheetList = this.Timesheets.ToList();

            if (timesheetList.Count > 0)
            {
                foreach (var item in timesheetList)
                {
                    tempHours = tempHours + item.Hours;
                }
            }

            fHours = tempHours;

            if (forceChangeEvents)
            {
                OnChanged("Hours", oldHours, fHours);
            }
        }

        private double? fProjectCosts = null;

        public double? ProjectCostsTotal
        {
            get
            {
                if (!IsLoading && !IsSaving && fProjectCosts == null)
                    UpdateProjectCostsTotal(false);
                return fProjectCosts;
            }
        }

        private void UpdateProjectCostsTotal(bool forceChangeEvents)
        {
            double? oldTotal = fProjectCosts;
            double tempTotal = 0;

            try
            {
                var projectCostsList = this.ProjectCosts.ToList();
                if(projectCostsList.Count > 0)
                {
                    foreach(var item in projectCostsList) {
                        tempTotal = tempTotal + item.GrandTotal;
                    }
                }
            } catch (Exception ex)
            {
                tempTotal= 0;
            }

            fProjectCosts= tempTotal;

            if (forceChangeEvents)
            {
                OnChanged("ProjectCostsTotal", oldTotal, fProjectCosts);
            }
        }

        private double? fTotalMealsQuantity = null;

        public double? TotalMealsQuantity
        {
            get
            {
                if (!IsLoading && !IsSaving && fTotalMealsQuantity == null)
                    UpdateTotalMealsQuantity(false);
                return fTotalMealsQuantity;
            }
        }

        private void UpdateTotalMealsQuantity(bool forceChangeEvents)
        {
            double? oldTotalMealsQuantity = fTotalMealsQuantity; 
            double tempTotal = 0;
            double tempTotalMealPrice = 0;
            

            try
            {
                var list = this.Timesheets.ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        tempTotal = tempTotal + item.MealQuantity;
                    }
                }
            } catch (Exception ex)
            {
                tempTotal = 0;
            }

            fTotalMealsQuantity= tempTotal;

            if (forceChangeEvents)
            {
                OnChanged("TotalMealsQuantity", oldTotalMealsQuantity, fTotalMealsQuantity);
            }

        }

        private double? fTotalMealPrice = null; 
        public double? TotalMealPrice
        {
            get {

                if (!IsLoading && !IsSaving && fTotalMealPrice == null)
                    UpdateTotalMealPrice(false);
                return fTotalMealPrice; 
            
            }
        }

        private void UpdateTotalMealPrice(bool forceChangeEvents)
        {
            double? oldTotal = fTotalMealsQuantity;
            double tempTotal = 0;

            try
            {
                var list = this.Timesheets.ToList();

                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        tempTotal = tempTotal + item.MealPrice;
                    }
                }
            } catch
            (Exception ex)
            {
                tempTotal = 0;
            }

            fTotalMealPrice= tempTotal;
            if (forceChangeEvents)
            {
                OnChanged("TotalMealPrice",oldTotal,fTotalMealPrice);
            }
        }

        private double? fTotalKM = null;

        public double? TotalKM { 
            get {

                if (!IsLoading && !IsSaving && fTotalKM == null)
                    UpdateTotalKM(false);
                return fTotalKM; 
            } 
        }

        private void UpdateTotalKM(bool forceChangeEvent)
        {
            double? oldTotal = fTotalKM;
            double tempTotal = 0;

            try
            {
                var list = this.Timesheets.ToList();
                if (list.Count > 0) { foreach (var item in list)
                    {
                        tempTotal = tempTotal+item.KM;
                    } }
            } catch(Exception ex)
            {
                tempTotal = 0;
            }

            fTotalKM= tempTotal;

            if (forceChangeEvent)
            {
                OnChanged("TotalKM", oldTotal, fTotalKM);
            }
        }

        private double? fTotalKMPrice = null;

        public double? TotalKMPrice
        {
            get
            {
                if (!IsLoading && !IsSaving && fTotalKMPrice == null)
                    UpdateTotalKMPrice(false);
                return fTotalKMPrice;
            }
        }

        private void UpdateTotalKMPrice(bool forceChangeEvents)
        {
            double? oldTotal = fTotalKMPrice;
            double tempTotal = 0;

            try
            {
                var list = this.Timesheets.ToList() ;

                foreach(var item in list)
                {
                    tempTotal = tempTotal+item.DisplacementPrice;
                }
            } catch(Exception ex)
            {
                tempTotal = 0;
            }

            fTotalKMPrice= tempTotal;

            if(forceChangeEvents) { 
                OnChanged("TotalKMPrice",oldTotal, fTotalKMPrice);
            }
        }

        private double? fExpensesTotal = null;

        [Persistent("ExpensesTotal")]
        public double? ExpensesTotal
        {
            get
            {
                if (!IsLoading && !IsSaving && fExpensesTotal == null)
                    UpdateExpensesTotal(false);
                return fExpensesTotal;
            }
        }

        private void UpdateExpensesTotal(bool forceChangeEvents)
        {
            double? oldExpensesTotal = fExpensesTotal;
            double tempTotal = 0;


            try
            {
                var expenseList = this.Expenses.ToList();

                if (expenseList.Count > 0)
                {
                    foreach (var item in expenseList)
                    {
                        tempTotal = tempTotal + item.Total;
                    }
                }
            }
            catch
            {
                tempTotal= 0;
            }
            
            fExpensesTotal = tempTotal;

            if (forceChangeEvents)
            {
                OnChanged("ExpensesTotal", oldExpensesTotal, fExpensesTotal);
            }
        }

        [ModelDefault("AllowEdit", "False")]
        public double THours
        {
            get => tHours;
            set => SetPropertyValue(nameof(THours), ref tHours, value);
        }

        private double? fNH = null;

        [Persistent("NH")]
        public double? NH
        {
            get
            {
                if (!IsLoading && !IsSaving && fNH == null)
                    UpdateHoursTotal(false);
                return fNH;
            }
        }

        private double? fEH = null;

        [Persistent("EH")]
        public double? EH
        {
            get
            {
                if (!IsLoading && !IsSaving && fEH == null)
                    UpdateHoursTotal(false);
                return fEH;
            }
        }

        private double? fHH = null;

        [Persistent("HH")]
        public double? HH
        {
            get
            {
                if (!IsLoading && !IsSaving && fHH == null)
                    UpdateHoursTotal(false);
                return fHH;
            }
        }

        private void UpdateHoursTotal(bool forceChangeEvents)
        {
            double? oldNH = fNH;
            double? oldEH= fEH;
            double? oldHH = fHH;

            double tempNH = 0;
            double tempEH = 0;
            double tempHH = 0;

            try
            {
                var listOfPTFs = this.PTFs.ToList();

                foreach(var item in listOfPTFs)
                {
                    tempNH = tempNH + item.NH;
                    tempEH = tempEH + item.EH;
                    tempHH = tempHH + item.HH;
                }
            } catch {
                tempNH = 0;
                tempEH = 0;
                tempHH= 0;
                //throw new Exception(this.Code + " can not calculate timesheets for this project");
            }

            fNH= tempNH;
            fEH= tempEH;
            fHH= tempHH;

            if (forceChangeEvents)
            {
                OnChanged("NH", oldNH, fNH);
                OnChanged("EH", oldEH, fEH);
                OnChanged("HH", oldHH, fHH);
            }
        }

        private double? fGrandTotal = null;

        public double? GrandTotal
        {
            get {

                if (!IsLoading && !IsSaving && fGrandTotal == null)
                    UpdateGrandTotal(false);
                return fGrandTotal; 
            }
        }

        private void UpdateGrandTotal(bool forceChangeEvents)
        {
            double? oldGrandTotal = fGrandTotal;
            double tempGrandTotal = 0;

            try
            {
                var timesheetList = this.Timesheets.ToList();

                

                double tempTimesheetsCost = 0;
                double tempTimesheetsMealCost = 0;
                double tempTimesheetsKMCost = 0;

                if(timesheetList.Count > 0)
                {
                    foreach (var item in timesheetList)
                    {
                        tempTimesheetsCost = tempTimesheetsCost + (item.Hours * item.ProjectTimesheetType.Amount);
                        tempTimesheetsMealCost = tempTimesheetsMealCost + item.MealPrice;
                        tempTimesheetsKMCost = tempTimesheetsKMCost + item.DisplacementPrice;
                    }
                }

                var ptfCostList = this.ProjectCosts.ToList();

                double tempProjectCosts = 0;

                if(ptfCostList.Count > 0)
                {
                    foreach (var item in ptfCostList)
                    {
                        tempProjectCosts = tempProjectCosts + item.GrandTotal;
                    }
                }

                tempGrandTotal = tempTimesheetsCost + tempTimesheetsMealCost + tempTimesheetsKMCost + tempProjectCosts;

            } catch { 
                tempGrandTotal = 0;
            }

            fGrandTotal= tempGrandTotal;

            if(forceChangeEvents )
            {
                OnChanged("GrandTotal", oldGrandTotal, fGrandTotal);
            }
        }

        protected override void OnLoaded()
        {
            Reset();
            base.OnLoaded();
        }
        private void Reset()
        {
            fNH = null;
            fEH= null;
            fHH = null;
            fOK = null;
            fNOK = null;
            fRNOK = null;
            fROK = null;
            fTOTAL = null;
            fSerial = null;
            fBox = null;
            fPalette = null;
            fDelivery = null;
            fOrder = null;
            fTPTFs = null;
            fTParts = null;
            fTInvoices = null;
            fTTimesheets = null;
            fTTraces = null;
            fTFailureModes = null;
            fTFailures = null;
            fProjectCosts= null;
            fTotalKM= null;
            fTotalKMPrice= null;
            fTotalMealPrice= null;
            fTotalMealsQuantity= null;

            fGrandTotal= null;
            
            fHours = null;
            fExpensesTotal = null;
        }

        #endregion

        ProjectEndInfo pei = null;


        [DataSourceProperty("Company.ProjectEndInformations")]
        public ProjectEndInfo PEI
        {
            get { return pei; }
            set
            {
                if (pei == null)
                    return;
                pei = value;

                ProjectEndInfo previousEndInfo = pei;

                if (IsLoading) return;

                if (previousEndInfo != null && previousEndInfo.Project == this)
                    previousEndInfo.Project = null;

                if (pei != null)
                {
                    pei.Project = this;
                }

                OnChanged(nameof(PEI));
            }
        }

        [Association("Project-InvoiceProjectPeriods")]
        public XPCollection<InvoiceProjectPeriod> InvoiceProjectPeriods
        {
            get
            {
                return GetCollection<InvoiceProjectPeriod>(nameof(InvoiceProjectPeriods));
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

        [Association("Project-ProjectCosts")]
        public XPCollection<ProjectCost> ProjectCosts
        {
            get
            {
                return GetCollection<ProjectCost>(nameof(ProjectCosts));
            }
        }
    }

    public enum State
    {
        [ImageName("State_Validation_Valid")]
        ON,
        [ImageName("State_Validation_Invalid")]
        OFF,
        [ImageName("State_Validation_Skipped")]
        SUSPENDED
    }

    public enum InvoiceState
    {
        [ImageName("Business_Money")]
        TO_BE_INVOICED,
        [ImageName("BO_Invoice")]
        INVOICED
    }

    public enum ProjectType
    {
        SORTING,
        REWORK,
        SURWAY,
        SORTINGANDREWORK,
        MEASUREMENT,
        MANPOWER,
        CARPROCUREMENT,
        FORKLIFTPROCUREMENT,
        TOOLPROCUREMENT
    }

    public enum CSL
    {
        CSL2,
        CSL3,
        CSL1
    }

    public enum Lang
    {
        TR,
        EN,
        DE,
        FR
    }
}