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
   public class Branch : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Branch(Session session)
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
        }

        protected override void OnSaving()
        {
            UpdatedBy = GetCurrentUser();
            UpdatedOn = DateTime.Now;
            HasUpdate = true;
            Version = Version + 1;
            base.OnSaving();

            if (!(Session is NestedUnitOfWork)
                && (Session.DataLayer != null)
                    && Session.IsNewObject(this)
                    && (Session.ObjectLayer is SimpleObjectLayer)
            //OR
            //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
            && string.IsNullOrEmpty(Code))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("BR{0:D8}", nextSequence);
            }

        }

        [RuleRequiredField("RuleRequiredField for Branch.Company", DefaultContexts.Save, "A company must be specified")]
        [Association("Company-Branches")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [Association("Branch-Interviews")]
        public XPCollection<Interview> Interviews
        {
            get
            {
                return GetCollection<Interview>(nameof(Interviews));
            }
        }

        string financeResponsible;
        string reporter;
        State state;
        DateTime end;
        DateTime start;
        string kAM;
        string branchResponsible;
        string branchManager;
        CostCenter costCenter;
        Branch main;
        Company company;
        string code;
        string name;
        MediaDataObject photo;


        [ImageEditor(ListViewImageEditorCustomHeight = 250, DetailViewImageEditorFixedWidth = 250)]
        public MediaDataObject Photo
        {
            get => photo;
            set => SetPropertyValue(nameof(Photo), ref photo, value);
        }

        public State State
        {
            get => state;
            set => SetPropertyValue(nameof(State), ref state, value);
        }

        [RuleRequiredField("RuleRequiredField for Branch.Name", DefaultContexts.Save, "A name must be specified")]
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

        [DataSourceCriteria("Oid!='@This.Oid'")]
        [Association("Branch-Branchs")]
        public Branch Main
        {
            get => main;
            set => SetPropertyValue(nameof(Main), ref main, value);
        }


        public DateTime Start
        {
            get => start;
            set => SetPropertyValue(nameof(Start), ref start, value);
        }


        public DateTime End
        {
            get => end;
            set => SetPropertyValue(nameof(End), ref end, value);
        }

        [DataSourceCriteria("Oid!='@This.Oid'")]
        [Association("Branch-Branchs")]
        public XPCollection<Branch> SubBranches
        {
            get
            {
                return GetCollection<Branch>(nameof(SubBranches));
            }
        }

        [Association("Branch-BDA")]
        public XPCollection<BranchAnalysisDaily> BDA
        {
            get
            {
                return GetCollection<BranchAnalysisDaily>(nameof(BDA));
            }
        }

        [Association("Branch-BranchLocations")]
        public XPCollection<BranchLocation> Locations
        {
            get
            {
                return GetCollection<BranchLocation>(nameof(Locations));
            }
        }

        [Association("Employee-Branches")]
        public XPCollection<Employee> Employees
        {
            get
            {
                return GetCollection<Employee>(nameof(Employees));
            }
        }

        [Association("Branch-BTFs")]
        public XPCollection<BTF> BTFs
        {
            get
            {
                return GetCollection<BTF>(nameof(BTFs));
            }
        }

        [Association("Branch-BranchTimesheets")]
        public XPCollection<BranchTimesheet> BranchTimesheets
        {
            get
            {
                return GetCollection<BranchTimesheet>(nameof(BranchTimesheets));
            }
        }

        [Association("Branch-Projects")]
        public XPCollection<Project> Projects
        {
            get
            {
                return GetCollection<Project>(nameof(Projects));
            }
        }

        [Association("Branch-PTFs")]
        public XPCollection<PTF> PTFs
        {
            get
            {
                return GetCollection<PTF>(nameof(PTFs));
            }
        }


        [Association("CostCenter-Branches")]
        public CostCenter CostCenter
        {
            get => costCenter;
            set => SetPropertyValue(nameof(CostCenter), ref costCenter, value);
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


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string BranchManager
        {
            get => branchManager;
            set => SetPropertyValue(nameof(BranchManager), ref branchManager, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string BranchResponsible
        {
            get => branchResponsible;
            set => SetPropertyValue(nameof(BranchResponsible), ref branchResponsible, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string KAM
        {
            get => kAM;
            set => SetPropertyValue(nameof(KAM), ref kAM, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Reporter
        {
            get => reporter;
            set => SetPropertyValue(nameof(Reporter), ref reporter, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string FinanceResponsible
        {
            get => financeResponsible;
            set => SetPropertyValue(nameof(FinanceResponsible), ref financeResponsible, value);
        }

        //[Association("Branch-BranchExpenses")]
        //public XPCollection<BranchExpense> BranchExpenses
        //{
        //    get
        //    {
        //        return GetCollection<BranchExpense>(nameof(BranchExpenses));
        //    }
        //}

        [Association("Branch-EmployeeForms")]
        public XPCollection<EmployeeForm> EmployeeForms
        {
            get
            {
                return GetCollection<EmployeeForm>(nameof(EmployeeForms));
            }
        }
    }
}