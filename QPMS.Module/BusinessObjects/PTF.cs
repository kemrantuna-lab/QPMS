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
    public class PTF : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public PTF(Session session)
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

            Date = DateTime.Now.AddDays(-1);

            if (Project != null)
            {
                this.Branch = Project.Branch;
            }
        }

        protected override void OnSaving()
        {
            UpdatedBy = GetCurrentUser();
            UpdatedOn = DateTime.Now;
            HasUpdate = true;
            Version = Version + 1;

            if(Name == null)
            {
                Name = "PTF " + Date.ToShortDateString();
            }

            if (Project != null)
            {
                if(Project.Branch!=null)

                this.Branch = Project.Branch;
            }


            if (!(Session is NestedUnitOfWork)
               && (Session.DataLayer != null)
                   && Session.IsNewObject(this)
                   && (Session.ObjectLayer is SimpleObjectLayer)
           //OR
           //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
           && string.IsNullOrEmpty(Code))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("POTF-{0:D8}", nextSequence);
            }

            base.OnSaving();
        }

        protected override void OnDeleted()
        {
            var listOfTimesheets = this.Timesheets.ToList();
            var listOfTraceabilities = this.Traceabilities.ToList();
            var listOfExpenses = this.Expenses.ToList();
            var listOfProjectCosts = this.ProjectCosts.ToList();

            if (listOfTimesheets.Count > 0)
            {
                foreach(var item in listOfTimesheets)
                {
                    if (item.Project != null)
                    {
                        item.PTF = null;
                    }
                }
            }


            base.OnDeleted();
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        double totalDisplacementPrice;
        double totalKM;
        double totalMealPrice;
        double totalMeals;
        ProjectLocation location;
        string code;
        string description;
        MediaDataObject photo;

        [ImageEditor(ListViewImageEditorCustomHeight = 250)]
        public MediaDataObject Photo
        {
            get => photo;
            set => SetPropertyValue(nameof(Photo), ref photo, value);
        }

        

        [DataSourceCriteria("[Employees][[Oid] = CurrentUserId()]")]
        [Association("Branch-PTFs")]
        public Branch Branch
        {
            get => branch;
            set => SetPropertyValue(nameof(Branch), ref branch, value);
        }

        double hH;
        double eH;
        double nH;
        string name;
        Branch branch;
        DateTime date;
        Project project;

        [DataSourceProperty("Branch.Projects")]
        [RuleRequiredField("RuleRequiredField for PTF.Project", DefaultContexts.Save, "A project must be specified")]
        [Association("Project-PTFs")]
        public Project Project
        {
            get => project;
            set => SetPropertyValue(nameof(Project), ref project, value);
        }

        [DataSourceProperty("Project.Locations")]
        [Association("ProjectLocation-PTFs")]
        public ProjectLocation Location
        {
            get => location;
            set => SetPropertyValue(nameof(Location), ref location, value);
        }


        [RuleRequiredField("RuleRequiredField for PTF.Date", DefaultContexts.Save, "A date must be specified")]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Association("PTF-Traceabilities")]
        public XPCollection<Traceability> Traceabilities
        {
            get
            {
                return GetCollection<Traceability>(nameof(Traceabilities));
            }
        }

        [Association("PTF-Timesheets")]
        public XPCollection<ProjectTimesheet> Timesheets
        {
            get
            {
                return GetCollection<ProjectTimesheet>(nameof(Timesheets));
            }
        }

        [Association("PTF-Failures")]
        public XPCollection<Failure> Failures
        {
            get
            {
                return GetCollection<Failure>(nameof(Failures));
            }
        }

        [Association("PTF-ProjectCosts")]
        public XPCollection<ProjectCost> ProjectCosts
        {
            get
            {
                return GetCollection<ProjectCost>(nameof(ProjectCosts));
            }
        }

        [Association("PTF-Expenses")]
        public XPCollection<ProjectExpense> Expenses
        {
            get
            {
                return GetCollection<ProjectExpense>(nameof(Expenses));
            }
        }


        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
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
        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public bool HasUpdate
        {
            get => hasUpdate;
            set => SetPropertyValue(nameof(HasUpdate), ref hasUpdate, value);
        }

        int version;
        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public int Version
        {
            get => version;
            set => SetPropertyValue(nameof(Version), ref version, value);
        }
        #endregion


        private int? fTotalOK = null;

        public int? TotalOK
        {
            get
            {
                if (!IsLoading && !IsSaving && fTotalOK == null)
                    UpdateTotalOK(false);// UpdateTotalNOK(false);
                return fTotalOK;
            }
        }

        private void UpdateTotalOK(bool forceChangeEvents)
        {
            int? oldTotalOK = fTotalOK;
            int tempTotal = 0;
            foreach (Traceability trace in Traceabilities)
            {
                tempTotal += trace.OK;
            }
            fTotalOK = tempTotal;
            if (forceChangeEvents)
            {
                OnChanged("TotalOK", oldTotalOK, fTotalOK);
            }
        }

        private int? fTotalNOK = null;
        public int? TotalNOK
        {
            get
            {
                if (!IsLoading && !IsSaving && fTotalNOK == null)
                    UpdateTotalNOK(false);// UpdateTotalNOK(false);
                return fTotalNOK;
            }
        }

        private void UpdateTotalNOK(bool forceChangeEvents)
        {
            int? oldTotalNOK = fTotalNOK;
            int tempTotal = 0;
            foreach (Traceability trace in Traceabilities)
            {
                tempTotal += trace.NOK;
            }
            fTotalNOK = tempTotal;
            if (forceChangeEvents)
            {
                OnChanged("TotalNOK", oldTotalNOK, fTotalNOK);
            }
        }

        private int? fTotalRNOK = null;
        public int? TotalRNOK
        {
            get
            {
                if (!IsLoading && !IsSaving && fTotalRNOK == null)
                    UpdateTotalRNOK(false); //UpdateTotalRNOK(false);
                return fTotalRNOK;
            }
        }

        private void UpdateTotalRNOK(bool forceChangeEvents)
        {
            int? oldTotalRNOK = fTotalRNOK;
            int tempTotal = 0;
            foreach (Traceability trace in Traceabilities)
            {
                tempTotal += (int)trace.RNOK;
            }
            fTotalRNOK = tempTotal;
            if (forceChangeEvents)
            {
                OnChanged("TotalRNOK", oldTotalRNOK, fTotalRNOK);
            }
        }

        private int? fTotalROK = null;
        public int? TotalROK
        {
            get
            {
                if (!IsLoading && !IsSaving && fTotalROK == null)
                    UpdateTotalROK(false);// UpdateTotalROK(false);
                return fTotalROK;
            }
        }

        private void UpdateTotalROK(bool forceChangeEvents)
        {
            int? oldTotalROK = fTotalROK;
            int tempTotal = 0;
            foreach (Traceability trace in Traceabilities)
            {
                tempTotal += trace.ROK;
            }
            fTotalROK = tempTotal;
            if (forceChangeEvents)
            {
                OnChanged("TotalROK", oldTotalROK, fTotalROK);
            }
        }

        private double? fTH = null;

        public double? TH
        {
            get
            {
                if (!IsLoading && !IsSaving && fTH == null)
                    UpdateTH(false);
                return fTH;
            }
        }

        private void UpdateTH(bool forceChangeEvents)
        {
            double? oldTH = fTH;
            double temp = 0;

            var timesheets = this.Timesheets.ToList();

            if (timesheets.Count > 0)
            {
                foreach (var tsheet in timesheets)
                {
                    temp = temp + tsheet.Hours;
                    //temp = temp + tsheet.DHour.Value;
                }
            }
            else
            {
                temp = 0;
            }

            fTH = temp;

            if (forceChangeEvents)
            {
                OnChanged("TH", oldTH, fTH);
            }
        }

        private double? fCost= null;
        
        public double? Cost
        {
            get {

                if (!IsLoading && !IsSaving && fCost == null)
                    UpdateCost(false);
                return fCost; 
            }
        }

        private void UpdateCost(bool forceChangeEvents)
        {
            double? oldCost = fCost;
            double temp = 0;

            try
            {
                var costList = this.ProjectCosts.ToList();
                if(costList.Count > 0)
                {
                    foreach(var item in costList)
                    {
                        temp = temp + item.GrandTotal;
                    }
                }
            } catch(Exception ex)
            {
                Console.WriteLine("Can not calculate cost of PTF");
                Console.WriteLine(ex.ToString());

                temp = 0;
            }

            fCost= temp;

            if (forceChangeEvents)
            {
                OnChanged("Cost", oldCost, fCost);
            }
        }


        private double? fExpense = null;

        public double? Expense
        {
            get
            {
                if (!IsLoading && !IsSaving && fExpense == null)
                    UpdateExpense(false);
                return fExpense;
            }
        }

        private void UpdateExpense(bool forceChangeEvents)
        {
            double? oldExpense = fExpense;
            double tempExpense = 0;

            try
            {
                var listOfExpenses = this.Expenses.ToList();

                foreach (var exp in listOfExpenses)
                {
                    tempExpense = tempExpense + exp.GrandTotal;
                }
            }
            catch
            {
                tempExpense = 0;
            }

            fExpense = tempExpense;

            if (forceChangeEvents)
            {
                OnChanged("Expense", oldExpense, fExpense);
            }

        }

        protected override void OnLoaded()
        {
            Reset();
            base.OnLoaded();
        }
        private void Reset()
        {
            fTotalOK = null;
            fTotalNOK = null;
            fTotalROK = null;
            fTotalRNOK = null;
            fTH = null;
            fExpense = null;
            fCost= null;
        }

        public double NH
        {
            get => nH;
            set => SetPropertyValue(nameof(NH), ref nH, value);
        }


        public double EH
        {
            get => eH;
            set => SetPropertyValue(nameof(EH), ref eH, value);
        }

        public double HH
        {
            get => hH;
            set => SetPropertyValue(nameof(HH), ref hH, value);
        }


        public double TotalMeals
        {
            get => totalMeals;
            set => SetPropertyValue(nameof(TotalMeals), ref totalMeals, value);
        }


        public double TotalMealPrice
        {
            get => totalMealPrice;
            set => SetPropertyValue(nameof(TotalMealPrice), ref totalMealPrice, value);
        }


        public double TotalKM
        {
            get => totalKM;
            set => SetPropertyValue(nameof(TotalKM), ref totalKM, value);
        }

        
        public double TotalDisplacementPrice
        {
            get => totalDisplacementPrice;
            set => SetPropertyValue(nameof(TotalDisplacementPrice), ref totalDisplacementPrice, value);
        }
    }
}