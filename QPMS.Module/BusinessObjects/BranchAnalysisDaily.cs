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
    public class BranchAnalysisDaily : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public BranchAnalysisDaily(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        double branchTimesheetQuantity;
        int traceabilityQuantity;
        double bTFPeopleQuantity;
        double pTFHolidayHours;
        double pTFExtraHours;
        double pTFNormalHours;
        double pTFTimesheetHours;
        double pTFTimesheet;
        double pTFQuantity;
        double projectEnding;
        double projectStarting;
        double dailySortedPart;
        double dailyRNOK;
        double dailyROK;
        double dailyNOK;
        double dailyOK;
        double projectQuantity;
        double totalBTFHourOfPeopleNonPaid;
        double totalBTFHourOfMissingPeople;
        double totalBTFHourOfPresentPeople;
        double totalBTF;
        double missingPeople;
        double staffQuantity;
        double workingPeople;
        DateTime date;
        Branch branch;

        [RuleRequiredField("RuleRequiredField for BranchAnalysisDaily.Branch", DefaultContexts.Save, "A branch must be specified")]
        [Association("Branch-BDA")]
        public Branch Branch
        {
            get => branch;
            set => SetPropertyValue(nameof(Branch), ref branch, value);
        }

        [RuleRequiredField("RuleRequiredField for BranchAnalysisDaily.Date", DefaultContexts.Save, "A date must be specified")]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }


        public double StaffQuantity
        {
            get => staffQuantity;
            set => SetPropertyValue(nameof(StaffQuantity), ref staffQuantity, value);
        }


        public double BTFPeopleQuantity
        {
            get => bTFPeopleQuantity;
            set => SetPropertyValue(nameof(BTFPeopleQuantity), ref bTFPeopleQuantity, value);
        }

        public double WorkingPeople
        {
            get => workingPeople;
            set => SetPropertyValue(nameof(WorkingPeople), ref workingPeople, value);
        }

        public double MissingPeople
        {
            get => missingPeople;
            set => SetPropertyValue(nameof(MissingPeople), ref missingPeople, value);
        }


        public double TotalBTF
        {
            get => totalBTF;
            set => SetPropertyValue(nameof(TotalBTF), ref totalBTF, value);
        }


        public double TotalBTFHourOfPresentPeople
        {
            get => totalBTFHourOfPresentPeople;
            set => SetPropertyValue(nameof(TotalBTFHourOfPresentPeople), ref totalBTFHourOfPresentPeople, value);
        }


        public double TotalBTFHourOfMissingPeople
        {
            get => totalBTFHourOfMissingPeople;
            set => SetPropertyValue(nameof(TotalBTFHourOfMissingPeople), ref totalBTFHourOfMissingPeople, value);
        }


        public double TotalBTFHourOfPeopleNonPaid
        {
            get => totalBTFHourOfPeopleNonPaid;
            set => SetPropertyValue(nameof(TotalBTFHourOfPeopleNonPaid), ref totalBTFHourOfPeopleNonPaid, value);
        }

        
        public double BranchTimesheetQuantity
        {
            get => branchTimesheetQuantity;
            set => SetPropertyValue(nameof(BranchTimesheetQuantity), ref branchTimesheetQuantity, value);
        }


        public double ProjectStarting
        {
            get => projectStarting;
            set => SetPropertyValue(nameof(ProjectStarting), ref projectStarting, value);
        }


        public double ProjectEnding
        {
            get => projectEnding;
            set => SetPropertyValue(nameof(ProjectEnding), ref projectEnding, value);
        }

        public double ProjectQuantity
        {
            get => projectQuantity;
            set => SetPropertyValue(nameof(ProjectQuantity), ref projectQuantity, value);
        }

        
        public int TraceabilityQuantity
        {
            get => traceabilityQuantity;
            set => SetPropertyValue(nameof(TraceabilityQuantity), ref traceabilityQuantity, value);
        }

        public double DailyOK
        {
            get => dailyOK;
            set => SetPropertyValue(nameof(DailyOK), ref dailyOK, value);
        }


        public double DailyNOK
        {
            get => dailyNOK;
            set => SetPropertyValue(nameof(DailyNOK), ref dailyNOK, value);
        }


        public double DailyROK
        {
            get => dailyROK;
            set => SetPropertyValue(nameof(DailyROK), ref dailyROK, value);
        }


        public double DailyRNOK
        {
            get => dailyRNOK;
            set => SetPropertyValue(nameof(DailyRNOK), ref dailyRNOK, value);
        }

        public double DailySortedPart
        {
            get => dailySortedPart;
            set => SetPropertyValue(nameof(DailySortedPart), ref dailySortedPart, value);
        }


        public double PTFQuantity
        {
            get => pTFQuantity;
            set => SetPropertyValue(nameof(PTFQuantity), ref pTFQuantity, value);
        }


        public double PTFTimesheet
        {
            get => pTFTimesheet;
            set => SetPropertyValue(nameof(PTFTimesheet), ref pTFTimesheet, value);
        }


        public double PTFTimesheetHours
        {
            get => pTFTimesheetHours;
            set => SetPropertyValue(nameof(PTFTimesheetHours), ref pTFTimesheetHours, value);
        }


        public double PTFNormalHours
        {
            get => pTFNormalHours;
            set => SetPropertyValue(nameof(PTFNormalHours), ref pTFNormalHours, value);
        }


        public double PTFExtraHours
        {
            get => pTFExtraHours;
            set => SetPropertyValue(nameof(PTFExtraHours), ref pTFExtraHours, value);
        }

        
        public double PTFHolidayHours
        {
            get => pTFHolidayHours;
            set => SetPropertyValue(nameof(PTFHolidayHours), ref pTFHolidayHours, value);
        }
    }
}