using DevExpress.Xpo;
using System;

namespace QPMS.Module.BusinessObjects
{
    /***
     * Employee form non paid day
     * 
     * 
     * ***/
    public class EmployeeFormNPD : EmployeeForm
    {

        public EmployeeFormNPD(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }


        string explanation;
        DateTime endDate;
        DateTime startDate;

        public DateTime StartDate
        {
            get => startDate;
            set => SetPropertyValue(nameof(StartDate), ref startDate, value);
        }


        public DateTime EndDate
        {
            get => endDate;
            set => SetPropertyValue(nameof(EndDate), ref endDate, value);
        }

        
        [Size(SizeAttribute.Unlimited)]
        public string Explanation
        {
            get => explanation;
            set => SetPropertyValue(nameof(Explanation), ref explanation, value);
        }
    }

}