using System;
using System.Diagnostics;
using System.IO;
using CoreDaemon.Models;
using Microsoft.Extensions.Logging;

namespace CoreDaemon
{
    public abstract class ServiceInstallerBase
    {
        private readonly ILogger _logger;
        protected ServiceInfo ServiceInfo { get; }

        public ServiceInstallerBase(ServiceInfo serviceInfo, ILogger logger)
        {
            ServiceInfo = serviceInfo;
            _logger = logger;
        }

        public abstract bool IsServiceInstalled();

        public abstract void InstallService();

        public void InstallServiceIfNot()
        {
            if (!this.IsServiceInstalled())
            {
                this.InstallService();
            }
        }

        public abstract void TryUninstallService();

        protected void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);

                    _logger.LogDebug(path + "Has been DELETED.");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error deleting service file: {Path},\n{Exception}",
                        path, e);
                }
            }
        }

        protected void Execute(string command, params string[] args)
        {
            try
            {
                var arguments = "";
                var sep = "";

                foreach (var arg in args)
                {
                    arguments += sep + arg;

                    sep = " ";
                }

                Process.Start(command, arguments);
            }
            catch (Exception e)
            {
                var fullCommand = command + (args == null ? "" : " " + string.Join(" ", args));

                _logger.LogError(e, "Error executing {}.\n{Exception}",fullCommand, e);
            }
        }

        protected void ReloadServices()
        {
            Execute("systemctl", "daemon-reload");
        }

        protected void StopService()
        {
            Execute("systemctl", "stop", ServiceInfo.Name);
        }

        protected void StartService()
        {
            Execute("systemctl", "start", ServiceInfo.Name);
        }

        protected void EnableService()
        {
            Execute("systemctl", "enable", ServiceInfo.Name);
        }

        protected void DisableService()
        {
            Execute("systemctl", "disable", ServiceInfo.Name);
        }
    }
}