# Task Summary Report 01

Append new task summaries to this file until it reaches 20 MB.
After that, create `summary-report-02.md` and continue there.

## 2026-04-24 02:03 Europe/Istanbul - QPMS application error / qpms2026

- Scope:
  - Investigated the live IIS site `qpms.orerps.com`.
  - Compared deployed configuration with the local source repo.
  - Checked SQL Server service state, database availability, IIS app pool identity, application log, and SQL Server event log.

- Primary finding:
  - The deployed site uses this connection target in `C:\Plesk Vhosts\orerps.com\qpms.orerps.com\Web.config`:
    - `Data Source=.;Initial Catalog=qpms2026;Integrated Security=SSPI;Pooling=false;`
  - `qpms2026` exists and is `ONLINE`.
  - The IIS app pool identity is `NS10EB7FAF1FF17\IWPD_4(orerps.c_vf4)`.
  - SQL Server Application log entries around `2026-04-24 01:57:40` through `01:59:26` show repeated `18456` login failures for that IIS identity with:
    - `Reason: Failed to open the explicitly specified database 'qpms2026'.`
  - No matching SQL Server login or database user mapping was found for that IIS identity during inspection.
  - Result: the web app can render the login page, but database-backed work fails for the IIS worker identity.

- Supporting finding:
  - Earlier in `C:\Plesk Vhosts\orerps.com\qpms.orerps.com\eXpressAppFramework.log`, there was also a connection failure during logon compatibility checks while SQL connectivity was unavailable.
  - After SQL Server came back, the current blocker became database access for the IIS identity rather than instance discovery.

- Secondary risk found in source code:
  - The source repo still contains hard-coded legacy connections that point to `qpms_prod_4` with SQL login `webadmin1`.
  - Relevant files observed:
    - `QPMS.Web/Global.asax.cs`
    - `QPMS.Web/Startup.cs`
    - `QPMS.Web/HangFireJobs.aspx.cs`
  - SQL Server event log also showed earlier login failures for `webadmin1`, which is consistent with those code paths still existing.

- Likely fix path:
  - Create a SQL Server login/user mapping for `NS10EB7FAF1FF17\IWPD_4(orerps.c_vf4)` and grant access to `qpms2026`.
  - Recycle the `qpms.orerps.com(domain)(4.0)(pool)` app pool after the SQL permission fix.
  - Remove hard-coded legacy connection strings from code and use the configured connection string everywhere before the next deployment.

- Changes applied during this investigation:
  - No live SQL permission change applied.
  - No live IIS config change applied.
  - No source code change applied.

## 2026-04-24 02:10 Europe/Istanbul - Follow-up applied changes

- Live SQL fix applied:
  - Created SQL Server Windows login for `NS10EB7FAF1FF17\IWPD_4(orerps.c_vf4)`.
  - Set its default database to `qpms2026`.
  - Created the matching database user in `qpms2026`.
  - Granted `CONNECT` and added the user to `db_owner`.
  - Recycled IIS app pool `qpms.orerps.com(domain)(4.0)(pool)`.

- Live verification:
  - `EXECUTE AS LOGIN = 'NS10EB7FAF1FF17\IWPD_4(orerps.c_vf4)'` now reports `HAS_DBACCESS('qpms2026') = 1`.
  - The login page `https://qpms.orerps.com/Login.aspx?ReturnUrl=%2f` now returns normal logon HTML instead of the generic application error page.
  - No newer `18456` login failure for that IIS identity was observed after `2026-04-24 02:03:40` during this check window.

- Source changes applied:
  - Added `QPMS.Module.ConnectionStringProvider.RequireConnectionString()` to centralize active connection string lookup.
  - Replaced active hard-coded `qpms_prod_4 / webadmin1` usage in:
    - `QPMS.Web/Global.asax.cs`
    - `QPMS.Web/Startup.cs`
    - `QPMS.Web/HangFireJobs.aspx.cs`
    - `QPMS.Module/Controllers/ViewControllerCompany.cs`
  - Switched active app config/web config connection strings to local `qpms2026` in:
    - `QPMS.Web/Web.config`
    - `QPMS.Win/App.config`
    - `OBI.Web/Web.config`
    - `OBI.Win/App.config`

- Remaining notes:
  - Old remote connection examples still remain in commented-out config lines only.
  - Source changes were not redeployed from this repo in this step because a full .NET Framework / Visual Studio MSBuild environment and required DevExpress build references are not available in the current shell environment.
  - `dotnet msbuild` failed due missing WebApplication targets, unsupported `LC` task on .NET Core MSBuild, and unresolved DevExpress `v25.2` references.

## 2026-04-24 02:18 Europe/Istanbul - QPMS 25.1 build and live deployment

- User direction applied:
  - Standardized the QPMS solution branch on DevExpress `25.1` because `25.2` is not compatible with some project classes.

## 2026-04-24 02:42 Europe/Istanbul - GitHub push investigation

- Scope:
  - Checked repository status, active branch, upstream tracking, and `origin` remote configuration.
  - Reproduced push behavior with `git push --dry-run origin main` and `git push origin main`.

- Findings:
  - `origin` is configured as `https://github.com/kemrantuna-lab/QPMS.git`.
  - Local branch `main` tracks `origin/main`.
  - `git rev-list --left-right --count origin/main...main` returned `0  0`, so local and remote are on the same commit.
  - Both `git push --dry-run origin main` and `git push origin main` returned `Everything up-to-date`.
  - There are staged but uncommitted changes in 19 files, including:
    - `QPMS.Web/Web.config`
    - `QPMS.Web/Global.asax.cs`
    - `QPMS.Module/Module.cs`
    - `build/msbuild/v18.0/WebApplications/Microsoft.WebApplication.targets`
    - `task-reports/summary-report-01.md`

- Conclusion:
  - GitHub push is currently not blocked by remote access or branch configuration.
  - The likely reason for "push olmuyor" is that the current changes are only staged and have not been committed yet, so there is no new commit to send.

- Changes applied during this investigation:
  - No source code change applied.
  - No git commit created.
  - No remote branch change applied.

- Source/build updates:
  - Replaced active `v25.2` references with `v25.1` across QPMS projects:
    - `QPMS.Module/ QPMS.Module.csproj`
    - `QPMS.Module.Web/ QPMS.Module.Web.csproj`
    - `QPMS.Module.Win/ QPMS.Module.Win.csproj`
    - `QPMS.Web/ QPMS.Web.csproj`
    - `QPMS.Win/ QPMS.Win.csproj`
    - `QPMS.Web/Web.config`
    - `QPMS.Web/Default.aspx`
    - `QPMS.Web/Login.aspx`
    - `QPMS.Module/FunctionalTests/config.xml`
  - Updated explicit assembly version declarations from `25.2.4.0` to `25.1.5.0`.
  - Updated `DevExpress.ExpressApp.CodeAnalysis` package references from `25.2.4` to `25.1.4`.

- Build environment workaround:
  - Added a minimal local stub target file:
    - `build/msbuild/v18.0/WebApplications/Microsoft.WebApplication.targets`
  - Created a local reference pool at:
    - `build/references/qpms-web`
  - Seeded that folder from the live working `bin` plus `System.EnterpriseServices.Wrapper.dll` from the .NET Framework path so MSBuild could resolve all web-project references without touching system MSBuild directories.

- Build result:
  - Built successfully with:
    - `C:\Program Files\Microsoft SQL Server Management Studio 22\Release\MSBuild\Current\Bin\MSBuild.exe`
  - Successful outputs:
    - `QPMS.Module\bin\Release\QPMS.Module.dll`
    - `QPMS.Module.Web\bin\Release\QPMS.Module.Web.dll`
    - `QPMS.Web\bin\QPMS.Web.dll`

- Live deployment applied:
  - Backed up current live assemblies to:
    - `C:\Plesk Vhosts\orerps.com\qpms.orerps.com\_deploy-backups\20260424-021715`
  - Stopped IIS app pool `qpms.orerps.com(domain)(4.0)(pool)`.
  - Deployed updated:
    - `QPMS.Module.dll`
    - `QPMS.Module.Web.dll`
    - `QPMS.Web.dll`
  - Restarted the same app pool.

- Live verification:
  - App pool state returned to `Started`.
  - Live file timestamps now match the new build outputs:
    - `QPMS.Module.dll` `2026-04-24 02:16:36`
    - `QPMS.Module.Web.dll` `2026-04-24 02:16:41`
    - `QPMS.Web.dll` `2026-04-24 02:16:44`
  - SHA256 hashes of deployed DLLs match the locally built DLLs for all three files.
  - `https://qpms.orerps.com/Login.aspx?ReturnUrl=%2f` returned normal login HTML with HTTP `200` after deployment.

- Notes:
  - Deployment scope was limited to changed QPMS assemblies because the live site was already running with working `25.1` markup/config and the active `qpms2026` connection string.

## 2026-04-24 02:23 Europe/Istanbul - Admin password reset

- Scope:
  - Reviewed the active authentication user type for QPMS web/win applications.
  - Reset the password for the login account actually used by the QPMS application.

- Findings:
  - The active QPMS authentication configuration uses `Employee` as the security user type in:
    - `QPMS.Web/WebApplication.cs`
    - `QPMS.Win/WinApplication.Designer.cs`
  - There is also a separate `CustomerUser` row with `UserName = 'Admin'`, but it is not the active user type for the main QPMS application login.

- Change applied:
  - Updated `Employee` table row where `UserName = 'Admin'` in database `qpms2026`.
  - Regenerated `StoredPassword` using the DevExpress `PasswordCryptographer` logic from the live `25.1` assembly instead of writing a manual hash.
  - Set `ChangePasswordOnFirstLogon = 0`.

- Verification:
  - Exactly one `Employee/Admin` row was updated.
  - `IsActive = 1`.
  - The stored hash verifies successfully against the requested new password using the same DevExpress verifier used by the application.

- Notes:
  - The separate `CustomerUser/Admin` record was left unchanged intentionally.

## 2026-04-24 10:38 Europe/Istanbul - QPMS Let's Encrypt certificate deployment

- Scope:
  - Added a live Let's Encrypt certificate for `qpms.orerps.com`.
  - Verified the certificate was deployed through Plesk and bound in IIS.
  - Confirmed the HTTPS login endpoint still responds normally after the certificate change.

- Findings during issuance:
  - A direct `win-acme` filesystem challenge attempt against the app root was blocked because `/.well-known/acme-challenge/*` was being redirected into the QPMS login flow.
  - The server already had the Plesk `letsencrypt` extension installed, so issuance was completed through Plesk instead of keeping an app-level ACME workaround.

- Changes applied:
  - Executed:
    - `plesk bin extension --exec letsencrypt cli.php -d qpms.orerps.com -m info@orerps.com`
  - Plesk created and assigned certificate:
    - `Lets Encrypt qpms.orerps.com`
  - IIS HTTPS bindings for `qpms.orerps.com` now use certificate thumbprint:
    - `32D41597C774C1822D53D301F27F7863A45AF883`
  - Issuer:
    - `Let's Encrypt R13`

- Live verification:
  - `domain.exe --info qpms.orerps.com` now reports:
    - `Certificate: Lets Encrypt qpms.orerps.com`
  - The certificate in `LocalMachine\WebHosting` has:
    - `Subject: CN=qpms.orerps.com`
    - `NotBefore: 2026-04-24 09:36:23`
    - `NotAfter: 2026-07-23 09:36:22`
  - TLS handshake against `qpms.orerps.com:443` returns the same thumbprint and subject.
  - `curl -k -I https://qpms.orerps.com/Login.aspx?ReturnUrl=%2f` returned `HTTP/1.1 200 OK`.

- Notes:
  - Temporary ACME test files and repo-side test artifacts were cleaned up after issuance.
  - `www.qpms.orerps.com` and `ipv4.qpms.orerps.com` still have IIS bindings, but DNS resolution for those hostnames was not present during this check window.

## 2026-04-24 10:47 Europe/Istanbul - QPMS warning cleanup

- Scope:
  - Reproduced the current warning set by building the solution with MSBuild.
  - Cleaned the actionable compiler and reference warnings in the `QPMS.Module` path.
  - Rebuilt `QPMS.Module`, `QPMS.Module.Web`, and `QPMS.Web` to verify warning-free output.

- Findings:
  - The actionable warnings were concentrated in `QPMS.Module` and were mainly:
    - unused exception variables
    - dead locals and private fields
    - stale project references to unused DevExpress assemblies
  - Two warnings exposed real defects rather than harmless noise:
    - `Employee.CompanyMobile` was not assigning its backing field
    - `ViewControllerBranchTimesheetC` was checking a `NewObjectViewController` field that was never assigned

- Changes applied:
  - Removed unused catch variables and dead locals in:
    - `QPMS.Module/Controllers/ViewControllerProjectCost.cs`
    - `QPMS.Module/Controllers/ViewControllerBranchAnalysisDaily.cs`
    - `QPMS.Module/Controllers/ViewControllerProject.cs`
    - `QPMS.Module/BusinessObjects/EmployeeForm.cs`
    - `QPMS.Module/Controllers/ViewControllerFailure.cs`
    - `QPMS.Module/BusinessObjects/Project.cs`
  - Removed never-used private fields in:
    - `QPMS.Module/Controllers/ViewControllerBranchTimesheetDetailView.cs`
    - `QPMS.Module/BusinessObjects/Project.cs`
    - `QPMS.Module/BusinessObjects/ProjectAnalysisDaily.cs`
    - `QPMS.Module/BusinessObjects/EmployeeQuit.cs`
  - Restored intended controller wiring in:
    - `QPMS.Module/Controllers/ViewControllerBranchTimesheetC.cs`
  - Fixed the `CompanyMobile` / `User` association cleanup logic in:
    - `QPMS.Module/BusinessObjects/Employee.cs`
    - `QPMS.Module/BusinessObjects/CompanyMobilePhone.cs`
  - Fixed two obvious copy-paste issues discovered in touched files:
    - `QPMS.Module/BusinessObjects/Project.cs` now uses `fTotalMealPrice` as the old value for `TotalMealPrice`
    - `QPMS.Module/BusinessObjects/EmployeeQuit.cs` now returns `updatedBy` from `UpdatedBy`
  - Removed unused unresolved DevExpress references from:
    - `QPMS.Module/QPMS.Module.csproj`
    - Removed references:
      - `DevExpress.Data.Desktop.v25.1`
      - `DevExpress.Pdf.v25.1.Core`
      - `DevExpress.Sparkline.v25.1.Core`
      - `DevExpress.XtraGauges.v25.1.Core`

- Verification:
  - `QPMS.Module.csproj` build completed with:
    - `0 Warning(s)`
    - `0 Error(s)`
  - `QPMS.Module.Web.csproj` build completed with:
    - `0 Warning(s)`
    - `0 Error(s)`
  - `QPMS.Web.csproj` build completed with:
    - `0 Warning(s)`
    - `0 Error(s)`

- Notes:
  - The first full-solution build still showed separate `OBI.*` branch issues outside this warning cleanup scope:
    - `OBI.*` projects still reference missing DevExpress `25.2` assemblies on this machine
    - legacy web projects require the local `build/msbuild/v18.0` web-target shim when using the current MSBuild toolchain
  - A temporary local reference pool was created only for verification of the QPMS web build path and then removed.

## 2026-04-24 12:31 Europe/Istanbul - Project Report 2 template recovery and upload

- Scope:
  - Analyzed the missing sample output `Sondaj Sayım Isçiligi - Arcelik Bolu(Mustafa Kacan)-XFDB13155`.
  - Recovered the matching report layout source from the live QPMS reporting stack.
  - Uploaded a usable `Project Report 2` template record into the system and assigned it to the target project.

- Findings:
  - The sample PDF exists in the repo at:
    - `QPMS.Module/Reports/Sondaj Sayim Isciligi - Arcelik Bolu(Mustafa Kacan)-XFDB13155.pdf`
  - The sample output is not produced by `QPMS.Win/Reports/Proje Raporu.repx`; that XML layout is a different `Project` report focused on failure modes.
  - The sample PDF text and section structure match the predefined DevExpress report class:
    - `QPMS.Module.Reports.Project_Report`
  - Matching sections confirmed from the sample output:
    - `DAILY RESULTS REPORT`
    - `Project Information`
    - `Project Locations`
    - `Project Part Numbers`
    - `Project Daily Summary`
    - `Project Daily Timesheet Report`
  - Before this recovery, project `XFDB13155` had `Report = NULL`, so no report template was assigned.

- Changes applied:
  - Inserted a new `QPMSReport` / `ReportDataV2` record with:
    - `Name: Project Report 2`
    - `Code: REPORT-00000023`
    - `ObjectTypeName: QPMS.Module.BusinessObjects.Project`
    - `PredefinedReportType: QPMS.Module.Reports.Project_Report`
    - `IsInplaceReport: 0`
    - `Company: Orca Turkiye`
  - Assigned the new report to:
    - `Project Code: XFDB13155`
    - `Project Name: Sondaj Sayim Isciligi - Arcelik Bolu(Mustafa Kacan)`

- Verification:
  - `QPMSReport` count increased to `2`.
  - `Project XFDB13155` now points to:
    - `Report Name: Project Report 2`
    - `Report Code: REPORT-00000023`
    - `Company: Orca Turkiye`

- Notes:
  - The upload was completed directly in the live QPMS database because the recovered layout already exists as the predefined `Project_Report` class in the application.
  - No application code or IIS deployment change was required for this report recovery step.

## 2026-04-24 13:15 Europe/Istanbul - QPMS report preview runtime fix

- Scope:
  - Investigated the live `Project Report 2` preview failure in `https://qpms.orerps.com/QPMSReport_ListView/`.
  - Fixed the missing DevExpress PDF runtime dependency and deployed the required binaries to the live site.

- Findings:
  - The preview failed with:
    - `Could not load file or assembly 'DevExpress.Pdf.v25.1.Drawing, Version=25.1.5.0'`
  - `QPMS.Web/bin` and the live site bin both contained report assemblies such as:
    - `DevExpress.XtraReports.v25.1.dll`
    - `DevExpress.XtraReports.v25.1.Web.dll`
    - `DevExpress.XtraReports.v25.1.Web.WebForms.dll`
  - But both bins were missing:
    - `DevExpress.Pdf.v25.1.Core.dll`
    - `DevExpress.Pdf.v25.1.Drawing.dll`
  - Root cause:
    - `DevExpress.XtraReports.v25.1.dll` references the DevExpress PDF assemblies, but `QPMS.Web.csproj` did not include them as direct references, so CopyLocal never placed them in the web app `bin`.

- Changes applied:
  - Added direct references in:
    - `QPMS.Web/QPMS.Web.csproj`
  - Added references:
    - `DevExpress.Pdf.v25.1.Core`
    - `DevExpress.Pdf.v25.1.Drawing`
  - Rebuilt `QPMS.Web` in `Release`.
  - Copied the resulting PDF assemblies to the live site:
    - `C:\Plesk Vhosts\orerps.com\qpms.orerps.com\bin\DevExpress.Pdf.v25.1.Core.dll`
    - `C:\Plesk Vhosts\orerps.com\qpms.orerps.com\bin\DevExpress.Pdf.v25.1.Drawing.dll`
  - Recycled the IIS application pool:
    - `qpms.orerps.com(domain)(4.0)(pool)`

- Verification:
  - `QPMS.Web` build completed with:
    - `0 Warning(s)`
    - `0 Error(s)`
  - Live site `bin` now contains:
    - `DevExpress.Pdf.v25.1.Core.dll` version `25.1.5.0`
    - `DevExpress.Pdf.v25.1.Drawing.dll` version `25.1.5.0`
  - `curl -k -I https://qpms.orerps.com/Login.aspx?ReturnUrl=%2f` returned:
    - `HTTP/1.1 200 OK`

- Notes:
  - A temporary local reference pool under `build/references/qpms-web` was recreated only for the Release build and then removed again.
  - The sample PDF file under `QPMS.Module/Reports/` remains an existing untracked repo artifact and was not modified by this runtime fix.

## 2026-04-24 13:23 Europe/Istanbul - Project report information visibility diagnosis

- Scope:
  - Investigated the follow-up complaint that project information was not appearing in the `Project Report 2` output.
  - Compared the report layout bindings with live `Project` data for both the sample project and the on-screen selected project.

- Findings:
  - The dedicated `Project Information` table exists in:
    - `QPMS.Module/Reports/ProjectReport.Designer.cs`
  - But it is placed on the main `Detail` band and that band is explicitly disabled:
    - `this.Detail.Visible = false;`
  - The first-page header still prints project fields directly from the root `Project` object:
    - `Code -> [Code]`
    - `Name -> [Name]`
    - `Start -> [Start]`
    - `End -> [End]`
    - `Branch Name -> [Branch].[Name]`
    - `Customer -> [Customer].[Code]`
  - This means there are two separate reasons a user can perceive "project information is missing":
    - the dedicated `Project Information` detail section is hidden by layout
    - some header fields are empty because the selected project data is actually null in the database

- Live data checks:
  - Sample project `XFDB13155`:
    - `Code`, `Name`, `Start`, `End`, and `Branch` are present
    - `Customer` is `NULL`
  - On-screen selected project `XFC013133` (`ZUIDBERG-91768524-REWORK-200471412`):
    - `Code`, `Name`, `Start`, `End`, and `Branch` are present
    - `Customer` is `NULL`

- Notes:
  - No code change was applied in this diagnosis step because there are two valid next fixes with different output consequences:
    - unhide the dedicated `Project Information` band
    - keep the current layout and treat the remaining blanks as source-data gaps

## 2026-04-24 13:29 Europe/Istanbul - Project Information section unhide and live deploy

- Scope:
  - Enabled the hidden `Project Information` section in `Project Report 2`.
  - Rebuilt the report assembly and deployed the updated module to the live QPMS site.

- Changes applied:
  - Updated:
    - `QPMS.Module/Reports/ProjectReport.Designer.cs`
  - Changed the report main detail band from hidden to visible:
    - `this.Detail.Visible = true;`
  - Rebuilt:
    - `QPMS.Module.csproj` in `Release`
  - Deployed to live site:
    - `C:\Plesk Vhosts\orerps.com\qpms.orerps.com\bin\QPMS.Module.dll`
    - `C:\Plesk Vhosts\orerps.com\qpms.orerps.com\bin\tr\QPMS.Module.resources.dll`
  - Recycled IIS app pool:
    - `qpms.orerps.com(domain)(4.0)(pool)`

- Verification:
  - `QPMS.Module.csproj` build completed with:
    - `0 Warning(s)`
    - `0 Error(s)`
  - Live `QPMS.Module.dll` SHA256 now matches the freshly built local module:
    - `F41088FBFFAFF89D3E71B3BB28AE0D8868652ADDB509C878C082797E7B5C759E`
  - `curl -k -I https://qpms.orerps.com/Login.aspx?ReturnUrl=%2f` returned:
    - `HTTP/1.1 200 OK`

- Notes:
  - This fix restores the dedicated `Project Information` layout section, but fields backed by null project data will still render empty.
  - Current live data checks showed `Customer` is null for both:
    - `XFDB13155`
    - `XFC013133`
