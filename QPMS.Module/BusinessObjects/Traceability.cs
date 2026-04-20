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
    public class Traceability : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Traceability(Session session)
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

            if (PTF != null)
            {
                Date = PTF.Date;

                if(Project == null)
                {
                    if (PTF.Project != null)
                    {
                        Project = PTF.Project;
                    }
                }
            } else
            {
                Date = DateTime.Today.AddDays(-1);
            }
        }

        protected override void OnSaving()
        {
            UpdatedBy = GetCurrentUser();
            UpdatedOn = DateTime.Now;
            HasUpdate = true;
            Version = Version + 1;

            TOTAL = OK + NOK + ROK + RNOK;

            if (PTF != null)
            {
                if (PTF.Project != null)
                {
                    Project = PTF.Project;
                }


                this.PTF.HasUpdate = true;

                this.Project = this.PTF.Project;

                if (PTF.Project != null)
                {
                    this.PTF.Project.HasUpdate = true;
                }
            }

            base.OnSaving();

        }

        string finalSW;
        string initialSW;
        string vIN;
        string cabin;
        string description;
        string sERIAL;
        string bOX;
        string pALETTE;
        string dELIVERY;
        string oRDER;
        DateTime date;
        Part part;
        int tOTAL;
        int rNOK;
        int rOK;
        int nOK;
        int oK;
        PTF pTF;
        Project project;

        [RuleRequiredField("RuleRequiredField for Traceability.Project", DefaultContexts.Save, "A project must be specified")]
        [Association("Project-Traceabilities")]
        public Project Project
        {
            get => project;
            set => SetPropertyValue(nameof(Project), ref project, value);
        }

        [RuleRequiredField("RuleRequiredField for Traceability.Part", DefaultContexts.Save, "A part must be specified")]
        [DataSourceProperty("Project.Parts")]
        [Association("Part-Traceabilities")]
        public Part Part
        {
            get => part;
            set => SetPropertyValue(nameof(Part), ref part, value);
        }

        [DataSourceProperty("Project.PTFs")]
        [Association("PTF-Traceabilities")]
        public PTF PTF
        {
            get => pTF;
            set => SetPropertyValue(nameof(PTF), ref pTF, value);
        }


        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ORDER
        {
            get => oRDER;
            set => SetPropertyValue(nameof(ORDER), ref oRDER, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string DELIVERY
        {
            get => dELIVERY;
            set => SetPropertyValue(nameof(DELIVERY), ref dELIVERY, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PALETTE
        {
            get => pALETTE;
            set => SetPropertyValue(nameof(PALETTE), ref pALETTE, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string BOX
        {
            get => bOX;
            set => SetPropertyValue(nameof(BOX), ref bOX, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SERIAL
        {
            get => sERIAL;
            set => SetPropertyValue(nameof(SERIAL), ref sERIAL, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string VIN
        {
            get => vIN;
            set => SetPropertyValue(nameof(VIN), ref vIN, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Cabin
        {
            get => cabin;
            set => SetPropertyValue(nameof(Cabin), ref cabin, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string InitialSW
        {
            get => initialSW;
            set => SetPropertyValue(nameof(InitialSW), ref initialSW, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string FinalSW
        {
            get => finalSW;
            set => SetPropertyValue(nameof(FinalSW), ref finalSW, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
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



        #region activityresults

        [ModelDefault("AllowEdit", "False")]
        public int TOTAL
        {
            get => tOTAL;
            set => SetPropertyValue(nameof(TOTAL), ref tOTAL, value);
        }

        public int OK
        {
            get => oK;
            set => SetPropertyValue(nameof(OK), ref oK, value);
        }


        public int NOK
        {
            get => nOK;
            set => SetPropertyValue(nameof(NOK), ref nOK, value);
        }


        public int ROK
        {
            get => rOK;
            set => SetPropertyValue(nameof(ROK), ref rOK, value);
        }

        
        public int RNOK
        {
            get => rNOK;
            set => SetPropertyValue(nameof(RNOK), ref rNOK, value);
        }
        #endregion
    }
}