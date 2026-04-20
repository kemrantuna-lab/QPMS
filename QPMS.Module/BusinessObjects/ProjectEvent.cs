using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;

namespace QPMS.Module.BusinessObjects
{
    public class ProjectEvent : Event
    {
        public ProjectEvent(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }


        Project project;

        [Association("Project-Events")]
        public Project Project
        {
            get => project;
            set => SetPropertyValue(nameof(Project), ref project, value);
        }
    }

}