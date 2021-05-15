namespace CoreDaemon
{
    public class ServiceInstaller
    {
        
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