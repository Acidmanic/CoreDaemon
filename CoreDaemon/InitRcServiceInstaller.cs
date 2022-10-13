using System;
using System.Diagnostics;
using System.IO;
using CoreDaemon.Models;
using Microsoft.Extensions.Logging;

namespace CoreDaemon
{
    public class InitRcServiceInstaller : ServiceInstallerBase
    {
        private readonly string _scriptFileName;
        private readonly ILogger _logger;
        public InitRcServiceInstaller(ServiceInfo serviceInfo, ILogger logger) : base(serviceInfo,logger)
        {
            _logger = logger;
            _scriptFileName = Path.Combine(new DirectoryInfo(".").Root.Name, "etc", "init.d", serviceInfo.Name);
        }

        public override bool IsServiceInstalled()
        {
            return File.Exists(_scriptFileName);
        }

        public override void InstallService()
        {
            TryUninstallService();

            string content = new InMemoryTemplate()
                .InitDTemplate(ServiceInfo.RunInBackgroundCommand,
                    ServiceInfo.KillAllInstancesCommand,
                    ServiceInfo.RunningInstanceName);

            File.WriteAllText(_scriptFileName, content);

            Execute("chmod", $"+x {_scriptFileName}");

            Execute("update-rc.d", $"{ServiceInfo.Name} default");

            Execute("update-rc.d", $"{ServiceInfo.Name} enable");

            Execute("systemctl", $"start {ServiceInfo.Name}.service");

            Execute("systemctl", $"status {ServiceInfo.Name}.service");
        }

        public override void TryUninstallService()
        {
            Execute("systemctl", $"stop {ServiceInfo.Name}.service");
            Execute("update-rc.d", $"{ServiceInfo.Name} remove");

            DeleteFile(_scriptFileName);
            Execute("systemctl", "daemon-reload");
        }
    }
}