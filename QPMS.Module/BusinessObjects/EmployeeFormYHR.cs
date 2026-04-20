using DevExpress.Xpo;
using System;

namespace QPMS.Module.BusinessObjects
{

    /***
     * Employee yearly holiday request form
     * 
     * 
     * ***/
    public class EmployeeFormYHR : EmployeeForm
    {

        public EmployeeFormYHR(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }


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
    }

}