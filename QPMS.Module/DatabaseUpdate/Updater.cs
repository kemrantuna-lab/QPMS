using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.XtraSpreadsheet.Model;
using QPMS.Module.BusinessObjects;
using System;
using System.Linq;

namespace QPMS.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

            Employee admin3 = ObjectSpace.FirstOrDefault<Employee>(e=>e.UserName =="Admin3");
            if(admin3 == null)
            {
                admin3 = ObjectSpace.CreateObject<Employee>();
                admin3.IsActive = true;
                admin3.ChangePasswordOnFirstLogon = false;
                admin3.UserName = "Admin3";
                admin3.FirstName = "Admin3";
                admin3.LastName = "Admin3";
                admin3.SetPassword("AzraAa.963852741");

                ObjectSpace.CommitChanges();
            }

            EmployeeRole admin3Role = ObjectSpace.FirstOrDefault<EmployeeRole>(er => er.Name == "AdminRole3");
            if (admin3Role == null) {

                admin3Role = ObjectSpace.CreateObject<EmployeeRole>();
                admin3Role.Name = "AdminRole3";
                admin3Role.IsAdministrative = true;

                ObjectSpace.CommitChanges();
            }

            admin3 = ObjectSpace.FirstOrDefault<Employee>(e => e.UserName == "Admin3");
            admin3Role = ObjectSpace.FirstOrDefault<EmployeeRole>(er => er.Name == "AdminRole3");

            if(admin3!=null && admin3Role != null)
            {
                admin3.EmployeeRoles.Add(admin3Role);
                admin3Role.Employees.Add(admin3);
                ObjectSpace.CommitChanges();
            }


            EmployeeRole defaultEmployeeRole = CreateEmployeeDefaultRole();
            //EmployeeRole defaultEmployeeAdminRole = CreateEmployeeAdminRole();

            Employee sampleEmployee = ObjectSpace.FirstOrDefault<Employee>(u => u.UserName == "UserTemp");
            if (sampleEmployee == null)
            {
                sampleEmployee = ObjectSpace.CreateObject<Employee>();
                sampleEmployee.UserName = "UserTemp";
                // Set a password if the standard authentication type is used
                sampleEmployee.SetPassword(string.Empty);

                // The UserLoginInfo object requires a user object Id (Oid).
                // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
                ObjectSpace.CommitChanges(); //This line persists created object(s).
                //((ISecurityUserWithLoginInfo)sampleEmployee).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(sampleEmployee));
            } else
            {
                sampleEmployee.IsActive = true;
                sampleEmployee.ChangePasswordOnFirstLogon = false;
            }

            sampleEmployee.EmployeeRoles.Add(defaultEmployeeRole);

            var adminList = ObjectSpace.GetObjects<Employee>().Where(p=>p.UserName == "SuperAdmin").ToList();

            //if (adminList.Count > 1)
            //{
            //    foreach(var adminu in adminList)
            //    {
            //        adminList.Remove(adminu);
            //        ObjectSpace.CommitChanges();
            //    }
            //}

           Employee employeeSuperAdmin = ObjectSpace.FirstOrDefault<Employee>(u => u.UserName == "SuperAdmin");
            if (employeeSuperAdmin == null)
            {
                employeeSuperAdmin = ObjectSpace.CreateObject<Employee>();
                employeeSuperAdmin.UserName = "SuperAdmin";
                // Set a password if the standard authentication type is used
                employeeSuperAdmin.SetPassword(string.Empty);
                employeeSuperAdmin.IsActive = true;
                //employeeSuperAdmin.IsActive = true;
                employeeSuperAdmin.ChangePasswordOnFirstLogon = false;
                // The UserLoginInfo object requires a user object Id (Oid).
                // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
                employeeSuperAdmin.SetPassword("AzraAa.963852741");
                ObjectSpace.CommitChanges(); //This line persists created object(s).
                //((ISecurityUserWithLoginInfo)employeeSuperAdmin).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(employeeSuperAdmin));
            } 



            //else
            //{
            //    employeeSuperAdmin.SetPassword("AzraAa.963852741");
            //    ObjectSpace.CommitChanges();
            //}

            ObjectSpace.CommitChanges();
            // If a role with the Administrators name doesn't exist in the database, create this role
            EmployeeRole adminRole = ObjectSpace.FirstOrDefault<EmployeeRole>(r => r.Name == "Administrators");
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<EmployeeRole>();
                adminRole.Name = "Administrators";
                adminRole.IsAdministrative = true;
                employeeSuperAdmin.EmployeeRoles.Add(adminRole);
                adminRole.Employees.Add(employeeSuperAdmin);
            } 

           
            ObjectSpace.CommitChanges();


            Employee adminEmp = ObjectSpace.FindObject<Employee>(
                    new BinaryOperator("UserName","SuperAdmin")
                );

            if(adminEmp!=null && adminEmp.EmployeeRoles.Count == 0)
            {
                EmployeeRole adminRoleSearch = ObjectSpace.FirstOrDefault<EmployeeRole>(r => r.Name == "Administrators");

                if (adminRoleSearch != null)
                {
                    adminRoleSearch.Employees.Add(adminEmp);
                    adminEmp.EmployeeRoles.Add(adminRoleSearch);
                }

                ObjectSpace.CommitChanges();
            }


            Employee userAdmin2 = ObjectSpace.FirstOrDefault<Employee>(u => u.UserName == "Admin2");
            if (userAdmin2 == null)
            {
                userAdmin2 = ObjectSpace.CreateObject<Employee>();
                // Set a password if the standard authentication type is used
                userAdmin2.SetPassword("AzraAa.963852741Aa.");

                // The UserLoginInfo object requires a user object Id (Oid).
                // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
                ObjectSpace.CommitChanges(); //This line persists created object(s).
                //((ISecurityUserWithLoginInfo)userAdmin2).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(userAdmin2));
            }
            // If a role with the Administrators name doesn't exist in the database, create this role
            EmployeeRole adminRole2 = ObjectSpace.FirstOrDefault<EmployeeRole>(r => r.Name == "Administrators");
            if (adminRole2 == null)
            {
                adminRole2 = ObjectSpace.CreateObject<EmployeeRole>();
                adminRole2.Name = "Administrators";
            }
            adminRole2.IsAdministrative = true;
            userAdmin2.EmployeeRoles.Add(adminRole);
            ObjectSpace.CommitChanges(); //This line persists created object(s).

            CreateDefaultCustomerUsersAndCustomerUserRoles();

        }

        private void CreateDefaultCustomerUsersAndCustomerUserRoles()
        {
            CustomerUser customerUserAdmin = ObjectSpace.FirstOrDefault<CustomerUser>(u => u.UserName == "Admin");
            if (customerUserAdmin == null)
            {
                customerUserAdmin= ObjectSpace.CreateObject<CustomerUser>();
                customerUserAdmin.UserName = "Admin";
                customerUserAdmin.FirstName = "Admin";
                customerUserAdmin.LastName = "Admin";
                
                customerUserAdmin.IsActive= true;
                customerUserAdmin.SetPassword("AzraAa.963852741Aa.");

                ObjectSpace.CommitChanges();
            }


            CustomerUser customerUserNormalUser = ObjectSpace.FirstOrDefault<CustomerUser>(a=>a.UserName == "User");
            if (customerUserNormalUser == null)
            {
                customerUserNormalUser= ObjectSpace.CreateObject<CustomerUser>();
                customerUserNormalUser.UserName = "User";
                customerUserNormalUser.FirstName = "User";
                customerUserNormalUser.LastName = "User";
                customerUserNormalUser.IsActive= true;

                customerUserNormalUser.SetPassword("AzraAa.963852741");
                ObjectSpace.CommitChanges();
            }

            CustomerUserRole customerUserRoleAdminRole = ObjectSpace.FirstOrDefault<CustomerUserRole>(r => r.Name == "Administrators");
            if (customerUserRoleAdminRole == null)
            {
                customerUserRoleAdminRole= ObjectSpace.CreateObject<CustomerUserRole>();
                customerUserRoleAdminRole.Name = "Administrators";
                customerUserRoleAdminRole.IsAdministrative= true;

                ObjectSpace.CommitChanges();
            }
            customerUserRoleAdminRole.IsAdministrative = true;
            customerUserRoleAdminRole.CustomerUsers.Add(customerUserAdmin);
            customerUserAdmin.CustomerUserRoles.Add(customerUserRoleAdminRole);

            ObjectSpace.CommitChanges();


        }

        private EmployeeRole CreateEmployeeDefaultRole()
        {
            EmployeeRole role = ObjectSpace.FirstOrDefault<EmployeeRole>(r => r.Name == "Default");
            if(role == null) 
            {
                role = ObjectSpace.CreateObject<EmployeeRole>();
                role.Name = "Default";

                role.AddObjectPermissionFromLambda<Employee>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                role.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                role.AddMemberPermissionFromLambda<Employee>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                role.AddMemberPermissionFromLambda<Employee>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                role.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                role.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                role.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                role.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                role.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }
            return role;
        }

        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
        private EmployeeRole CreateBranchResponsibleRole()
        {
            EmployeeRole branchResponsibleRole = ObjectSpace.FirstOrDefault<EmployeeRole>(role => role.Name == "Branch Responsible");

            if (branchResponsibleRole == null)
            {
                branchResponsibleRole = ObjectSpace.CreateObject<EmployeeRole>();
                branchResponsibleRole.Name = "Branch Responsible";

                branchResponsibleRole.AddObjectPermissionFromLambda<Employee>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                branchResponsibleRole.AddNavigationPermission(@"Application/NavigationItems/Items/BranchResponsible/Items", SecurityPermissionState.Allow);

                branchResponsibleRole.AddMemberPermissionFromLambda<Employee>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);


                branchResponsibleRole.AddTypePermissionsRecursively<EmployeeRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                branchResponsibleRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                branchResponsibleRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                branchResponsibleRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                branchResponsibleRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                branchResponsibleRole.AddTypePermissionsRecursively<Branch>(SecurityOperations.Read, SecurityPermissionState.Deny);
                branchResponsibleRole.AddObjectPermission<Branch>(SecurityOperations.Read, "[Employee][[Oid] = CurrentUserId()]", SecurityPermissionState.Allow);

                branchResponsibleRole.AddTypePermissionsRecursively<BTF>(SecurityOperations.Read, SecurityPermissionState.Allow);
                branchResponsibleRole.AddTypePermissionsRecursively<BranchTimesheet>(SecurityOperations.Read, SecurityPermissionState.Deny);
                branchResponsibleRole.AddTypePermissionsRecursively<Project>(SecurityOperations.Read, SecurityPermissionState.Allow);
                branchResponsibleRole.AddTypePermissionsRecursively<PTF>(SecurityOperations.Read, SecurityPermissionState.Deny);
                branchResponsibleRole.AddTypePermissionsRecursively<ProjectTimesheet>(SecurityOperations.Read, SecurityPermissionState.Allow);
                branchResponsibleRole.AddTypePermissionsRecursively<Part>(SecurityOperations.Read, SecurityPermissionState.Deny);
                branchResponsibleRole.AddTypePermissionsRecursively<FailureMode>(SecurityOperations.Read, SecurityPermissionState.Allow);
                branchResponsibleRole.AddTypePermissionsRecursively<Failure>(SecurityOperations.Read, SecurityPermissionState.Deny);
                branchResponsibleRole.AddTypePermissionsRecursively<Traceability>(SecurityOperations.Read, SecurityPermissionState.Allow);

            }

            return branchResponsibleRole;
        }

    }
}
