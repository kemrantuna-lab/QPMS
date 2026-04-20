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
    public class CostCenter : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public CostCenter(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {


            if (!(Session is NestedUnitOfWork)
            && (Session.DataLayer != null)
            && Session.IsNewObject(this)
            && (Session.ObjectLayer is SimpleObjectLayer)
            //OR
            //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
            && string.IsNullOrEmpty(CostCenterCode))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                CostCenterCode = string.Format("CCC-{0:D8}", nextSequence);
            }

            base.OnSaving();
        }

        string costCenterOwner;
        string costCenterCode;
        string costCenterName;
        Company company;

        [RuleRequiredField("RuleRequiredField for CostCenter.Company", DefaultContexts.Save, "A company must be specified")]
        [Association("Company-CostCenters")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [RuleRequiredField("RuleRequiredField for CostCenter.CostCenterCode", DefaultContexts.Save, "A cost center code must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CostCenterCode
        {
            get => costCenterCode;
            set => SetPropertyValue(nameof(CostCenterCode), ref costCenterCode, value);
        }

        [RuleRequiredField("RuleRequiredField for CostCenter.CostCenterName", DefaultContexts.Save, "A name must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CostCenterName
        {
            get => costCenterName;
            set => SetPropertyValue(nameof(CostCenterName), ref costCenterName, value);
        }

        [RuleRequiredField("RuleRequiredField for CostCenter.CostCenterOwner", DefaultContexts.Save, "A cost center owner must be specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CostCenterOwner
        {
            get => costCenterOwner;
            set => SetPropertyValue(nameof(CostCenterOwner), ref costCenterOwner, value);
        }


        [Association("CostCenter-Branches")]
        public XPCollection<Branch> Branches
        {
            get
            {
                return GetCollection<Branch>(nameof(Branches));
            }
        }
    }
}