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
                RunningInstanceName = applicationInfo.ApplicationName,
                KillAllInstancesCommand = $"killall -9 {applicationInfo.ApplicationName}",
                RunInBackgroundCommand = $"(cd {applicationInfo.BinaryDirectory} && (./{applicationInfo.ApplicationName} &))"
            };
        }
    }
}