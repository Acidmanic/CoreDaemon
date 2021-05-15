using CoreDaemon.Models;

namespace CoreDaemon
{
    public class ServiceInstaller
    {
        private readonly ServiceInfo _serviceInfo;

        public ServiceInstaller(ServiceInfo serviceInfo)
        {
            _serviceInfo = serviceInfo;
        }
        public bool IsServiceInstalled()
        {
            
        }

        public void InstallService()
        {
            
        }

        public void InstallServiceIfNot()
        {
            if (!this.IsServiceInstalled())
            {
                this.InstallService();
            }
        }

        public void TryUninstallService()
        {
            
        }
    }
}