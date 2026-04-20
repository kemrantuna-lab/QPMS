using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.Security;
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
    [DefaultProperty(nameof(UserName))]
    public class CustomerUser : Person, ISecurityUser, IAuthenticationStandardUser, IAuthenticationActiveDirectoryUser, ISecurityUserWithRoles, IPermissionPolicyUser, ICanInitialize
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public CustomerUser(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }



        string storedPassword;
        bool changePasswordOnFirstLogon;
        bool isActive;
        string userName;
        string code;


        [ModelDefault("AllowEdit", "False")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }


        [RuleRequiredField("CustomerUserUserNameRequired", DefaultContexts.Save)]
        [RuleUniqueValue("CustomerUserNameIsUnique", DefaultContexts.Save, "The login already exists in the system. Please try another username for the customer user")]
        public string UserName
        {
            get => userName;
            set => SetPropertyValue(nameof(UserName), ref userName, value);
        }


        public bool IsActive
        {
            get => isActive;
            set => SetPropertyValue(nameof(IsActive), ref isActive, value);
        }

        [Browsable(false), Size(SizeAttribute.Unlimited), Persistent, SecurityBrowsable]
        public string StoredPassword
        {
            get => storedPassword;
            set => SetPropertyValue(nameof(StoredPassword), ref storedPassword, value);
        }

        public bool ComparePassword(string password)
        {
            return PasswordCryptographer.VerifyHashedPasswordDelegate(this.storedPassword, password);
        }

        public void SetPassword(string password)
        {
            this.storedPassword = PasswordCryptographer.HashPasswordDelegate(password);
            OnChanged(nameof(StoredPassword));
        }

        [Association("CustomerUserRole-CustomerUsers")]
        public XPCollection<CustomerUserRole> CustomerUserRoles
        {
            get
            {
                return GetCollection<CustomerUserRole>(nameof(CustomerUserRoles));
            }
        }

        IList<ISecurityRole> ISecurityUserWithRoles.Roles
        {
            get
            {
                IList<ISecurityRole> result = new List<ISecurityRole>();
                foreach (CustomerUserRole role in CustomerUserRoles)
                {
                    result.Add(role);
                }

                return result;
            }
        }

        IEnumerable<IPermissionPolicyRole> IPermissionPolicyUser.Roles
        {
            get
            {
                return CustomerUserRoles.OfType<CustomerUserRole>();
            }

        }

        public bool ChangePasswordOnFirstLogon
        {
            get => changePasswordOnFirstLogon;
            set => SetPropertyValue(nameof(ChangePasswordOnFirstLogon), ref changePasswordOnFirstLogon, value);
        }

        void ICanInitialize.Initialize(IObjectSpace objectSpace, SecurityStrategyComplex security)
        {
            CustomerUserRole newUserRole = objectSpace.FirstOrDefault<CustomerUserRole>(role => role.Name == security.NewUserRoleName);
            if (newUserRole == null)
            {
                newUserRole = objectSpace.CreateObject<CustomerUserRole>();
                newUserRole.Name = security.NewUserRoleName;
                newUserRole.IsAdministrative = true;
                newUserRole.CustomerUsers.Add(this);
            }
        }

        [Association("CustomerUser-AssignedProjects")]
        public XPCollection<Project> AssignedProjects
        {
            get
            {
                return GetCollection<Project>(nameof(AssignedProjects));
            }
        }

    }
}