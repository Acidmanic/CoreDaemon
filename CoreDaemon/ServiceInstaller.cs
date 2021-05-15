using System;
using System.Diagnostics;
using System.IO;
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
            return File.Exists(this._serviceInfo.ScriptFileName);
        }

        public void InstallService()
        {
            TryUninstallService();
            
            string content = new InMemoryTemplate()
                .InitDTemplate(_serviceInfo.RunInBackgroundCommand,
                    _serviceInfo.KillAllInstancesCommand,
                    _serviceInfo.RunningInstanceName);
            
            File.WriteAllText(_serviceInfo.ScriptFileName,content);

            Process.Start("update-rc.d", $"{_serviceInfo.Name} default");
            
            Process.Start("update-rc.d", $"{_serviceInfo.Name} enable");
            
            Process.Start("systemctl", $"start {_serviceInfo.Name}.service");
            
            Process.Start("systemctl", $"status {_serviceInfo.Name}.service");
            
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
            try
            {
                Process.Start("systemctl", $"stop {_serviceInfo.Name}.service");
            }
            catch (Exception) { }
            
            try
            {
                Process.Start("update-rc.d", $"{_serviceInfo.Name} remove");
            }
            catch (Exception) { }
            
            try
            {
                if (File.Exists(_serviceInfo.ScriptFileName))
                {
                    File.Delete(_serviceInfo.ScriptFileName);
                }
            }
            catch (Exception e)
            { }
            try
            {
                Process.Start("systemctl", "daemon-reload");
            }
            catch (Exception) { }
        }
    }
}