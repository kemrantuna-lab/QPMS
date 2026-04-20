using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;

namespace QPMS.Module.BusinessObjects
{
    public class EmployeeFormNonPaidDay : EmployeeForm
    {

        public EmployeeFormNonPaidDay(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }


        string supervisor;
        string text;
        [RuleRequiredField("RuleRequiredField for EmployeeForm.Text", DefaultContexts.Save, "Reason of day off must specified")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Text
        {
            get => text;
            set => SetPropertyValue(nameof(Text), ref text, value);
        }

        [RuleRequiredField("RuleRequiredField for EmployeeForm.Supervisor", DefaultContexts.Save, "Supervisor of employee must be specified.")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Supervisor
        {
            get => supervisor;
            set => SetPropertyValue(nameof(Supervisor), ref supervisor, value);
        }

    }

}