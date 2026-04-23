using Hangfire;
using Hangfire.SqlServer;
using Owin;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Hangfire.Dashboard;

namespace QPMS.Web
{
    public class Startup
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(QPMS.Module.ConnectionStringProvider.RequireConnectionString(), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer();
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard();

            // Let's also create a sample background job
            //BackgroundJob.Enqueue(() => Debug.WriteLine("Hello world from Hangfire!"));

            // ...other configuration logic
        }

        public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                //PermissionPolicyUser user = (PermissionPolicyUser) SecuritySystem.CurrentUser;

                //var userRoles = user.Roles;

                //foreach(var item in userRoles)
                //{
                //    if (item.Name == "Administrator")
                //        return true;

                //}
                //return false;
                ////if (HttpContext.Current.User.IsInRole("Admin"))
                ////    return true;

                ////return false;
                return true;
            }
        }
    }
}
