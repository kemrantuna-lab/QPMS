using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;

namespace QPMS.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class QPMSReport : ReportDataV2
    {
        //public QPMSReport() : base()
        //{
        //    // This constructor is used when an object is loaded from a persistent storage.
        //    // Do not place any code here.
        //}

        public QPMSReport(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
            CreatedBy = GetCurrentUser();
            CreatedOn = DateTime.Now;

            HasUpdate = true;
            Version = 0;
        }

        protected override void OnSaving()
        {
            HasUpdate = true;
            Version = Version + 1;
            if (!(Session is NestedUnitOfWork)
               && (Session.DataLayer != null)
                   && Session.IsNewObject(this)
                   && (Session.ObjectLayer is SimpleObjectLayer)
           //OR
           //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
           && string.IsNullOrEmpty(Code))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("REPORT-{0:D8}", nextSequence);
            }
            base.OnSaving();
        }


        Company company;
        string code;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }


        [Association("Company-Reports")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [Association("EmployeeRole-QPMSReports")]
        public XPCollection<EmployeeRole> EmployeeRoles
        {
            get
            {
                return GetCollection<EmployeeRole>(nameof(EmployeeRoles));
            }
        }

        #region ORERPRecordDetailsRegion
        bool hasUpdate;
        int version;
        //MediaDataObject photo;

        //[ImageEditor(ListViewImageEditorCustomHeight = 250, DetailViewImageEditorFixedWidth = 250)]
        //public MediaDataObject Photo
        //{
        //    get => photo;
        //    set => SetPropertyValue(nameof(Photo), ref photo, value);
        //}
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

        [DataSourceProperty("Company.Projects")]
        [Association("QPMSReport-Projects")]
        public XPCollection<Project> Projects
        {
            get
            {
                return GetCollection<Project>(nameof(Projects));
            }
        }
    }

}