using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraSpellChecker.Parser;
using System;
using System.Activities.Statements;
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
    public class BranchTimesheet : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public BranchTimesheet(Session session)
            : base(session)
        {
        }

        protected override void OnLoaded()

        {
            Reset();
            base.OnLoaded();
        }

        double workingHour;
        int mealDuration;
        double hour;
        double duration;
        CompanyAbsence absence;
        string transportPriceCurrency;
        string mealPriceCurrency;
        double transportPrice;
        double transportKM;
        double mealQuantity;
        double mealPrice;
        int endMinute;
        int startHour;
        BranchTimesheetEndMinute eM;
        BranchTimesheetEndHour eH;
        BranchTimesheetStartMinute sM;
        BranchTimesheetStartHour sH;

        private void Reset()
        {
            Debug.WriteLine("RESET ");


        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            CreatedBy = GetCurrentUser();
            CreatedOn = DateTime.Now;

            Version = 1;
            HasUpdate = true;

            Date = DateTime.Today.AddDays(-1);

            MealDuration = 30;

            #region startconstructionofbt
            //try
            //{
            //    if (Branch != null)
            //    {
            //        try
            //        {
            //            if (Branch.Company != null)
            //            {
            //                Presence = Branch.Company.CompanyPresences.FirstOrDefault();
            //                Absence = Branch.Company.CompanyAbsences.FirstOrDefault();

            //                SH = Branch.Company.BranchTimesheetStartHours.FirstOrDefault(p => p.Val == 8);
            //                SM = Branch.Company.BranchTimesheetStartMinutes.FirstOrDefault(p => p.Val == 0);
            //                EH = Branch.Company.BranchTimesheetEndHours.FirstOrDefault(p => p.Val == 8);
            //                EM = Branch.Company.BranchTimesheetEndMinutes.FirstOrDefault(p => p.Val == 0);
            //            }
            //        }
            //        catch
            //        {
            //            throw new Exception("Branch of timesheet detected. Branch Timesheet failed during obtaning one of SH,SM,EH,EM or Presence or Absence value");
            //        }

            //    }
            //    else if (BTF != null)
            //    {
            //        try
            //        {
            //            if (BTF.Branch != null)
            //            {
            //                try
            //                {
            //                    if (BTF.Branch.Company != null)
            //                    {
            //                        try
            //                        {
            //                            Presence = BTF.Branch.Company.CompanyPresences.FirstOrDefault();
            //                            Absence = BTF.Branch.Company.CompanyAbsences.FirstOrDefault();

            //                            SH = BTF.Branch.Company.BranchTimesheetStartHours.FirstOrDefault(p => p.Val == 8);
            //                            SM = BTF.Branch.Company.BranchTimesheetStartMinutes.FirstOrDefault(p => p.Val == 0);
            //                            EH = BTF.Branch.Company.BranchTimesheetEndHours.FirstOrDefault(p => p.Val == 8);
            //                            EM = BTF.Branch.Company.BranchTimesheetEndMinutes.FirstOrDefault(p => p.Val == 0);
            //                        }
            //                        catch
            //                        {
            //                            throw new Exception("Branch of branch timesheet has not been detected. BTF has been detected but there has been an issue while trying to obtain Presence,Absence, SH,SM,EH,EM values.");
            //                        }
            //                    }
            //                }
            //                catch
            //                {
            //                    throw new Exception("BTF of timesheet detected and branch obtained properly. While trying to obtain Company ID there has been an issue.");
            //                }


            //            }
            //        }
            //        catch
            //        {
            //            throw new Exception("Branch Timesheet did not manage to obtain BTF value.");
            //        }

            //    }
            //}
            //catch
            //{
            //    throw new Exception("Branch Timesheet could not be constructed.");
            //}
            #endregion
        }

        protected override void OnLoading()
        {
            base.OnLoading();
        }

        protected override void OnSaving()
        {
            UpdatedBy = GetCurrentUser();
            UpdatedOn = DateTime.Now;
            HasUpdate = true;
            Version = Version + 1;

            Debug.WriteLine(SH.Val.ToString() + SM.Val.ToString());
            Debug.WriteLine(EH.Val.ToString() + EM.Val.ToString());


            if (BTF != null)
            {
                BTF.HasUpdate = true;

                if (BTF.Branch != null)
                {
                    BTF.Branch.HasUpdate = true;
                }
            }

            try
            {
                if (!(Session is NestedUnitOfWork)
                    && (Session.DataLayer != null)
                    && Session.IsNewObject(this)
                    && (Session.ObjectLayer is SimpleObjectLayer)
                    //OR
                    //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
                    && string.IsNullOrEmpty(Code))
                {
                    int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                    Code = string.Format("BRT-{0:D7}", nextSequence);
                }
            }
            catch
            {
                throw new Exception("Branch Timesheet could not obtain code");
            }


            DateTime startT = new DateTime(Date.Year, Date.Month, Date.Day, SH.Val, SM.Val, 0);

            DateTime endT = DateTime.Now;

            if (EH.Val >= 24)
            {
                endT = new DateTime(Date.Year, Date.Month, Date.Day, 0, 0, 0).AddDays(1);
                EM = CreatedBy.Company.BranchTimesheetEndMinutes.Where(p => p.Val == 0 && p.Company.Oid == UpdatedBy.Company.Oid).FirstOrDefault();
            }
            else
            {
                endT = new DateTime(Date.Year, Date.Month, Date.Day, EH.Val, EM.Val, 0);

                if (endT < startT)
                {

                    EH = CreatedBy.Company.BranchTimesheetEndHours.Where(p => p.Val == SH.Val && p.Company.Oid == UpdatedBy.Company.Oid).FirstOrDefault();
                    EM = CreatedBy.Company.BranchTimesheetEndMinutes.Where(p => p.Val == SM.Val && p.Company.Oid == UpdatedBy.Company.Oid).FirstOrDefault();
                    endT = new DateTime(Date.Year, Date.Month, Date.Day, EH.Val, EM.Val, 0);
                }
            }
            //DateTime endT = new DateTime(Date.Year, Date.Month, Date.Day, EH.Val, EM.Val, 0);

            Duration = endT.Subtract(startT).TotalMinutes;

            WorkingHour = Duration - MealDuration;

            Hour = endT.Subtract(startT).TotalHours;

            base.OnSaving();
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }


        //[DataSourceCriteria("[Employees][[Oid] = CurrentUserId()]")]
        [RuleRequiredField("RuleRequiredField for BranchTimesheet.Branch", DefaultContexts.Save, "A branch must be specified")]
        [Association("Branch-BranchTimesheets")]
        public Branch Branch
        {
            get => branch;
            set => SetPropertyValue(nameof(Branch), ref branch, value);
        }

        [RuleRequiredField("RuleRequiredField for BranchTimesheet.Presence", DefaultContexts.Save, "A Presence type must be specified")]
        //[DataSourceProperty("Branch.Company.CompanyPresences")]
        [Association("CompanyPresence-BranchTimesheets")]
        public CompanyPresence Presence
        {
            get => presence;
            set => SetPropertyValue(nameof(Presence), ref presence, value);
        }

        [RuleRequiredField("RuleRequiredField for BranchTimesheet.Absence", DefaultContexts.Save, "An absence type must be specified")]
        //[DataSourceProperty("Branch.Company.CompanyAbsences")]
        [Association("CompanyAbsence-BranchTimesheets")]
        public CompanyAbsence Absence
        {
            get => absence;
            set => SetPropertyValue(nameof(Absence), ref absence, value);
        }

        string code;
        CompanyPresence presence;
        DateTime date;
        Branch branch;
        Employee employee;


        //[DataSourceProperty("Branch.Employees")]
        [RuleRequiredField("RuleRequiredField for BranchTimesheet.Emloyee", DefaultContexts.Save, "An employee must be specified")]
        [Association("Employee-BranchTimesheets")]
        public Employee Employee
        {
            get => employee;
            set => SetPropertyValue(nameof(Employee), ref employee, value);
        }

        [RuleRequiredField("RuleRequiredField for BranchTimesheet.Date", DefaultContexts.Save, "A date must be specified")]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        [DataSourceProperty("Branch.BTFs")]
        [Association("BTF-BranchTimesheets")]
        public BTF BTF
        {
            get => bTF;
            set => SetPropertyValue(nameof(BTF), ref bTF, value);
        }

        BTF bTF;

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


        #region employeetimesheetregion
        [Browsable(false)]
        public int StartHour
        {
            get => startHour;
            set => SetPropertyValue(nameof(StartHour), ref startHour, value);
        }


        int startMinute;

        int endHour;

        [Browsable(false)]
        public int EndMinute
        {
            get => endMinute;
            set => SetPropertyValue(nameof(EndMinute), ref endMinute, value);
        }

        [Browsable(false)]
        public int StartMinute
        {
            get => startMinute;
            set => SetPropertyValue(nameof(StartMinute), ref startMinute, value);
        }

        [Browsable(false)]

        public int EndHour
        {
            get => endHour;
            set => SetPropertyValue(nameof(EndHour), ref endHour, value);
        }



        #endregion


        #region TimesheetHoursRegion

        [RuleRequiredField("RuleRequiredField for BranchTimesheet.SH", DefaultContexts.Save, "A start hour must be specified")]
        //[DataSourceProperty("BTF.Branch.Company.BranchTimesheetStartHours || Branch.Company.BranchTimesheetStartHours")]
        [Association("BranchTimesheetStartHour-BranchTimesheets")]
        public BranchTimesheetStartHour SH
        {
            get => sH;
            set => SetPropertyValue(nameof(SH), ref sH, value);
        }

        [RuleRequiredField("RuleRequiredField for BranchTimesheet.SM", DefaultContexts.Save, "A start minute must be specified")]
        [Association("BranchTimesheetStartMinute-BranchTimesheets")]
        public BranchTimesheetStartMinute SM
        {
            get => sM;
            set => SetPropertyValue(nameof(SM), ref sM, value);
        }

        [RuleRequiredField("RuleRequiredField for BranchTimesheet.EH", DefaultContexts.Save, "An end hour must be specified")]
        [Association("BranchTimesheetEndHour-BranchTimesheets")]
        public BranchTimesheetEndHour EH
        {
            get => eH;
            set => SetPropertyValue(nameof(EH), ref eH, value);
        }

        [RuleRequiredField("RuleRequiredField for BranchTimesheet.EM", DefaultContexts.Save, "An end minute must be specified")]
        [Association("BranchTimesheetEndMinute-BranchTimesheets")]
        public BranchTimesheetEndMinute EM
        {
            get => eM;
            set => SetPropertyValue(nameof(EM), ref eM, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public double Duration
        {
            get => duration;
            set => SetPropertyValue(nameof(Duration), ref duration, value);
        }

        
        public double WorkingHour
        {
            get => workingHour;
            set => SetPropertyValue(nameof(WorkingHour), ref workingHour, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public double Hour
        {
            get => hour;
            set => SetPropertyValue(nameof(Hour), ref hour, value);
        }

        #endregion

        
        public int MealDuration
        {
            get => mealDuration;
            set => SetPropertyValue(nameof(MealDuration), ref mealDuration, value);
        }

        public double MealPrice
        {
            get => mealPrice;
            set => SetPropertyValue(nameof(MealPrice), ref mealPrice, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string MealPriceCurrency
        {
            get => mealPriceCurrency;
            set => SetPropertyValue(nameof(MealPriceCurrency), ref mealPriceCurrency, value);
        }

        public double MealQuantity
        {
            get => mealQuantity;
            set => SetPropertyValue(nameof(MealQuantity), ref mealQuantity, value);
        }


        public double TransportKM
        {
            get => transportKM;
            set => SetPropertyValue(nameof(TransportKM), ref transportKM, value);
        }


        public double TransportPrice
        {
            get => transportPrice;
            set => SetPropertyValue(nameof(TransportPrice), ref transportPrice, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TransportPriceCurrency
        {
            get => transportPriceCurrency;
            set => SetPropertyValue(nameof(TransportPriceCurrency), ref transportPriceCurrency, value);
        }
    }
}