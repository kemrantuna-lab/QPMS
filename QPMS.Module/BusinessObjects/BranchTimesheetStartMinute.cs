using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;

namespace QPMS.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class BranchTimesheetStartMinute : BaseObject
    {

        public BranchTimesheetStartMinute() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public BranchTimesheetStartMinute(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        [Association("Company-BranchTimesheetStartMinutes")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [Association("BranchTimesheetStartMinute-BranchTimesheets")]
        public XPCollection<BranchTimesheet> BranchTimesheets
        {
            get
            {
                return GetCollection<BranchTimesheet>(nameof(BranchTimesheets));
            }
        }

        string name;
        Company company;
        string startMinute;
        int val;

        public int Val
        {
            get => val;
            set => SetPropertyValue(nameof(Val), ref val, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string StartMinute
        {
            get => startMinute;
            set => SetPropertyValue(nameof(StartMinute), ref startMinute, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }
    }

}