using System;
using System.Diagnostics;
using System.IO;
using CoreDaemon.Models;

namespace CoreDaemon
{
    public abstract class ServiceInstallerBase
    {
        protected ServiceInfo ServiceInfo { get; }

        public ServiceInstallerBase(ServiceInfo serviceInfo)
        {
            ServiceInfo = serviceInfo;
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

                    Console.WriteLine(path + "Has been DELETED.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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
                Console.WriteLine(e);
            }
        }

        protected void ReloadServices()
        {
            Execute("systemctl","daemon-reload");
        }

        protected void StopService()
        {
            Execute("systemctl","stop", ServiceInfo.Name);
        }
        
        protected void StartService()
        {
            Execute("systemctl","start", ServiceInfo.Name);
        }
        
        protected void EnableService()
        {
            Execute("systemctl","enable", ServiceInfo.Name);
        }
        
        protected void DisableService()
        {
            Execute("systemctl","disable", ServiceInfo.Name);
        }
    }
}