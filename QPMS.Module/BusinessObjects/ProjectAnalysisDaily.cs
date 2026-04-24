using DevExpress.DataAccess.Sql;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace QPMS.Module.BusinessObjects
{
    //[DefaultClassOptions]
    public class ProjectAnalysisDaily : XPObject
    {
        public ProjectAnalysisDaily() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ProjectAnalysisDaily(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        protected override void OnLoaded()
        {
            Reset();
            base.OnLoaded();
        }

        private void Reset()
        {
            fOK = null;
            fNOK= null;
            fRNOK= null;
            fROK= null;
            fTotal = null;
            fTimesheetDurationHours= null; 
            fTimesheetDurationMinutes= null;
            fPTFDurationHours= null;
            fPTFDurationHH= null;
            fPTFDurationEH= null;
            fPTFDurationNH= null;
            fOrderQuantityNA= null;
        }

        DateTime date;
        Project project;

        [RuleRequiredField("RuleRequiredField for ProjectAnalysisDaily.Project", DefaultContexts.Save, "A project must be specified")]
        [Association("Project-PDA")]
        public Project Project
        {
            get => project;
            set => SetPropertyValue(nameof(Project), ref project, value);
        }

        [RuleRequiredField("RuleRequiredField for ProjectAnalysisDaily.Date", DefaultContexts.Save, "A date must be specified")]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        private double? fOK = null;

        public double? OK
        {
            get
            {
           
                    if (!IsLoading && !IsSaving && fOK == null)
                        UpdateOK(false);
                    return fOK;
                
            }
        }

        private void UpdateOK(bool forceChangeEvents)
        {
            double? oldOK = fOK;
            double tempOK = 0;

            if (this.Project != null && this.Project.Traceabilities.Count>0)
            {
                var list = this.Project.Traceabilities.Where(p=>p.Date == this.Date).ToList();
                if(list.Count > 0)
                {
                    foreach(var item in list)
                    {
                        try
                        {
                            tempOK = tempOK + item.OK;
                        } catch
                        {

                        }
                        
                    }
                } 
            }
            else
            {
                tempOK = 0;
            }

            fOK = tempOK;

            if (forceChangeEvents)
            {
                OnChanged("OK", oldOK, fOK);
            }
        }

        private double? fNOK = null;

        public double? NOK
        {
            get
            {

                if (!IsLoading && !IsSaving && fNOK == null)
                    UpdateNOK(false);
                return fNOK;

            }
        }

        private void UpdateNOK(bool forceChangeEvents)
        {
            double? oldNOK = fNOK;
            double tempNOK = 0;

            if (this.Project != null && this.Project.Traceabilities.Count > 0)
            {
                var list = this.Project.Traceabilities.Where(p => p.Date == this.Date).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        try
                        {
                            tempNOK = tempNOK + item.NOK;
                        }
                        catch
                        {

                        }

                    }
                }
            }
            else
            {
                tempNOK = 0;
            }

            fNOK = tempNOK;

            if (forceChangeEvents)
            {
                OnChanged("NOK", oldNOK, fNOK);
            }
        }

        private double? fROK = null;

        public double? ROK
        {
            get
            {

                if (!IsLoading && !IsSaving && fROK == null)
                    UpdateROK(false);
                return fROK;

            }
        }

        private void UpdateROK(bool forceChangeEvents)
        {
            double? oldROK = fROK;
            double tempROK = 0;

            if (this.Project != null && this.Project.Traceabilities.Count > 0)
            {
                var list = this.Project.Traceabilities.Where(p => p.Date == this.Date).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        try
                        {
                            tempROK = tempROK + item.ROK;
                        }
                        catch
                        {

                        }

                    }
                }
            }
            else
            {
                tempROK = 0;
            }

            fROK = tempROK;

            if (forceChangeEvents)
            {
                OnChanged("ROK", oldROK, fROK);
            }
        }

        private double? fRNOK = null;

        public double? RNOK
        {
            get
            {

                if (!IsLoading && !IsSaving && fRNOK == null)
                    UpdateRNOK(false);
                return fRNOK;

            }
        }

        private void UpdateRNOK(bool forceChangeEvents)
        {
            double? oldRNOK = fRNOK;
            double tempRNOK = 0;

            if (this.Project != null && this.Project.Traceabilities.Count > 0)
            {
                var list = this.Project.Traceabilities.Where(p => p.Date == this.Date).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        try
                        {
                            tempRNOK = tempRNOK + item.RNOK;
                        }
                        catch
                        {

                        }

                    }
                }
            }
            else
            {
                tempRNOK = 0;
            }

            fRNOK = tempRNOK;

            if (forceChangeEvents)
            {
                OnChanged("RNOK", oldRNOK, fRNOK);
            }
        }


        private double? fTotal = null;
        public double Total
        {
            get
            {

                if (!IsLoading && !IsSaving && fTotal == null)
                    UpdateTotal(false);
                return (double)fTotal;

            }
        }

        private void UpdateTotal(bool forceChangeEvents)
        {
            double? old = fTotal;
            double temp = 0;

            if (this.Project != null && this.Project.Traceabilities.Count > 0)
            {
                var list = this.Project.Traceabilities.Where(p => p.Date == this.Date).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        try
                        {
                            temp = temp + item.OK + item.ROK + item.NOK + item.RNOK;
                        }
                        catch
                        {

                        }

                    }
                }
            }
            else
            {
                temp = 0;
            }

            fTotal = temp;

            if (forceChangeEvents)
            {
                OnChanged("Total", old, fTotal);
            }
        }

        //Calculates duration from timesheets associated to project
        private double? fTimesheetDurationMinutes = null;
       
        public double? TimesheetDurationMinutes
        {
            get
            {

                if (!IsLoading && !IsSaving && fTimesheetDurationMinutes == null)
                    UpdateTimesheetDurationMinutes(false);
                return fTimesheetDurationMinutes;

            }
        }

        private void UpdateTimesheetDurationMinutes(bool forceChangeEvents)
        {
            double? old = fTimesheetDurationMinutes;
            double temp = 0;

            if (this.Project != null && this.Project.Timesheets.Count > 0)
            {
                var list = this.Project.Timesheets.Where(p => p.Date == this.Date).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        try
                        {
                            temp = temp + item.Minutes;
                        }
                        catch
                        {

                        }

                    }
                }
            }
            else
            {
                temp = 0;
            }

            fTimesheetDurationMinutes = temp;

            if (forceChangeEvents)
            {
                OnChanged("TimesheetDurationMinutes", old, fTimesheetDurationMinutes);
            }
        }

        private double? fTimesheetDurationHours = null;

        public double? TimesheetDurationHours
        {
            get
            {

                if (!IsLoading && !IsSaving && fTimesheetDurationHours == null)
                    UpdateTimesheetDurationHours(false);
                return fTimesheetDurationHours;

            }
        }

        private void UpdateTimesheetDurationHours(bool forceChangeEvents)
        {
            double? old = fTimesheetDurationHours;
            double temp = 0;

            if (this.Project != null && this.Project.Timesheets.Count > 0)
            {
                var list = this.Project.Timesheets.Where(p => p.Date == this.Date).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        try
                        {
                            temp = temp + item.Hours;
                        }
                        catch
                        {

                        }

                    }
                }
            }
            else
            {
                temp = 0;
            }

            fTimesheetDurationHours = temp;

            if (forceChangeEvents)
            {
                OnChanged("TimesheetDurationHours", old, fTimesheetDurationHours);
            }
        }

        private double? fPTFDurationHours = null;

        public double? PTFDurationHours
        {
            get
            {

                if (!IsLoading && !IsSaving && fPTFDurationHours == null)
                    UpdatePTFDurationHours(false);
                return fPTFDurationHours;

            }
        }

        private void UpdatePTFDurationHours(bool forceChangeEvents)
        {
            double? old = fPTFDurationHours;
            double temp = 0;

            if (this.Project != null && this.Project.PTFs.Count > 0)
            {
                var list = this.Project.PTFs.Where(p => p.Date == this.Date).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        try
                        {
                            double tempTH = double.Parse(item.TH.ToString());

                            temp = temp + tempTH;
                        }
                        catch
                        {

                        }

                    }
                }
            }
            else
            {
                temp = 0;
            }

            fPTFDurationHours = temp;

            if (forceChangeEvents)
            {
                OnChanged("PTFDurationHours", old, fPTFDurationHours);
            }
        }

        private double? fPTFDurationNH = null;

        public double? PTFDurationNH
        {
            get
            {

                if (!IsLoading && !IsSaving && fPTFDurationNH == null)
                    UpdatePTFDurationNH(false);
                return fPTFDurationNH;

            }
        }

        private void UpdatePTFDurationNH(bool forceChangeEvents)
        {
            double? old = fPTFDurationNH;
            double temp = 0;

            if (this.Project != null && this.Project.PTFs.Count > 0)
            {
                var list = this.Project.PTFs.Where(p => p.Date == this.Date).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        try
                        {
                            
                            temp = temp + item.NH;
                        }
                        catch
                        {

                        }

                    }
                }
            }
            else
            {
                temp = 0;
            }

            fPTFDurationNH = temp;

            if (forceChangeEvents)
            {
                OnChanged("PTFDurationNH", old, fPTFDurationNH);
            }
        }

        private double? fPTFDurationEH = null;

        public double? PTFDurationEH
        {
            get
            {

                if (!IsLoading && !IsSaving && fPTFDurationEH == null)
                    UpdatePTFDurationEH(false);
                return fPTFDurationEH;

            }
        }

        private void UpdatePTFDurationEH(bool forceChangeEvents)
        {
            double? old = fPTFDurationEH;
            double temp = 0;

            if (this.Project != null && this.Project.PTFs.Count > 0)
            {
                var list = this.Project.PTFs.Where(p => p.Date == this.Date).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        try
                        {

                            temp = temp + item.EH;
                        }
                        catch
                        {

                        }

                    }
                }
            }
            else
            {
                temp = 0;
            }

            fPTFDurationEH = temp;

            if (forceChangeEvents)
            {
                OnChanged("PTFDurationEH", old, fPTFDurationEH);
            }
        }

        private double? fPTFDurationHH = null;

        public double? PTFDurationHH
        {
            get
            {

                if (!IsLoading && !IsSaving && fPTFDurationHH == null)
                    UpdatePTFDurationHH(false);
                return fPTFDurationHH;

            }
        }

        private void UpdatePTFDurationHH(bool forceChangeEvents)
        {
            double? old = fPTFDurationHH;
            double temp = 0;

            if (this.Project != null && this.Project.PTFs.Count > 0)
            {
                var list = this.Project.PTFs.Where(p => p.Date == this.Date).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        try
                        {

                            temp = temp + item.HH;
                        }
                        catch
                        {

                        }

                    }
                }
            }
            else
            {
                temp = 0;
            }

            fPTFDurationHH = temp;

            if (forceChangeEvents)
            {
                OnChanged("PTFDurationEH", old, fPTFDurationHH);
            }
        }

        //This parameter counts how many orders are not entered propery. Strong typing is welcome
        private double? fOrderQuantityNA = null;

        public double? OrderQuantityNA
        {
            get
            {

                if (!IsLoading && !IsSaving && fOrderQuantityNA == null)
                    UpdateOrderQuantityNA(false);
                return fOrderQuantityNA;

            }
        }

        private void UpdateOrderQuantityNA(bool forceChangeEvents)
        {
            double? old = fOrderQuantityNA;
            double temp = 0;

            if (this.Project != null && this.Project.Traceabilities.Count > 0)
            {
                var list = this.Project.Traceabilities.Where(p => p.Date == this.Date).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        try
                        {
                            if(item.ORDER == null || item.ORDER == "NA")
                            {
                                temp = temp + 1;
                            }
                        }
                        catch
                        {

                        }

                    }
                }
            }
            else
            {
                temp = 0;
            }

            fOrderQuantityNA = temp;

            if (forceChangeEvents)
            {
                OnChanged("OrderQuantityNA", old, fOrderQuantityNA);
            }
        }
    }

}
