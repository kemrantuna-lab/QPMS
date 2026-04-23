using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using QPMS.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace QPMS.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ViewControllerCompany : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public ViewControllerCompany()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void simpleActionCreateDefaultRoles_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var currentCompany = View.CurrentObject as Company;

            if (currentCompany != null)
            {
                try
                {
                    Company com = ObjectSpace.FirstOrDefault<Company>(r => r.Oid == currentCompany.Oid);

                    if (com != null)
                    {
                        /**
                         * Create Company Admin
                         * **/
                        EmployeeRole companyAdmin = ObjectSpace.FirstOrDefault<EmployeeRole>(r => r.Company.Oid == com.Oid && r.Name == (com.Name + " " + "Company Admin"));

                        if (companyAdmin == null)
                        {
                            companyAdmin = ObjectSpace.CreateObject<EmployeeRole>();
                            companyAdmin.Name = com.Name + " " + "Company Admin";
                            companyAdmin.CanEditModel = false;
                            companyAdmin.IsAdministrative = false;

                            companyAdmin.Company = com;
                            com.EmployeeRoles.Add(companyAdmin);
                        }

                        ObjectSpace.CommitChanges();


                        /**
                         * Create HR Role
                         * **/
                        EmployeeRole companyHR = ObjectSpace.FirstOrDefault<EmployeeRole>(r => r.Company.Oid == com.Oid && r.Name == (com.Name + " " + "Company HR"));

                        if (companyHR == null)
                        {
                            companyHR = ObjectSpace.CreateObject<EmployeeRole>();
                            companyHR.Name = com.Name + " " + "Company HR";
                            companyHR.CanEditModel = false;
                            companyHR.IsAdministrative = false;

                            companyHR.Company = com;
                            com.EmployeeRoles.Add(companyHR);

                            AddCompanyRoleTypePermissions(com, companyHR);

                        }

                        ObjectSpace.CommitChanges();


                        EmployeeRole companyFinance = ObjectSpace.FirstOrDefault<EmployeeRole>(r => r.Company.Oid == com.Oid && r.Name == (com.Name + " " + "Company Finance"));

                        if (companyFinance == null)
                        {
                            companyFinance = ObjectSpace.CreateObject<EmployeeRole>();
                            companyFinance.Name = com.Name + " " + "Company Finance";
                            companyFinance.CanEditModel = false;
                            companyFinance.IsAdministrative = false;

                            companyFinance.Company = com;
                            com.EmployeeRoles.Add(companyFinance);
                        }

                        ObjectSpace.CommitChanges();


                        EmployeeRole companyAccounting = ObjectSpace.FirstOrDefault<EmployeeRole>(r => r.Company.Oid == com.Oid && r.Name == (com.Name + " " + "Company Accounting"));

                        if (companyAccounting == null)
                        {
                            companyAccounting = ObjectSpace.CreateObject<EmployeeRole>();
                            companyAccounting.Name = com.Name + " " + "Company Accounting";
                            companyAccounting.CanEditModel = false;
                            companyAccounting.IsAdministrative = false;

                            companyAccounting.Company = com;
                            com.EmployeeRoles.Add(companyAccounting);
                        }

                        ObjectSpace.CommitChanges();

                        EmployeeRole companyProjectResponsible = ObjectSpace.FirstOrDefault<EmployeeRole>(r => r.Company.Oid == com.Oid && r.Name == (com.Name + " " + "Company Project Responsible"));

                        if (companyProjectResponsible == null)
                        {
                            companyProjectResponsible = ObjectSpace.CreateObject<EmployeeRole>();
                            companyProjectResponsible.Name = com.Name + " " + "Company Project Responsible";
                            companyProjectResponsible.CanEditModel = false;
                            companyProjectResponsible.IsAdministrative = false;

                            companyProjectResponsible.Company = com;
                            com.EmployeeRoles.Add(companyProjectResponsible);
                        }
                        ObjectSpace.CommitChanges();

                        EmployeeRole companyBranchResponsible = ObjectSpace.FirstOrDefault<EmployeeRole>(r => r.Company.Oid == com.Oid && r.Name == (com.Name + " " + "Company Branch Responsible"));

                        if (companyBranchResponsible == null)
                        {
                            companyBranchResponsible = ObjectSpace.CreateObject<EmployeeRole>();
                            companyBranchResponsible.Name = com.Name + " " + "Company Branch Responsible";
                            companyBranchResponsible.CanEditModel = false;
                            companyBranchResponsible.IsAdministrative = false;

                            companyBranchResponsible.Company = com;
                            com.EmployeeRoles.Add(companyBranchResponsible);

                            AddCompanyBranchResponsibleRolePermissions(com, companyBranchResponsible);
                        }
                        ObjectSpace.CommitChanges();


                        EmployeeRole companyBranchReporting = ObjectSpace.FirstOrDefault<EmployeeRole>(r=>r.Company.Oid == com.Oid && r.Name == (com.Name + " " + "Branch Reporting"));

                        if(companyBranchReporting == null)
                        {
                            companyBranchReporting = ObjectSpace.CreateObject<EmployeeRole>(); ;
                            companyBranchReporting.Name = com.Name + " " + "Branch Reporting";
                            companyBranchReporting.CanEditModel = false;
                            companyBranchReporting.IsAdministrative = false;
                            companyBranchReporting.Company = com;

                            com.EmployeeRoles.Add(companyBranchReporting);

                            AddCompanyBranchReportingRoleTypePermissions(com,companyBranchReporting);
                        }

                        ObjectSpace.CommitChanges();

                        EmployeeRole companyEmployeeRole = ObjectSpace.FirstOrDefault<EmployeeRole>(r => r.Company.Oid == com.Oid && r.Name == (com.Name + " " + "Employee"));

                        if(companyEmployeeRole == null)
                        {
                            companyEmployeeRole = ObjectSpace.CreateObject<EmployeeRole>();
                            companyEmployeeRole.Name = com.Name + " " + "Employee";
                            companyEmployeeRole.CanEditModel = false;
                            companyEmployeeRole.IsAdministrative = false;

                            companyEmployeeRole.Company = com;
                            com.EmployeeRoles.Add(companyEmployeeRole);

                            AddCompanyEmployeeRoleTypePermissions(com,companyEmployeeRole);
                        }

                        ObjectSpace.CommitChanges();


                       
                    }
                }
                catch
                {

                }
            }
        }

        private void AddCompanyBranchResponsibleRolePermissions(Company com, EmployeeRole companyBranchResponsible)
        {
            try
            {
                companyBranchResponsible.AddObjectPermissionFromLambda<Employee>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                companyBranchResponsible.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);

                companyBranchResponsible.AddMemberPermissionFromLambda<Employee>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                companyBranchResponsible.AddMemberPermissionFromLambda<Employee>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);

                companyBranchResponsible.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchResponsible.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                companyBranchResponsible.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                companyBranchResponsible.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchResponsible.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                AddCompanyBranchReportingRoleTypePermissions(com, companyBranchResponsible);
            } catch
            {

            }
        }

        private void AddCompanyBranchReportingRoleTypePermissions(Company com, EmployeeRole companyBranchReporting)
        {
            try
            {
                companyBranchReporting.AddObjectPermissionFromLambda<Employee>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                companyBranchReporting.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                companyBranchReporting.AddNavigationPermission(@"Application/NavigationItems/Items/CompanyBranchReporting", SecurityPermissionState.Allow);


                companyBranchReporting.AddMemberPermissionFromLambda<Employee>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                companyBranchReporting.AddMemberPermissionFromLambda<Employee>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);

                companyBranchReporting.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                /** ADDRESS START  **/
                companyBranchReporting.AddTypePermissionsRecursively<Address>(SecurityOperations.Read,null);
                companyBranchReporting.AddTypePermissionsRecursively<Address>(SecurityOperations.Write, null);
                companyBranchReporting.AddTypePermissionsRecursively<Address>(SecurityOperations.Create, null);
                companyBranchReporting.AddTypePermissionsRecursively<Address>(SecurityOperations.Delete, null);
                /** ADDRESS END    **/

                /***MEDIA DATA OBJECT start**/
                companyBranchReporting.AddTypePermissionsRecursively<MediaDataObject>(SecurityOperations.Read, null);
                companyBranchReporting.AddTypePermissionsRecursively<MediaDataObject>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<MediaDataObject>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<MediaDataObject>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                /***MEDIA DATA OBJECT end**/

                /***PARTY start**/
                companyBranchReporting.AddTypePermissionsRecursively<Party>(SecurityOperations.Read, null);
                companyBranchReporting.AddTypePermissionsRecursively<Party>(SecurityOperations.Write, null);
                companyBranchReporting.AddTypePermissionsRecursively<Party>(SecurityOperations.Create, null);
                companyBranchReporting.AddTypePermissionsRecursively<Party>(SecurityOperations.Delete, null);
                /***PARTY end**/

                /** BRANCH START  **/
                companyBranchReporting.AddTypePermissionsRecursively<Branch>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<Branch>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<Branch>(SecurityOperations.Create, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<Branch>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<Branch>(SecurityOperations.ReadWriteAccess,
                    "BDA; Branch Timesheets; BTFs; " +
                    "Code; " +
                    "Created By; Created On; Has Update; " +
                    "Interviews; Locations; Main; Oid; " +
                    "Projects; PTFs; " +
                    "Updated By; Updated On; Version", "[Employees][[Oid] Equals CurrentUserId()]", SecurityPermissionState.Allow);


                companyBranchReporting.AddMemberPermission<Branch>(SecurityOperations.Read,
                   "Branch Manager; Branch Responsible; " +
                   "Code; Company; Cost Center; " +
                   "Employees; KAM; Name; " +
                   "Photo; Sub Branches", "[Employees][[Oid] Equals CurrentUserId()]", SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<Branch>(SecurityOperations.Write,
                   "Branch Manager; Branch Responsible; " +
                   "Code; Company; Cost Center; " +
                   "Employees; KAM; Name; " +
                   "Photo; Sub Branches", "[Employees][[Oid] Equals CurrentUserId()]", SecurityPermissionState.Deny);
                /** BRANCH END    **/


                /***BRANCH LOCATION START**/
                companyBranchReporting.AddTypePermissionsRecursively<BranchLocation>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<BranchLocation>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<BranchLocation>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<BranchLocation>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<BranchLocation>(SecurityOperations.ReadWriteAccess,
                    "Branch; Code; Created By; Created On; Has Update; Name; Oid; Photo; Updated By; Updated On; Version", 
                    "[Branch.Employees][[Oid] Equals CurrentUserId()]", 
                    SecurityPermissionState.Allow);

                /***BRANCH LOCATION END**/

                /**BRANCH TIMESHEET START**/
                companyBranchReporting.AddTypePermissionsRecursively<BranchTimesheet>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<BranchTimesheet>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<BranchTimesheet>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<BranchTimesheet>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<BranchTimesheet>(SecurityOperations.ReadWriteAccess,
                    "Branch; BTF; " +
                    "Code; Created By; Created On; " +
                    "Date; " +
                    "Employee; End Hour; End Minute; " +
                    "Has Update; Oid; Presence; " +
                    "Start Hour; Start Minute; " +
                    "Updated By; Updated On; Version", 
                    "[BTF.Branch.Employees][[Oid] Equals CurrentUserId()]", 
                    SecurityPermissionState.Allow);
                /**BRANCH TIMESHEET END**/


                /**BRANCH TIMESHEET START HOUR --- START **/
                companyBranchReporting.AddTypePermission<BranchTimesheetStartHour>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<BranchTimesheetStartHour>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermission<BranchTimesheetStartHour>(SecurityOperations.Create, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<BranchTimesheetStartHour>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<BranchTimesheetStartHour>(
                        SecurityOperations.ReadWriteAccess,
                        "Branch Timesheets;",
                        "[Company.Employees][[Oid] = CurrentUserId()]",
                        SecurityPermissionState.Allow
                    );

                companyBranchReporting.AddMemberPermission<BranchTimesheetStartHour>(
                    SecurityOperations.Read,
                    "Company; Name; Oid; Start Hour; Val",
                    "[Company.Employees][[Oid] = CurrentUserId()]",
                    SecurityPermissionState.Allow
                    );

                companyBranchReporting.AddMemberPermission<BranchTimesheetStartHour>(
                SecurityOperations.Write,
                "Company; Name; Oid; Start Hour; Val",
                "[Company.Employees][[Oid] = CurrentUserId()]",
                SecurityPermissionState.Deny
                );

                /**BRANCH TIMESHEET START HOUR --- END   **/

                /**BRANCH TIMESHEET START MINUTE --- START **/
                companyBranchReporting.AddTypePermission<BranchTimesheetStartMinute>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<BranchTimesheetStartMinute>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermission<BranchTimesheetStartMinute>(SecurityOperations.Create, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<BranchTimesheetStartMinute>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<BranchTimesheetStartMinute>(
                        SecurityOperations.ReadWriteAccess,
                        "Branch Timesheets;",
                        "[Company.Employees][[Oid] = CurrentUserId()]",
                        SecurityPermissionState.Allow
                    );

                companyBranchReporting.AddMemberPermission<BranchTimesheetStartMinute>(
                    SecurityOperations.Read,
                    "Company; Name; Oid; Start Hour; Val",
                    "[Company.Employees][[Oid] = CurrentUserId()]",
                    SecurityPermissionState.Allow
                    );

                companyBranchReporting.AddMemberPermission<BranchTimesheetStartMinute>(
                SecurityOperations.Write,
                "Company; Name; Oid; Start Hour; Val",
                "[Company.Employees][[Oid] = CurrentUserId()]",
                SecurityPermissionState.Deny
                );

                /**BRANCH TIMESHEET START MINUTE --- END   **/

                /**BRANCH TIMESHEET END HOUR --- START **/
                companyBranchReporting.AddTypePermission<BranchTimesheetEndHour>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<BranchTimesheetEndHour>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermission<BranchTimesheetEndHour>(SecurityOperations.Create, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<BranchTimesheetEndHour>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<BranchTimesheetEndHour>(
                        SecurityOperations.ReadWriteAccess,
                        "Branch Timesheets;",
                        "[Company.Employees][[Oid] = CurrentUserId()]",
                        SecurityPermissionState.Allow
                    );

                companyBranchReporting.AddMemberPermission<BranchTimesheetEndHour>(
                    SecurityOperations.Read,
                    "Company; Name; Oid; Start Hour; Val",
                    "[Company.Employees][[Oid] = CurrentUserId()]",
                    SecurityPermissionState.Allow
                    );

                companyBranchReporting.AddMemberPermission<BranchTimesheetEndHour>(
                SecurityOperations.Write,
                "Company; Name; Oid; Start Hour; Val",
                "[Company.Employees][[Oid] = CurrentUserId()]",
                SecurityPermissionState.Deny
                );

                /**BRANCH TIMESHEET END HOUR --- END   **/

                /**BRANCH TIMESHEET END MINUTE --- START **/
                companyBranchReporting.AddTypePermission<BranchTimesheetEndMinute>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<BranchTimesheetEndMinute>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermission<BranchTimesheetEndMinute>(SecurityOperations.Create, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<BranchTimesheetEndMinute>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<BranchTimesheetEndMinute>(
                        SecurityOperations.ReadWriteAccess,
                        "Branch Timesheets;",
                        "[Company.Employees][[Oid] = CurrentUserId()]",
                        SecurityPermissionState.Allow
                    );

                companyBranchReporting.AddMemberPermission<BranchTimesheetEndMinute>(
                    SecurityOperations.Read,
                    "Company; Name; Oid; Start Hour; Val",
                    "[Company.Employees][[Oid] = CurrentUserId()]",
                    SecurityPermissionState.Allow
                    );

                companyBranchReporting.AddMemberPermission<BranchTimesheetEndMinute>(
                SecurityOperations.Write,
                "Company; Name; Oid; Start Hour; Val",
                "[Company.Employees][[Oid] = CurrentUserId()]",
                SecurityPermissionState.Deny
                );

                /**BRANCH TIMESHEET END HOUR --- END   **/




                /***BRANCH TRACKING FORM START**/
                companyBranchReporting.AddTypePermissionsRecursively<BTF>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<BTF>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<BTF>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<BTF>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<BTF>(SecurityOperations.ReadWriteAccess,
                    "Branch; Branch Timesheets; " +
                    "Code; " +
                    "Created By; Created On; " +
                    "Date; " +
                    "Has Update; " +
                    "Name; Oid; " +
                    "Photo; " +
                    "Updated By; Updated On; Version",
                    "[Branch.Employees][[Oid] Equals CurrentUserId()]",
                    SecurityPermissionState.Allow);
                /***BRANCH TRACKING FORM END**/

                /***COMPANY START   ****/
                companyBranchReporting.AddTypePermission<Company>(SecurityOperations.Read,SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<Company>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermission<Company>(SecurityOperations.Create, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<Company>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<Company>(
                    SecurityOperations.Read,
                    "Name;" +
                    "Branch Timesheet Start Hours;BranchTimesheetStartMinutes;BranchTimesheetEndHours; BranchTimesheetEndMinutes;" +
                    "ProjectTimesheetStartHours;ProjectTimesheetStartMinutes;ProjectTimesheetEndHours;ProjectTimesheetEndMinutes",
                    "[Employees][[Oid] = CurrentUserId()]",
                    SecurityPermissionState.Allow
                    ) ;
                /***COMPANY END   ****/

                /***EMPLOYEE START ***/
                companyBranchReporting.AddTypePermission<Employee>(SecurityOperations.Read,SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<Employee>(SecurityOperations.Write, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<Employee>(SecurityOperations.Create, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermission<Employee>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<Employee>(
                    SecurityOperations.Read,
                    "Branches",
                    "[Branches][[Company.Employees][[Oid] = CurrentUserId()]]",
                    SecurityPermissionState.Allow
                    );

                companyBranchReporting.AddMemberPermission<Employee>(
                    SecurityOperations.Write,
                    "Branches",
                    "[Branches][[Company.Employees][[Oid] = CurrentUserId()]]",
                    SecurityPermissionState.Deny
                    );

                companyBranchReporting.AddMemberPermission<Employee>(
                    SecurityOperations.ReadWriteAccess,
                    "Branch Timesheets",
                    "[Branches][[Company.Employees][[Oid] = CurrentUserId()]]",
                    SecurityPermissionState.Allow
                    );

                companyBranchReporting.AddMemberPermission<Employee>(
                    SecurityOperations.Read,
                    "Display Name; First Name; Last Name; User Name; Full Name; Is Active",
                    "[Branches][[Company.Employees][[Oid] = CurrentUserId()]]",
                    SecurityPermissionState.Allow
                    );

                companyBranchReporting.AddMemberPermission<Employee>(
                    SecurityOperations.Write,
                    "Display Name; First Name; Last Name; User Name; Full Name; Is Active",
                    "[Branches][[Company.Employees][[Oid] = CurrentUserId()]]",
                    SecurityPermissionState.Deny
                    );

                companyBranchReporting.AddMemberPermission<Employee>(
                    SecurityOperations.ReadWriteAccess,
                    "Project Timesheets",
                    "[Branches][[Company.Employees][[Oid] = CurrentUserId()]]",
                    SecurityPermissionState.Allow
                    );

                /***EMPLOYEE END **********/


                /***FAILURE MODE START**/
                companyBranchReporting.AddTypePermissionsRecursively<FailureMode>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<FailureMode>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<FailureMode>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<FailureMode>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<FailureMode>(SecurityOperations.ReadWriteAccess,
                    "Code; Created By; Created On; Description; Failures; Has Update; Name; Oid; Photo; Project; Updated By; Updated On; Version",
                    "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                    SecurityPermissionState.Allow);
                /***FAILURE MODE END****/

                /****PART START****/
                companyBranchReporting.AddTypePermissionsRecursively<Part>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<Part>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<Part>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<Part>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<Part>(SecurityOperations.ReadWriteAccess,
                    "Code; Created By; Created On; Currency; " +
                    "Has Update; Main; Name; " +
                    "OEMCode; OEMName; Oid; " +
                    "Photo; Price; Project; " +
                    "Sub Parts; Supplier Code; Supplier Name; " +
                    "Traceabilities; " +
                    "Updated By; Updated On; Version",
                    "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                    SecurityPermissionState.Allow);
                /******PART END*******/

                /******PROJECT START******/
                companyBranchReporting.AddTypePermissionsRecursively<Project>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<Project>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<Project>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<Project>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<Project>(SecurityOperations.ReadWriteAccess,
                    "Branch; " +
                    "Code; Company; " +
                    "Created By; Created On; " +
                    "Customer; Docs; Documentation; " +
                    "End; Expenses; " +
                    "Failure Modes; Failures; Files; " +
                    "Has Update; " +
                    "Invoices; Locations; Main; Members; " +
                    "Name; " +
                    "NOK; " +
                    "Notes; Oid; " +
                    "OK; " +
                    "Parts; PDA; Photo; PTFs; Risks; " +
                    "RNOK; ROK; " +
                    "Start; " +
                    "State; " +
                    "Sub Projects; " +
                    "TFailure Modes; TFailures; " +
                    "Timesheet Types; Timesheets; " +
                    "TInvoices; " +
                    "TOTAL; " +
                    "TParts; TPTF; " +
                    "Traceabilities; " +
                    "TTimesheets; TTraces; " +
                    "Updated By; Updated On; " +
                    "Users; " +
                    "Version; " +
                    "Work Order",
                    "[Branch.Employees][[Oid] Equals CurrentUserId()]",
                    SecurityPermissionState.Allow);
                /******PROJECT END*******/

                /******PROJECT LOCATION START**/
                companyBranchReporting.AddTypePermissionsRecursively<ProjectDocument>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectDocument>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectDocument>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectDocument>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<ProjectDocument>(SecurityOperations.ReadWriteAccess,
                    "Code; Created By; Created On; Has Update; Oid; Photo; Project; Subject; Text; Updated By; Updated On; Version",
                    "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                    SecurityPermissionState.Allow);
                /******PROJECT LOCATION END****/


                /*****PROJECT EXCEL START**/
                companyBranchReporting.AddTypePermissionsRecursively<ProjectExcel>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectExcel>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectExcel>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectExcel>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<ProjectDocument>(SecurityOperations.ReadWriteAccess,
                    "Created By; Created On; Data; Has Update; Oid; Photo; Project; Subject; Updated By; Updated On; Version",
                    "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                    SecurityPermissionState.Allow);
                /*****PROJECT EXCEL END****/

                /*****PROJECT EXPENSE START**/
                companyBranchReporting.AddTypePermissionsRecursively<ProjectExpense>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectExpense>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectExpense>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectExpense>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<ProjectDocument>(SecurityOperations.ReadWriteAccess,
                    "Branch Code; " +
                    "City; " +
                    "Code; Company Name; Cost Center Code; Country; Created By; Created On; Currency; Date; Description; Grand Total; Has Update; Invoice No; Is Equipment Invoice; Oid; Owner Company; Owner Company Code; Photo; Project; Total; Type; Updated By; Updated On; VAT; Version",
                    "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                    SecurityPermissionState.Allow);
                /*****PROJECT EXPENSE END****/

                /*****START PROJECT FILE ***/
                companyBranchReporting.AddTypePermissionsRecursively<ProjectFile>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectFile>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectFile>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectFile>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<ProjectFile>(SecurityOperations.ReadWriteAccess,
                    "Created By; Created On; File; Has Update; Oid; Photo; Project; Updated By; Updated On; Version",
                    "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                    SecurityPermissionState.Allow);
                /*****END PROJECT FILE ***/

                /*****START PROJECT LOCATION ***/
                companyBranchReporting.AddTypePermissionsRecursively<ProjectLocation>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectLocation>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectLocation>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectLocation>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<ProjectLocation>(SecurityOperations.ReadWriteAccess,
                    "Code; " +
                    "Created By; Created On; " +
                    "Description; " +
                    "Has Update; " +
                    "Is Active; Name; Oid; Photo; " +
                    "Project; " +
                    "Timesheets; " +
                    "Updated By; Updated On; Version",
                    "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                    SecurityPermissionState.Allow);
                /*****END PROJECT LOCATION   ***/

                /*****START PROJECT MEMBER *****/
                companyBranchReporting.AddTypePermissionsRecursively<ProjectMember>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectMember>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectMember>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectMember>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<ProjectMember>(SecurityOperations.ReadWriteAccess,
                   "Code; EMail; Name; Oid; Photo; Project; Surname",
                   "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                   SecurityPermissionState.Allow);
                /*****END PROJECT MEMEBER ******/


                /*****START PROJECT NOTE   *****/
                companyBranchReporting.AddTypePermissionsRecursively<ProjectNote>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectNote>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectNote>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectNote>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<ProjectNote>(SecurityOperations.ReadWriteAccess,
                   "Author; Date Time; Oid; Project; Text",
                   "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                   SecurityPermissionState.Allow);
                /*****END PROJECT NOTE     *****/

                /*****START PROJECT RISK   *****/
                companyBranchReporting.AddTypePermissionsRecursively<ProjectRisk>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectRisk>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectRisk>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectRisk>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<ProjectRisk>(SecurityOperations.ReadWriteAccess,
                   "Created By; Created On; Description; Has Update; Name; Oid; Photo; Project; Updated By; Updated On; Version",
                   "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                   SecurityPermissionState.Allow);
                /*****END PROJECT RISK     *****/

                /*****START PROJECT TIMESHEET   *****/
                companyBranchReporting.AddTypePermissionsRecursively<ProjectTimesheet>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectTimesheet>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectTimesheet>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectTimesheet>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyBranchReporting.AddMemberPermission<ProjectTimesheet>(SecurityOperations.ReadWriteAccess,
                   "Code; " +
                   "Created By; Created On; " +
                   "Date; " +
                   "DHour; DMinute; Duration; " +
                   "Employee; " +
                   "End Hour; End Minute; End Time; " +
                   "Has Update; " +
                   "Location; Oid; " +
                   "Project; " +
                   "Project Timesheet Type; PTF; " +
                   "Start Hour; Start Minute; Start Time; TF; " +
                   "Updated By; Updated On; Version;" +
                   "SH, SM; EH; EM",
                   "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                   SecurityPermissionState.Allow);
                /*****END PROJECT TIMESHEET     *****/


                /*****START PROJECT TIMESHEET TYPE   *****/
                companyBranchReporting.AddTypePermissionsRecursively<ProjectTimesheetType>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectTimesheetType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectTimesheetType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<ProjectTimesheetType>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<ProjectTimesheetType>(SecurityOperations.ReadWriteAccess,
                   "Amount; " +
                   "Code; " +
                   "Created By; Created On; " +
                   "Currency; " +
                   "Description; " +
                   "Has Update; Name; Oid; " +
                   "Project; Timesheets; " +
                   "Updated By; Updated On; Version",
                   "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                   SecurityPermissionState.Allow);
                /*****END PROJECT TIMESHEET TYPE    *****/


                /*****START PTF   *****/
                companyBranchReporting.AddTypePermissionsRecursively<PTF>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<PTF>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<PTF>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<PTF>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<ProjectTimesheetType>(SecurityOperations.ReadWriteAccess,
                   "Branch; " +
                   "Code; " +
                   "Created By; Created On; " +
                   "Date; Description; " +
                   "EH; " +
                   "Has Update; " +
                   "HH; " +
                   "Name; " +
                   "NH; " +
                   "Oid; " +
                   "Photo; Project; " +
                   "TH; " +
                   "Timesheets; " +
                   "Total NOK; " +
                   "Total OK; " +
                   "Total RNOK; " +
                   "Total ROK; " +
                   "Traceabilities; " +
                   "Updated By; Updated On; Version",
                   "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                   SecurityPermissionState.Allow);
                /*****END PTF    *****/

                /*****START TRACEABILITY *****/
                companyBranchReporting.AddTypePermissionsRecursively<Traceability>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyBranchReporting.AddTypePermissionsRecursively<Traceability>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<Traceability>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyBranchReporting.AddTypePermissionsRecursively<Traceability>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyBranchReporting.AddMemberPermission<Traceability>(
                        SecurityOperations.ReadWriteAccess,
                        "BOX; Created By; Created On; Date; DELIVERY; Description; Has Update; NOK; Oid; OK; ORDER; PALETTE; Part; Project; PTF; RNOK; ROK; SERIAL; TOTAL; Updated By; Updated On; Version",
                        "[PTF.Project.Branch.Employees][[Oid] Equals CurrentUserId()] Or " +
                        "[Part.Project.Branch.Employees][[Oid] Equals CurrentUserId()] Or " +
                        "[Project.Branch.Employees][[Oid] Equals CurrentUserId()]",
                        SecurityPermissionState.Allow
                    );
                /*****END TRACEABILITY *******/
            }
            catch
            {

            }
        }

        private void AddCompanyEmployeeRoleTypePermissions(Company com, EmployeeRole companyEmployeeRole)
        {
            try
            {
                companyEmployeeRole.AddObjectPermissionFromLambda<Employee>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                companyEmployeeRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);

                companyEmployeeRole.AddMemberPermissionFromLambda<Employee>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                companyEmployeeRole.AddMemberPermissionFromLambda<Employee>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);

                companyEmployeeRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyEmployeeRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                companyEmployeeRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                companyEmployeeRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyEmployeeRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            } catch
            {

            }
            //ObjectSpace.CommitChanges();
        }

        private void AddCompanyRoleTypePermissions(Company com, EmployeeRole companyHR)
        {
            EmployeeRole companyHRRole = ObjectSpace.FirstOrDefault<EmployeeRole>(r => r.Company.Oid == com.Oid && r.Oid == companyHR.Oid);

            if (companyHRRole != null && com.IsDefaultRolesCreated == false)
            {
                companyHRRole.AddObjectPermissionFromLambda<Employee>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                companyHRRole.AddNavigationPermission(@"Application/NavigationItems/Items/CompanyHR/Items", SecurityPermissionState.Allow);

                companyHRRole.AddTypePermissionsRecursively<EmployeeRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyHRRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                companyHRRole.AddMemberPermissionFromLambda<Employee>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);

                companyHRRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyHRRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                  
                companyHRRole.AddMemberPermission<Employee>(SecurityOperations.Read, "Code;FirstName;LastName;MiddleName;FullName;" +
                    "CitizenNo;" +
                    "EmployeePhoto;EmployeeRoles;Files;Photo;" +
                    "Start;End;" +
                    "CZTS;AZTS;ARFS;AZTS;KKDS;PIIFS;" +
                    "Car;" +
                    "Email;" +
                    "EMail;" +
                    "Mobile;LandLine;PhoneNumbers;" +
                    "Address;Address1;Address2;" +
                    "City;Country;Zip;" +
                    "Birthday;" +
                    "GrossSalaryMonthly;MonthlyHoursOfWork;SalaryCurrency;" +
                    "Projects;ProjectTimesheets;" +
                    "Branches;BranchTimesheets;" +
                    "Company;CompanyMobile;" +
                    "EmergencyContactName;EmergencyContactMobile;" +
                    "MyNotes;NotesAboutEmloyee;Tasks;" +
                    "IsActive;UserName;" +
                    "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;", "[Company.Employees][[Oid] = CurrentUserId()]", SecurityPermissionState.Allow);

                companyHRRole.AddMemberPermission<Employee>(SecurityOperations.Write, "Code;FirstName;LastName;MiddleName;FullName;" +
                    "CitizenNo;" +
                    "EmployeePhoto;EmployeeRoles;Files;Photo;" +
                    "Start;End;" +
                    "CZTS;AZTS;ARFS;AZTS;KKDS;PIIFS;" +
                    "Car;" +
                    "Email;" +
                    "EMail;" +
                    "Mobile;LandLine;PhoneNumbers;" +
                    "Address;Address1;Address2;" +
                    "City;Country;Zip;" +
                    "Birthday;" +
                    "GrossSalaryMonthly;MonthlyHoursOfWork;SalaryCurrency;" +
                    "Projects;ProjectTimesheets;" +
                    "Branches;BranchTimesheets;" +
                    "Company;CompanyMobile;" +
                    "EmergencyContactName;EmergencyContactMobile;" +
                    "MyNotes;NotesAboutEmloyee;Tasks;" +
                    "IsActive;UserName;" +
                    "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;", "[Company.Employees][[Oid] = CurrentUserId()]", SecurityPermissionState.Allow);

                companyHRRole.AddTypePermission<EmployeeRole>(SecurityOperations.Read,SecurityPermissionState.Allow);
                companyHRRole.AddTypePermission<EmployeeRole>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermission<EmployeeRole>(SecurityOperations.Create, SecurityPermissionState.Deny);
                companyHRRole.AddTypePermission<EmployeeRole>(SecurityOperations.Delete, SecurityPermissionState.Deny);
                
                companyHRRole.AddMemberPermission<EmployeeRole>(SecurityOperations.Read,
                    "Company; Employees; IsAdministrative;" +
                    "Name;" +
                    "QPMSReports;QPMSDashboards;QPMSAnalyses;" +
                    "NavigationPermissions;ActionPermissions;TypePermissions;",
                    "[Company.Employees][[Oid] = CurrentUserId()]",
                    SecurityPermissionState.Allow);

                companyHRRole.AddMemberPermission<EmployeeRole>(SecurityOperations.Write,
                    "Employees;",
                    "[Company.Employees][[Oid] = CurrentUserId()]",
                    SecurityPermissionState.Allow);

                #region Forms Allowance Request Form
                companyHRRole.AddTypePermission<EmployeeFormARF>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyHRRole.AddTypePermission<EmployeeFormARF>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermission<EmployeeFormARF>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermission<EmployeeFormARF>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyHRRole.AddMemberPermission<EmployeeFormARF>(SecurityOperations.Write,
                        "Currency;Date; Employee;Amount;"+
                        "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;", "[Employee.Company.Employees][[Oid] = CurrentUserId()]", SecurityPermissionState.Allow);

                companyHRRole.AddMemberPermission<EmployeeFormARF>(SecurityOperations.Read,
                        "Currency;Date; Employee;Amount;" +
                        "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;", "[Employee.Company.Employees][[Oid] = CurrentUserId()]", SecurityPermissionState.Allow);


                companyHRRole.AddTypePermission<EmployeeFormAZT>(SecurityOperations.Read,SecurityPermissionState.Deny);
                companyHRRole.AddTypePermission<EmployeeFormAZT>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermission<EmployeeFormAZT>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermission<EmployeeFormAZT>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyHRRole.AddMemberPermission<EmployeeFormAZT>(SecurityOperations.Write,
                    "Date; Car; Employee;" +
                    "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;", 
                    "[Employee.Company.Employees][[Oid] = CurrentUserId()]", 
                    SecurityPermissionState.Allow);

                companyHRRole.AddMemberPermission<EmployeeFormAZT>(SecurityOperations.Read,
                    "Date; Car; Employee;" +
                    "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;",
                    "[Employee.Company.Employees][[Oid] = CurrentUserId()]",
                    SecurityPermissionState.Allow);

                companyHRRole.AddTypePermission<EmployeeFormCZT>(SecurityOperations.Read,SecurityPermissionState.Deny);
                companyHRRole.AddTypePermission<EmployeeFormCZT>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermission<EmployeeFormCZT>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermission<EmployeeFormCZT>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyHRRole.AddMemberPermission<EmployeeFormCZT>(
                        SecurityOperations.Read,
                            "Date;Employee;Mobile;" +
                            "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;",
                            "[Employee.Company.Employees][[Oid] = CurrentUserId()]",
                        SecurityPermissionState.Allow
                    );

                companyHRRole.AddMemberPermission<EmployeeFormCZT>(
                        SecurityOperations.Write,
                            "Date;Employee;Mobile;" +
                            "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;",
                            "[Employee.Company.Employees][[Oid] = CurrentUserId()]",
                        SecurityPermissionState.Allow
                    );

                companyHRRole.AddTypePermission<EmployeeFormKKD>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyHRRole.AddTypePermission<EmployeeFormKKD>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermission<EmployeeFormKKD>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermission<EmployeeFormKKD>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyHRRole.AddMemberPermission<EmployeeFormKKD>(SecurityOperations.Read,
                            "Employee;Date;" +
                            "EarProtector; ESDShoes;ESDWear; SafetyHarnessBelt; SafetyHelmet; SafetyShoes; WorkingMask; WorkingWear;" +
                            "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;",
                    "[Employee.Company.Employees][[Oid] = CurrentUserId()]", 
                    SecurityPermissionState.Allow);


                companyHRRole.AddMemberPermission<EmployeeFormKKD>(SecurityOperations.Write,
                            "Employee;Date;" +
                            "EarProtector; ESDShoes;ESDWear; SafetyHarnessBelt; SafetyHelmet; SafetyShoes; WorkingMask; WorkingWear;" +
                            "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;",
                    "[Employee.Company.Employees][[Oid] = CurrentUserId()]",
                    SecurityPermissionState.Allow);


                #endregion


                companyHRRole.AddTypePermissionsRecursively<Branch>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyHRRole.AddTypePermissionsRecursively<Branch>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermissionsRecursively<Branch>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermissionsRecursively<Branch>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                companyHRRole.AddMemberPermission<Branch>(
                        SecurityOperations.Read,
                        "Main;Name;Code;Company;CostCenter;Locations;Photo;SubBranches;" +
                        "BDA;BTFs;BranchTimesheets;" +
                        "Employees;" +
                        "Interviews;" +
                        "PTFs;Projects;" +
                        "BranchManager;BranchResponsible;KAM;" +
                        "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;",
                        "[Company.Employees][[Oid] = CurrentUserId()] Or [Employees][[Oid] = CurrentUserId()]"
                        ,
                        SecurityPermissionState.Allow
                    );

                companyHRRole.AddMemberPermission<Branch>(
                        SecurityOperations.Write,
                        "Main;Name;Code;Company;CostCenter;Locations;Photo;SubBranches;" +
                        "BDA;BTFs;BranchTimesheets;" +
                        "Employees;" +
                        "Interviews;" +
                        "PTFs;Projects;" +
                        "BranchManager;BranchResponsible;KAM;" +
                        "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;",
                        "[Company.Employees][[Oid] = CurrentUserId()] Or [Employees][[Oid] = CurrentUserId()]"
                        ,
                        SecurityPermissionState.Allow
                    );


                companyHRRole.AddObjectPermission<Branch>(SecurityOperations.Read, "[Employees][[Oid] = CurrentUserId()]", SecurityPermissionState.Allow);


                companyHRRole.AddTypePermissionsRecursively<BTF>(SecurityOperations.Read, SecurityPermissionState.Deny);
                companyHRRole.AddTypePermissionsRecursively<BTF>(SecurityOperations.Write, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermissionsRecursively<BTF>(SecurityOperations.Create, SecurityPermissionState.Allow);
                companyHRRole.AddTypePermissionsRecursively<BTF>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                companyHRRole.AddMemberPermission<BTF>(SecurityOperations.Read, 
                "Branch;BranchTimesheets;Date;Name;" +
                "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;", 
                "[Branch.Company.Employees][[Oid] = CurrentUserId()]",
                SecurityPermissionState.Allow);

                companyHRRole.AddMemberPermission<BTF>(SecurityOperations.Write,
               "Branch;BranchTimesheets;Date;Name;" +
               "Version;HasUpdate;CreatedBy;CreatedOn;UpdatedBy;UpdatedOn;",
               "[Branch.Company.Employees][[Oid] = CurrentUserId()]",
               SecurityPermissionState.Allow);




                //branchResponsibleRole.AddTypePermissionsRecursively<BTF>(SecurityOperations.Read, SecurityPermissionState.Allow);
                //branchResponsibleRole.AddTypePermissionsRecursively<BranchTimesheet>(SecurityOperations.Read, SecurityPermissionState.Deny);
                //branchResponsibleRole.AddTypePermissionsRecursively<Project>(SecurityOperations.Read, SecurityPermissionState.Allow);
                //branchResponsibleRole.AddTypePermissionsRecursively<PTF>(SecurityOperations.Read, SecurityPermissionState.Deny);
                //branchResponsibleRole.AddTypePermissionsRecursively<ProjectTimesheet>(SecurityOperations.Read, SecurityPermissionState.Allow);
                //branchResponsibleRole.AddTypePermissionsRecursively<Part>(SecurityOperations.Read, SecurityPermissionState.Deny);
                //branchResponsibleRole.AddTypePermissionsRecursively<FailureMode>(SecurityOperations.Read, SecurityPermissionState.Allow);
                //branchResponsibleRole.AddTypePermissionsRecursively<Failure>(SecurityOperations.Read, SecurityPermissionState.Deny);
                //branchResponsibleRole.AddTypePermissionsRecursively<Traceability>(SecurityOperations.Read, SecurityPermissionState.Allow);
            }

            com.IsDefaultRolesCreated = true;

            ObjectSpace.CommitChanges();
        }

        private void CreateDefaultMisc_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var currentCompany1 = View.CurrentObject as Company;

            Company currentCompany = ObjectSpace.FindObject<Company>(
                    new BinaryOperator("Oid",currentCompany1.Oid)
                );


            if (currentCompany != null)
            {
                CreateDefaultBranches(currentCompany);
                CreateDefaultDepartments(currentCompany);
                CreateBranchTimesheetTimeConstraints(currentCompany);
                CreateProjectTimesheetTimeConstraints(currentCompany);
                CreateCompanyAdmin(currentCompany);
            }

           


        }

        private void CreateCompanyAdmin(Company currentCompany)
        {
            try
            {
                var employee = currentCompany.Employees.FirstOrDefault<Employee>(p=>p.UserName == (currentCompany.Name + " " + "Admin"));

                if(employee == null)
                {
                    employee = ObjectSpace.CreateObject<Employee>();
                    employee.UserName = currentCompany.Name + " " + "Admin";
                    employee.FirstName = "NA";
                    employee.LastName = "NA";
                    employee.Country = "NA";
                    employee.City = "NA";
                    employee.Address = "NA";
                    employee.Mobile = "NA";
                    employee.Company = currentCompany;
                    currentCompany.Employees.Add(employee);

                    try
                    {
                        EmployeeRole companyAdminRole = currentCompany.EmployeeRoles.FirstOrDefault(p=>p.Name == currentCompany.Name + " " + "Company Admin");

                        if (companyAdminRole != null)
                        {
                            companyAdminRole.Employees.Add(employee);
                            employee.EmployeeRoles.Add(companyAdminRole);
                        }
                    } catch
                    {

                    }

                    try
                    {
                        Branch adminBranch = currentCompany.Branches.FirstOrDefault(p=>p.Name == "Central Branch");

                        if (adminBranch != null)
                        {
                            adminBranch.Employees.Add(employee);
                            employee.Branches.Add(adminBranch);
                        }
                    } catch
                    {

                    }


                    try
                    {
                        Department adminDepartment = currentCompany.Departments.FirstOrDefault(p=>p.Name == (currentCompany.Name + " " + "IT"));

                        if (adminDepartment != null)
                        {
                            
                        }
                    }
                    catch
                    {

                    }
                }

                ObjectSpace.CommitChanges();
            } catch
            {

            }
        }

        private void CreateDefaultDepartments(Company currentCompany)
        {
            try
            {
                var departments = currentCompany.Departments;

                if (departments.Count == 0)
                {
                    Department hrDepartment = ObjectSpace.CreateObject<Department>();
                    hrDepartment.Name = currentCompany.Name + " " + "HR";
                    currentCompany.Departments.Add(hrDepartment);
                    hrDepartment.Company = currentCompany;

                    Department itDepartment = ObjectSpace.CreateObject<Department>();
                    itDepartment.Name = currentCompany.Name + " " + "IT";
                    currentCompany.Departments.Add(itDepartment);
                    itDepartment.Company = currentCompany;
                }

                ObjectSpace.CommitChanges();
            } catch
            {

            }
        }

        private void CreateDefaultBranches(Company currentCompany)
        {
            try
            {
                var branches = currentCompany.Branches;

                if(branches.Count == 0)
                {
                    Branch branchCental = ObjectSpace.CreateObject<Branch>();

                    branchCental.Name = "Cental Branch";
                    branchCental.Company = currentCompany;
                    currentCompany.Branches.Add(branchCental);

                    Branch branchOperational = ObjectSpace.CreateObject<Branch>();
                    branchOperational.Name = "Operations Branch";
                    branchOperational.Company = currentCompany;
                    currentCompany.Branches.Add(branchOperational);
                }

                ObjectSpace.CommitChanges();
            } catch
            {

            }
        }

        private void CreateProjectTimesheetTimeConstraints(Company currentCompany)
        {
            try
            {
                var proejctSHList = currentCompany.ProjectTimesheetStartHours.ToList();

                if (proejctSHList.Count == 0)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        ProjectTimesheetStartHour ptsh = currentCompany.Session.FindObject<ProjectTimesheetStartHour>(
                                new BinaryOperator("Val",i.ToString())
                            );

                        if (ptsh == null)
                        {
                            ptsh = ObjectSpace.CreateObject<ProjectTimesheetStartHour>();

                            ptsh.Val = i;
                            ptsh.StartHour = i.ToString("00");
                            ptsh.Name = i.ToString("00");
                            ptsh.Company = currentCompany;
                            currentCompany.ProjectTimesheetStartHours.Add(ptsh);
                        }

                        ObjectSpace.CommitChanges();
                    }
                }

                var projectSMList = currentCompany.ProjectTimesheetStartMinutes.ToList();

                if(projectSMList.Count == 0)
                {
                    for(int i = 0; i < 60; i++)
                    {
                        ProjectTimesheetStartMinute ptsm = currentCompany.Session.FindObject<ProjectTimesheetStartMinute>(
                                new BinaryOperator("Val", i.ToString())
                            );
                            
                        if(ptsm == null)
                        {
                            ptsm = ObjectSpace.CreateObject<ProjectTimesheetStartMinute>();

                            ptsm.Val = i;
                            ptsm.StartMinute = i.ToString("00");
                            ptsm.Name = i.ToString("00");
                            ptsm.Company = currentCompany;
                            currentCompany.ProjectTimesheetStartMinutes.Add(ptsm);
                        }

                        ObjectSpace.CommitChanges();
                    }
                }

                var projectEHList = currentCompany.ProjectTimesheetEndHours.ToList();

                if(projectEHList.Count == 0)
                {
                    for(int i=0;i<25; i++)
                    {
                        ProjectTimesheetEndHour pteh = currentCompany.Session.FindObject<ProjectTimesheetEndHour>(
                                new BinaryOperator("Val",i.ToString())
                            );

                        if (pteh == null)
                        {
                            pteh = ObjectSpace.CreateObject<ProjectTimesheetEndHour>();

                            pteh.Val = i;
                            pteh.EndHour = i.ToString("00");
                            pteh.Name = i.ToString("00");
                            pteh.Company = currentCompany;
                            currentCompany.ProjectTimesheetEndHours.Add(pteh);

                        }

                        ObjectSpace.CommitChanges();
                    }
                }


                var projectEMList = currentCompany.BranchTimesheetEndMinutes.ToList();

                if (projectEMList.Count == 0)
                {
                    for(int i =0;i<60; i++)
                    {
                        ProjectTimesheetEndMinute ptem = currentCompany.Session.FindObject<ProjectTimesheetEndMinute>(
                                new BinaryOperator("Val",i.ToString())
                            );

                        if(ptem == null)
                        {
                            ptem = ObjectSpace.CreateObject<ProjectTimesheetEndMinute>();

                            ptem.Val = i;
                            ptem.EndMinute = i.ToString("00");
                            ptem.Name = i.ToString("00");
                            ptem.Company = currentCompany;
                            currentCompany.ProjectTimesheetEndMinutes.Add(ptem);
                        }

                        ObjectSpace.CommitChanges();
                    }
                }


            } catch
            {
                
            }


            
        }

        private void CreateBranchTimesheetTimeConstraints(Company currentCompany)
        {
            try
            {
                var branchSHList = currentCompany.BranchTimesheetStartHours.ToList();

                if (branchSHList.Count == 0)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        BranchTimesheetStartHour btsh = currentCompany.Session.FindObject<BranchTimesheetStartHour>(
                                new BinaryOperator("Val", i.ToString())
                            );

                        if (btsh == null)
                        {
                            btsh = ObjectSpace.CreateObject<BranchTimesheetStartHour>();
                            btsh.Val = i;
                            btsh.StartHour = i.ToString("00");
                            btsh.Name = i.ToString("00");
                            btsh.Company = currentCompany;
                            currentCompany.BranchTimesheetStartHours.Add(btsh);
                        }

                        ObjectSpace.CommitChanges();
                    }
                }


                var branchSMList = currentCompany.BranchTimesheetStartMinutes.ToList();

                if (branchSMList.Count == 0)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        BranchTimesheetStartMinute btsm = currentCompany.Session.FindObject<BranchTimesheetStartMinute>(
                                new BinaryOperator("Val", i.ToString())
                            );

                        if (btsm == null)
                        {
                            btsm = ObjectSpace.CreateObject<BranchTimesheetStartMinute>();
                            btsm.Val = i;
                            btsm.StartMinute = i.ToString("00");
                            btsm.Name = i.ToString("00");
                            btsm.Company = currentCompany;
                            currentCompany.BranchTimesheetStartMinutes.Add(btsm);
                        }

                        ObjectSpace.CommitChanges();
                    }
                }

                var branchEHList = currentCompany.BranchTimesheetEndHours.ToList();

                if (branchEHList.Count == 0)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        BranchTimesheetEndHour bteh = currentCompany.Session.FindObject<BranchTimesheetEndHour>(
                                new BinaryOperator("Val", i.ToString())
                            );

                        if (bteh == null)
                        {
                            bteh = ObjectSpace.CreateObject<BranchTimesheetEndHour>();
                            bteh.Val = i;
                            bteh.EndHour = i.ToString("00");
                            bteh.Name = i.ToString("00");
                            bteh.Company = currentCompany;
                            currentCompany.BranchTimesheetEndHours.Add(bteh);
                        }

                        ObjectSpace.CommitChanges();
                    }
                }

                var branchEMList = currentCompany.BranchTimesheetEndMinutes.ToList();

                if (branchEMList.Count == 0)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        BranchTimesheetEndMinute btem = currentCompany.Session.FindObject<BranchTimesheetEndMinute>(
                                new BinaryOperator("Val", i.ToString())
                            );

                        if (btem == null)
                        {
                            btem = ObjectSpace.CreateObject<BranchTimesheetEndMinute>();
                            btem.Val = i;
                            btem.EndMinute = i.ToString("00");
                            btem.Name = i.ToString("00");
                            btem.Company = currentCompany;
                            currentCompany.BranchTimesheetEndMinutes.Add(btem);
                        }

                        ObjectSpace.CommitChanges();
                    }
                }

            }
            catch
            {

            }
        }

        private void simpleActionRecalc_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Recalc();
        }


        private void Recalc()
        {
            Debug.WriteLine("Entering Routine");
            XPObjectSpaceProvider globalObjectSpace = new XPObjectSpaceProvider(
 ConnectionStringProvider.RequireConnectionString(), null);

            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(BranchTimesheet));
                XPObjectSpaceProvider osProvider = globalObjectSpace;
                IObjectSpace objectSpace = osProvider.CreateObjectSpace();

                var branchTimesheetsList = objectSpace.GetObjects<BranchTimesheet>().ToList();

                Debug.WriteLine("Branch Tİmehsheets initialized");

                foreach (var item in branchTimesheetsList)
                {

                    try
                    {
                        DateTime startT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.SH.Val, item.SM.Val, 0);

                        DateTime endT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.EH.Val, item.EM.Val, 0);

                        if (item.EH.Val >= 24)
                        {
                            endT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, 0, 0, 0).AddDays(1);
                            item.EM = item.CreatedBy.Company.BranchTimesheetEndMinutes.Where(p => p.Val == 0 && p.Company.Oid == item.UpdatedBy.Company.Oid).FirstOrDefault();
                        }
                        else
                        {
                            endT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.EH.Val, item.EM.Val, 0);

                            if (endT < startT)
                            {

                                item.EH = item.CreatedBy.Company.BranchTimesheetEndHours.Where(p => p.Val == item.SH.Val && p.Company.Oid == item.UpdatedBy.Company.Oid).FirstOrDefault();
                                item.EM = item.CreatedBy.Company.BranchTimesheetEndMinutes.Where(p => p.Val == item.SM.Val && p.Company.Oid == item.UpdatedBy.Company.Oid).FirstOrDefault();
                                endT = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.EH.Val, item.EM.Val, 0);
                            }
                        }
                        //DateTime endT = new DateTime(Date.Year, Date.Month, Date.Day, EH.Val, EM.Val, 0);

                        //item.Duration = endT.Subtract(startT).TotalMinutes;
                        item.Hour = endT.Subtract(startT).TotalHours;

                        objectSpace.CommitChanges();
                        Debug.WriteLine(item.Oid.ToString() + " " + "has been successfully saved with " + item.Hour.ToString());
                    }
                    catch
                    {
                        Debug.WriteLine(item.Oid + " " + item.Date.ToString() + "Problem on timesheet");
                    }


                }
            }
            catch
            {
                Debug.WriteLine("Problem in recalc routine");
            }
        }
    }
}
