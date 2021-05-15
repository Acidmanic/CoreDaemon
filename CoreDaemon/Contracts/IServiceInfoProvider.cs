using CoreDaemon.Models;

namespace CoreDaemon.Contracts
{
    public interface IServiceInfoProvider
    {


        ServiceInfo ProvideServiceFor(ApplicationInfo applicationInfo);
    }
}