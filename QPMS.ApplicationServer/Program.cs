using System;
using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.MiddleTier;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using QPMS.Module.BusinessObjects;
using QPMS.Module.Win;

namespace QPMS.ApplicationServer {
    internal static class Program {
        [STAThread]
        private static void Main() {
            string connectionString = QPMS.Module.ConnectionStringProvider.RequireConnectionString();
            string serviceEndPoint = ConfigurationManager.AppSettings["ServiceEndpoint"];
            if(string.IsNullOrWhiteSpace(serviceEndPoint)) {
                throw new ConfigurationErrorsException("ServiceEndpoint is not configured.");
            }

            ValueManager.ValueManagerType = typeof(MultiThreadValueManager<>).GetGenericTypeDefinition();
            EnsureDatabaseCompatibility(connectionString);

            using(WcfXafServiceHost serviceHost = new WcfXafServiceHost(connectionString, CreateDataServerSecurityProvider)) {
                serviceHost.AddServiceEndpoint(typeof(IWcfXafDataServer), WcfDataServerHelper.CreateNetTcpBinding(), serviceEndPoint);
                serviceHost.Open();

                Console.WriteLine("QPMS middle tier server is running.");
                Console.WriteLine("Endpoint: " + serviceEndPoint);
                Console.WriteLine("Press Enter to stop.");
                Console.ReadLine();
            }
        }

        private static void EnsureDatabaseCompatibility(string connectionString) {
            using(ServerApplication serverApplication = new ServerApplication()) {
                serverApplication.ApplicationName = "QPMS";
                serverApplication.Modules.BeginInit();
                serverApplication.Modules.Add(new QPMSWindowsFormsModule());
                serverApplication.Modules.EndInit();
                serverApplication.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
                serverApplication.CreateCustomObjectSpaceProvider += (sender, e) =>
                    e.ObjectSpaceProvider = new XPObjectSpaceProvider(connectionString);
                serverApplication.ConnectionString = connectionString;
                serverApplication.Setup();
                serverApplication.CheckCompatibility();
            }
        }

        private static IDataServerSecurity CreateDataServerSecurityProvider() {
            AuthenticationStandard authentication = new AuthenticationStandard {
                LogonParametersType = typeof(AuthenticationStandardLogonParameters)
            };
            SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(Employee), typeof(EmployeeRole), authentication) {
                AllowAnonymousAccess = false,
                PermissionsReloadMode = PermissionsReloadMode.NoCache
            };
            return security;
        }
    }
}
