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
    public class InvoiceProjectPeriod : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public InvoiceProjectPeriod(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            CreatedBy = GetCurrentUser();
            CreatedOn = DateTime.Now;
            Version = 0;
            HasUpdate = true;
        }

        protected override void OnSaving()
        {
            UpdatedBy = GetCurrentUser();
            UpdatedOn = DateTime.Now;
            base.OnSaving();
        }


        DateTime end;
        DateTime start;
        Project project;
        Invoice invoice;

        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "True"), ModelDefault("IsVisibleInDetailView", "True")]
        [Association("Invoice-InvoiceProjectPeriods")]
        public Invoice Invoice
        {
            get => invoice;
            set => SetPropertyValue(nameof(Invoice), ref invoice, value);
        }

        [ModelDefault("AllowEdit", "True"), ModelDefault("IsVisibleInListView", "True"), ModelDefault("IsVisibleInDetailView", "True")]
        [Association("Project-InvoiceProjectPeriods")]
        public Project Project
        {
            get => project;
            set => SetPropertyValue(nameof(Project), ref project, value);
        }


        [ModelDefault("AllowEdit", "True"), ModelDefault("IsVisibleInListView", "True"), ModelDefault("IsVisibleInDetailView", "True")]
        public DateTime Start
        {
            get => start;
            set => SetPropertyValue(nameof(Start), ref start, value);
        }


        [ModelDefault("AllowEdit", "True"), ModelDefault("IsVisibleInListView", "True"), ModelDefault("IsVisibleInDetailView", "True")]
        public DateTime End
        {
            get => end;
            set => SetPropertyValue(nameof(End), ref end, value);
        }

        private double? fNormalHours = null;

        public double? NormalHours
        {
            get
            {
                if (!IsLoading && IsSaving && fNormalHours == null)
                    UpdateNormalHours(false);
                return fNormalHours;
            }
        }

        private void UpdateNormalHours(bool forceChangeEvents)
        {
            double? oldNormalHours = fNormalHours;
            double tempNormalHours = 0;

            try
            {
                var listOfPTFs = Project.PTFs.Where(p=>p.Date>=Start && p.Date<=End).ToList();

                if(listOfPTFs.Any())
                {
                    foreach(var p in listOfPTFs)
                    {
                        tempNormalHours = tempNormalHours + p.NH;
                    }
                }
            }catch
            {

            }

            fNormalHours = tempNormalHours;

            if (forceChangeEvents)
            {
                OnChanged("NormalHours", oldNormalHours, fNormalHours);
            }
        }

        private double? fExtraHours = null;

        public double? ExtraHours
        {
            get
            {
                if (!IsLoading && IsSaving && fExtraHours == null)
                    UpdateExtraHours(false);
                return fExtraHours;
            }
        }

        private void UpdateExtraHours(bool forceChangeEvents)
        {
            double? oldExtraHours = fExtraHours;
            double tempExtraHours = 0;

            try
            {
                var listOfPTFs = Project.PTFs.Where(p => p.Date >= Start && p.Date <= End).ToList();

                if (listOfPTFs.Any())
                {
                    foreach (var p in listOfPTFs)
                    {
                        tempExtraHours = tempExtraHours + p.EH;
                    }
                }
            }
            catch
            {

            }

            fExtraHours = tempExtraHours;

            if (forceChangeEvents)
            {
                OnChanged("ExtraHours", oldExtraHours, fExtraHours);
            }
        }

        private double? fHolidayHours = null;

        public double? HolidayHours
        {
            get
            {
                if (!IsLoading && IsSaving && fHolidayHours == null)
                    UpdateHolidayHours(false);
                return fExtraHours;
            }
        }

        private void UpdateHolidayHours(bool forceChangeEvents)
        {
            double? oldHolidayHours = fHolidayHours;
            double tempHolidayHours = 0;

            try
            {
                var listOfPTFs = Project.PTFs.Where(p => p.Date >= Start && p.Date <= End).ToList();

                if (listOfPTFs.Any())
                {
                    foreach (var p in listOfPTFs)
                    {
                        tempHolidayHours = tempHolidayHours + p.HH;
                    }
                }
            }
            catch
            {

            }

            fHolidayHours = tempHolidayHours;

            if (forceChangeEvents)
            {
                OnChanged("HolidayHours", oldHolidayHours, fHolidayHours);
            }
        }

        private void Reset()
        {
            fHolidayHours = null;
            fExtraHours= null;
            fNormalHours= null;
        }

        protected override void OnLoaded()
        {
            Reset();
            base.OnLoaded();
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



    }
}