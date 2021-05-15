using System.IO;
using CoreDaemon.Contracts;
using CoreDaemon.Models;

namespace CoreDaemon.ServiceInfoProviders
{
    public class DotnetCoreWebAppServiceInfoProvider:IServiceInfoProvider
    {
        public ServiceInfo ProvideServiceFor(ApplicationInfo applicationInfo)
        {
            return new ServiceInfo
            {
                Name = applicationInfo.ServiceName,
                RunningInstanceName = applicationInfo.ApplicationName,
                KillAllInstancesCommand = $"killall -9 {applicationInfo.ApplicationName}",
                RunInBackgroundCommand = $"(cd {applicationInfo.BinaryDirectory} && (./{applicationInfo.ApplicationName} &))",
                ScriptFileName = Path.Combine("etc","init.d",applicationInfo.ServiceName)
            };
        }
    }
}