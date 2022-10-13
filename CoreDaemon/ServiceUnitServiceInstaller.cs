using System.IO;
using CoreDaemon.Models;
using CoreDaemon.ServiceFile;
using Microsoft.Extensions.Logging;

namespace CoreDaemon
{
    public class ServiceUnitServiceInstaller : ServiceInstallerBase
    {
        private readonly string _scriptFileName;
        private readonly ILogger _logger;

        public ServiceUnitServiceInstaller(ServiceInfo serviceInfo, ILogger logger) : base(serviceInfo, logger)
        {
            _logger = logger;
            _scriptFileName = Path.Combine(new DirectoryInfo(".").Root.Name,
                "etc", "systemd", "system", serviceInfo.Name + ".service");
        }

        public override bool IsServiceInstalled()
        {
            return File.Exists(_scriptFileName);
        }

        public override void InstallService()
        {
            var service = new DotnetCoreServiceUnitFile(ServiceInfo.Application.ApplicationName);

            service.ServiceWorkingDirectory(ServiceInfo.Application.BinaryDirectory)
                .ServiceExecStart(ServiceInfo.Application.ExecutableFile)
                .SyslogIdentifier(ServiceInfo.Name)
                .UnitDescription("Background service for: " + ServiceInfo.Application.ApplicationName);

            service.Save(_scriptFileName);

            ReloadServices();

            EnableService();

            StartService();
        }

        public override void TryUninstallService()
        {
            StopService();

            DisableService();

            DeleteFile(_scriptFileName);

            ReloadServices();
        }
    }
}