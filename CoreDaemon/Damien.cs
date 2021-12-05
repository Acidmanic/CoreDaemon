using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using CoreDaemon.Contracts;
using CoreDaemon.Models;
using CoreDaemon.ServiceInfoProviders;

namespace CoreDaemon
{
    public class Damien
    {
        private static object locker = new object();
        private static Damien instance = null;

        private Damien()
        {
        }

        public static Damien Summon()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new Damien();
                }
            }

            return instance;
        }

        private IServiceInfoProvider _serviceProvider;
        private Func<ServiceInfo, ServiceInstallerBase> _serviceInstallerFactory;

        public ExecutionResult ExecuteCommands(string[] args)
        {
            if (args != null && args.Length > 1)
            {
                string callToDaemon = args[0].ToLower();

                if (callToDaemon == "daemon")
                {
                    string command = args[1].ToLower();

                    if (command == "help")
                    {
                        Console.WriteLine("help: Prints this.\n" +
                                          "install: will install a daemon on current system.\n" +
                                          "uninstall: will remove any installed service for this assembly from current system.\n\n" +
                                          "* to writing service files into init.d directory, use init-rc argument with install and uninstall commands." +
                                          "Otherwise a systemd/system/.service file will be created(/removed).");
                        return ExecutionResult.Handled;
                    }

                    _serviceProvider = new DotnetCoreWebAppServiceInfoProvider();
                    _serviceInstallerFactory = info => CreateInstaller(args, info);

                    if (command == "install")
                    {
                        try
                        {
                            Install();
                        }
                        catch (Exception)
                        {
                        }

                        return ExecutionResult.Handled;
                    }

                    if (command == "uninstall")
                    {
                        try
                        {
                            UnInstall();
                        }
                        catch (Exception)
                        {
                        }

                        return ExecutionResult.Handled;
                    }
                }
            }

            return ExecutionResult.NoActionTaken;
        }

        private ServiceInstallerBase CreateInstaller(string[] args, ServiceInfo info)
        {
            foreach (var arg in args)
            {
                if ("init-rc".Equals(arg, StringComparison.OrdinalIgnoreCase))
                {
                    return new InitRcServiceInstaller(info);
                }
            }

            return new ServiceUnitServiceInstaller(info);
        }


        private void Install()
        {
            var application = GetApplicationInfo();

            var service = _serviceProvider.ProvideServiceFor(application);

            var installer = _serviceInstallerFactory(service);

            installer.InstallService();
        }

        private void UnInstall()
        {
            var application = GetApplicationInfo();

            var service = _serviceProvider.ProvideServiceFor(application);

            var installer = _serviceInstallerFactory(service);

            installer.TryUninstallService();
        }


        public ApplicationInfo GetApplicationInfo()
        {
            var exeFile = Process.GetCurrentProcess().MainModule?.FileName;

            return new ApplicationInfo
            {
                ApplicationName = new FileInfo(exeFile).Name,
                BinaryDirectory = new FileInfo(exeFile).Directory?.FullName,
                ServiceName = new FileInfo(exeFile).Name.ToLower(),
                ExecutableFile = exeFile
            };
        }
    }
}