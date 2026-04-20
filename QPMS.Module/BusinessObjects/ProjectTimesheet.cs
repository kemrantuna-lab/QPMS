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
using System.Diagnostics;
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
    public class ProjectTimesheet : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public ProjectTimesheet(Session session)
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

            Date = DateTime.Today.AddDays(-1);

            MealDuration = 30;

            try
            {
                SH = this.Session.FindObject<ProjectTimesheetStartHour>(
                   new BinaryOperator("Val", 8));

                SM = this.Session.FindObject<ProjectTimesheetStartMinute>(
                    new BinaryOperator("Val", 0));

                EH = this.Session.FindObject<ProjectTimesheetEndHour>(
                    new BinaryOperator("Val", 16));

                EM = this.Session.FindObject<ProjectTimesheetEndMinute>(
                    new BinaryOperator("Val", 0));

            } catch
            {

            }
        }

        protected override void OnSaving()
        {
            UpdatedBy = GetCurrentUser();
            UpdatedOn = DateTime.Now;
            HasUpdate = true;
            Version = Version + 1;

            StartDT = new DateTime(Date.Year, Date.Month, Date.Day, SH.Val, SM.Val, 0);

            if (EH.Val == 24)
            {
                var tempEndHour = this.Session.FindObject<ProjectTimesheetEndHour>(
                        new BinaryOperator("Val", 0)
                    );

                var tempEndMinute = this.Session.FindObject<ProjectTimesheetEndMinute>(
                        new BinaryOperator("Val",0)
                    );

                

                if (tempEndHour!=null && tempEndMinute != null)
                {
                    //EH = tempEndHour;
                    EM = tempEndMinute;

                    EndDT = new DateTime(Date.Year, Date.Month, Date.Day, tempEndHour.Val, EM.Val, 0).AddDays(1);
                } else
                {
                    EndDT = StartDT;
                }
            } else
            {
                EndDT = new DateTime(Date.Year, Date.Month, Date.Day, EH.Val, EM.Val, 0);

                if (EndDT < StartDT)
                {
                    EndDT = StartDT;

                    var tempEndHour = this.Session.FindObject<ProjectTimesheetEndHour>(
                        new BinaryOperator("Val", SH.Val)
                    );

                    var tempEndMinute = this.Session.FindObject<ProjectTimesheetEndMinute>(
                            new BinaryOperator("Val", SM.Val)
                        );

                    if(tempEndHour!=null && tempEndMinute != null)
                    {
                        EH = tempEndHour;
                        EM = tempEndMinute;
                    }
                }
            }

            Debug.WriteLine("Start : " + StartDT.ToString() + " " + "End : " + EndDT.ToString());
            
            Minutes = EndDT.Subtract(StartDT).TotalMinutes;
            Hours = EndDT.Subtract(StartDT).TotalHours;


            if (PTF == null && Project!=null)
            {
                Project.HasUpdate = true;
            } else if (PTF!=null && Project == null)
            {
                PTF.HasUpdate = true;
                PTF.Project.HasUpdate = true;

            } else if (PTF!=null && PTF.Project != null && Project!=null)
            {
                PTF.HasUpdate = true;
                PTF.Project.HasUpdate = true;
            }


            if (!(Session is NestedUnitOfWork)
               && (Session.DataLayer != null)
                   && Session.IsNewObject(this)
                   && (Session.ObjectLayer is SimpleObjectLayer)
           //OR
           //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
           && string.IsNullOrEmpty(Code))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("POTI-{0:D8}", nextSequence);
            }

            base.OnSaving();
        }


        [ModelDefault("AllowEdit", "False")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }


        [RuleRequiredField("RuleRequiredField for Timesheet.Date", DefaultContexts.Save, "A date must be specified")]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        [RuleRequiredField("RuleRequiredField for Timesheet.Project", DefaultContexts.Save, "A project must be specified")]
        [Association("Project-Timesheets")]
        public Project Project
        {
            get => project;
            set => SetPropertyValue(nameof(Project), ref project, value);
        }

        [DataSourceProperty("Project.PTFs")]
        [Association("PTF-Timesheets")]
        public PTF PTF
        {
            get => pTF;
            set => SetPropertyValue(nameof(PTF), ref pTF, value);
        }

        [DataSourceProperty("Project.TimesheetTypes")]
        [Association("ProjectTimesheetType-Timesheets")]
        public ProjectTimesheetType ProjectTimesheetType
        {
            get => projectTimesheetType;
            set => SetPropertyValue(nameof(ProjectTimesheetType), ref projectTimesheetType, value);
        }

        [DataSourceProperty("Project.Locations")]
        [Association("ProjectLocation-Timesheets")]
        public ProjectLocation Location
        {
            get => location;
            set => SetPropertyValue(nameof(Location), ref location, value);
        }

        //[DataSourceCriteria("[IsActive] = True")]
        [RuleRequiredField("RuleRequiredField for Timesheet.Employee", DefaultContexts.Save, "An employee must be specified")]
        [DataSourceProperty("Project.Branch.Employees")]
        [Association("Employee-ProjectTimesheets")]
        public Employee Employee
        {
            get => employee;
            set => SetPropertyValue(nameof(Employee), ref employee, value);
        }

        int mealDuration;
        string displacementPriceCurrency;
        double displacementPrice;
        double kM;
        string mealPriceCurrency;
        int mealQuantity;
        double mealPrice;
        double minutes;
        double hours;

        ProjectTimesheetEndMinute eM;
        ProjectTimesheetEndHour eH;
        ProjectTimesheetStartMinute sM;
        ProjectTimesheetStartHour sH;
        DateTime endDT;
        DateTime startDT;
        string code;
        ProjectLocation location;
        ProjectTimesheetType projectTimesheetType;
        Employee employee;
        TimesheetFunction tF;



        [RuleRequiredField("RuleRequiredField for Timesheet.SH", DefaultContexts.Save, "A start hour must be specified")]
        [Association("ProjectTimesheetStartHour-ProjectTimesheets")]
        public ProjectTimesheetStartHour SH
        {
            get => sH;
            set => SetPropertyValue(nameof(SH), ref sH, value);
        }


        [RuleRequiredField("RuleRequiredField for Timesheet.SM", DefaultContexts.Save, "A start minute must be specified")]
        [Association("ProjectTimesheetStartMinute-ProjectTimesheets")]
        public ProjectTimesheetStartMinute SM
        {
            get => sM;
            set => SetPropertyValue(nameof(SM), ref sM, value);
        }


        [RuleRequiredField("RuleRequiredField for Timesheet.EH", DefaultContexts.Save, "An end hour must be specified")]
        [Association("ProjectTimesheetEndHour-ProjectTimesheets")]
        public ProjectTimesheetEndHour EH
        {
            get => eH;
            set => SetPropertyValue(nameof(EH), ref eH, value);
        }


        [RuleRequiredField("RuleRequiredField for Timesheet.EM", DefaultContexts.Save, "An end minute must be specified")]
        [Association("ProjectTimesheetEndMinute-ProjectTimesheets")]
        public ProjectTimesheetEndMinute EM
        {
            get => eM;
            set => SetPropertyValue(nameof(EM), ref eM, value);
        }

        [Browsable(false)]
        public int StartHour
        {
            get => startHour;
            set => SetPropertyValue(nameof(StartHour), ref startHour, value);
        }



        int startHour;
        int startMinute;
        int endMinute;
        int endHour;


        [Browsable(false)]

        public int StartMinute
        {
            get => startMinute;
            set => SetPropertyValue(nameof(StartMinute), ref startMinute, value);
        }


        [Browsable(false)]

        public int EndHour
        {
            get => endHour;
            set => SetPropertyValue(nameof(EndHour), ref endHour, value);
        }



        [Browsable(false)]
        public int EndMinute
        {
            get => endMinute;
            set => SetPropertyValue(nameof(EndMinute), ref endMinute, value);
        }

        Project project;
        PTF pTF;
        DateTime date;


        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public DateTime StartDT
        {
            get => startDT;
            set => SetPropertyValue(nameof(StartDT), ref startDT, value);
        }

        [ModelDefault("AllowEdit", "False"), ModelDefault("IsVisibleInListView", "False"), ModelDefault("IsVisibleInDetailView", "False")]
        public DateTime EndDT
        {
            get => endDT;
            set => SetPropertyValue(nameof(EndDT), ref endDT, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public double Hours
        {
            get => hours;
            set => SetPropertyValue(nameof(Hours), ref hours, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public double Minutes
        {
            get => minutes;
            set => SetPropertyValue(nameof(Minutes), ref minutes, value);
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


        public TimesheetFunction TF
        {
            get => tF;
            set => SetPropertyValue(nameof(TF), ref tF, value);
        }

        public double MealPrice
        {
            get => mealPrice;
            set => SetPropertyValue(nameof(MealPrice), ref mealPrice, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string MealPriceCurrency
        {
            get => mealPriceCurrency;
            set => SetPropertyValue(nameof(MealPriceCurrency), ref mealPriceCurrency, value);
        }

        public int MealQuantity
        {
            get => mealQuantity;
            set => SetPropertyValue(nameof(MealQuantity), ref mealQuantity, value);
        }

        
        public int MealDuration
        {
            get => mealDuration;
            set => SetPropertyValue(nameof(MealDuration), ref mealDuration, value);
        }

        public double KM
        {
            get => kM;
            set => SetPropertyValue(nameof(KM), ref kM, value);
        }


        public double DisplacementPrice
        {
            get => displacementPrice;
            set => SetPropertyValue(nameof(DisplacementPrice), ref displacementPrice, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string DisplacementPriceCurrency
        {
            get => displacementPriceCurrency;
            set => SetPropertyValue(nameof(DisplacementPriceCurrency), ref displacementPriceCurrency, value);
        }

        protected override void OnLoaded()
        {
            //Reset();
            base.OnLoaded();
        }
        private void Reset()
        {
            
        }
    }

    public enum TimesheetFunction
    {
        PTF,
        VTF
    }
}