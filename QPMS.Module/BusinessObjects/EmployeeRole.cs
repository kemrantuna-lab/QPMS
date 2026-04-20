using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace QPMS.Module.BusinessObjects
{
    [ImageName("BO_Role")]
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EmployeeRole : PermissionPolicyRoleBase,IPermissionPolicyRoleWithUsers
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public EmployeeRole(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        Company company;

        [Association("Company-EmployeeRoles")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [Association("Employee-EmployeeRoles")]
        public XPCollection<Employee> Employees
        {
            get
            {
                return GetCollection<Employee>(nameof(Employees));
            }
        }

        IEnumerable<IPermissionPolicyUser> IPermissionPolicyRoleWithUsers.Users
        {
            get
            {
                return Employees.OfType<IPermissionPolicyUser>();
            }
        }

        [Association("EmployeeRole-QPMSReports")]
        public XPCollection<QPMSReport> QPMSReports
        {
            get
            {
                return GetCollection<QPMSReport>(nameof(QPMSReports));
            }
        }

        [Association("EmployeeRole-QPMSAnalyses")]
        public XPCollection<QPMSAnalysis> QPMSAnalyses
        {
            get
            {
                return GetCollection<QPMSAnalysis>(nameof(QPMSAnalyses));
            }
        }

        [Association("EmployeeRole-QPMSDashboards")]
        public XPCollection<QPMSDashboard> QPMSDashboards
        {
            get
            {
                return GetCollection<QPMSDashboard>(nameof(QPMSDashboards));
            }
        }
    }
}