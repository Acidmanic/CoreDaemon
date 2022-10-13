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
        private static readonly object Locker = new object();
        private static Damien _instance = null;

        private Damien()
        {
            UseServiceUnitFile();
        }

        public static Damien Summon()
        {
            lock (Locker)
            {
                if (_instance == null)
                {
                    _instance = new Damien();
                }
            }

            return _instance;
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
                    _serviceProvider = new DotnetCoreWebAppServiceInfoProvider();
                    
                    string command = args[1].ToLower();

                    if (command == "help")
                    {
                        var defaultMethod = GetNameForUsingMethod();
                        
                        Console.WriteLine("help: Prints this.\n" +
                                          "install: will install a daemon on current system.\n" +
                                          "uninstall: will remove any installed service for this assembly from current system.\n\n" +
                                          "* For writing service files into init.d directory, use 'init-rc' argument with install and uninstall commands." +
                                          "In a same way, using the 'unit' argument will force to install/uninstall regarding ServiceUnit" +
                                          " files (/etc/systemd/system/). If neither of these arguments are present, then the default " +
                                          "method: ("+defaultMethod+") will be used.");
                        return ExecutionResult.Handled;
                    }

                    
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

        private string GetNameForUsingMethod()
        {
            var application = GetApplicationInfo();

            var service = _serviceProvider.ProvideServiceFor(application);

            var installer = _serviceInstallerFactory(service);

            if (installer is InitRcServiceInstaller)
            {
                return "Init-Rc Files";
            }

            if (installer is ServiceUnitServiceInstaller)
            {
                return "Service Units";
            }

            return "Default?!";
        }

        private ServiceInstallerBase CreateInstaller(string[] args, ServiceInfo info)
        {
            foreach (var arg in args)
            {
                if ("init-rc".Equals(arg, StringComparison.OrdinalIgnoreCase))
                {
                    return new InitRcServiceInstaller(info);
                }
                if ("unit".Equals(arg, StringComparison.OrdinalIgnoreCase))
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

        public Damien UseInitRc()
        {
            _serviceInstallerFactory = s => new InitRcServiceInstaller(s);

            return this;
        }

        public Damien UseServiceUnitFile()
        {
            _serviceInstallerFactory = s => new ServiceUnitServiceInstaller(s);

            return this;
        }
    }
}