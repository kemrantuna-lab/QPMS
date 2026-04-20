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
    public class Part : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Part(Session session)
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


            if (!(Session is NestedUnitOfWork)
               && (Session.DataLayer != null)
                   && Session.IsNewObject(this)
                   && (Session.ObjectLayer is SimpleObjectLayer)
           //OR
           //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
           && string.IsNullOrEmpty(Code))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("PART-{0:D8}", nextSequence);
            }

            base.OnSaving();
        }
        


        [ImageEditor(ListViewImageEditorCustomHeight = 250, DetailViewImageEditorFixedWidth = 250)]
        public MediaDataObject Photo
        {
            get => photo;
            set => SetPropertyValue(nameof(Photo), ref photo, value);
        }


        Cur currency;
        double price;
        MediaDataObject photo;
        Part main;
        Project project;
        string supplierName;
        string supplierCode;
        string oEMCode;
        string oEMName;
        string code;
        string name;

        [RuleRequiredField("RuleRequiredField for Part.Name", DefaultContexts.Save, "A name must be specified")]
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


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string OEMName
        {
            get => oEMName;
            set => SetPropertyValue(nameof(OEMName), ref oEMName, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string OEMCode
        {
            get => oEMCode;
            set => SetPropertyValue(nameof(OEMCode), ref oEMCode, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SupplierCode
        {
            get => supplierCode;
            set => SetPropertyValue(nameof(SupplierCode), ref supplierCode, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SupplierName
        {
            get => supplierName;
            set => SetPropertyValue(nameof(SupplierName), ref supplierName, value);
        }

        [RuleRequiredField("RuleRequiredField for Part.Project", DefaultContexts.Save, "A project must be specified")]
        [Association("Project-Parts")]
        public Project Project
        {
            get => project;
            set => SetPropertyValue(nameof(Project), ref project, value);
        }

        [DataSourceCriteria("Oid!='@This.Oid'")]
        [Association("Part-Parts")]
        public Part Main
        {
            get => main;
            set => SetPropertyValue(nameof(Main), ref main, value);
        }

        [DataSourceCriteria("Oid!='@This.Oid'")]
        [Association("Part-Parts")]
        public XPCollection<Part> SubParts
        {
            get
            {
                return GetCollection<Part>(nameof(SubParts));
            }
        }

        [Association("Part-Traceabilities")]
        public XPCollection<Traceability> Traceabilities
        {
            get
            {
                return GetCollection<Traceability>(nameof(Traceabilities));
            }
        }


        [Association("Part-Failures")]
        public XPCollection<Failure> Failures
        {
            get
            {
                return GetCollection<Failure>(nameof(Failures));
            }
        }


        public double Price
        {
            get => price;
            set => SetPropertyValue(nameof(Price), ref price, value);
        }

        
        [Association("Cur-Parts")]
        public Cur Currency
        {
            get => currency;
            set => SetPropertyValue(nameof(Currency), ref currency, value);
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

    public enum PartType
    {
        METAL,
        PLASTIC,
        ELECTRONIC,
        ASSEMBLY,
        SOFTWARE,
        CABIN,
        CAR,
        TRUCK
    }
}