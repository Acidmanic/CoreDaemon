using System;
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
                        Console.WriteLine("help: Prints this.\ninstall: will install an init.d daemon on current system." +
                                          "\nuninstall: will remove any installed service for this assembly from current system.");
                        return ExecutionResult.Handled;
                    }
                    _serviceProvider = new DotnetCoreWebAppServiceInfoProvider();
                    
                    if (command == "install")
                    {
                        Install();
                    }

                    if (command == "uninstall")
                    {
                        UnInstall();
                    }
                }
                
            }
            return ExecutionResult.NoActionTaken;
        }


        private void Install()
        {
            var application = GetApplicationInfo();

            var service = _serviceProvider.ProvideServiceFor(application);
            
            new ServiceInstaller(service).InstallService();
        }

        private void UnInstall()
        {
            var application = GetApplicationInfo();

            var service = _serviceProvider.ProvideServiceFor(application);
            
            new ServiceInstaller(service).InstallService();
        }

        
        public ApplicationInfo GetApplicationInfo()
        {
            var assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }
            return new ApplicationInfo
            {
                ApplicationName = new FileInfo(assembly.CodeBase).Name,
                BinaryDirectory = new FileInfo(assembly.CodeBase).Directory?.FullName,
                ServiceName = new FileInfo(assembly.CodeBase).Name.ToLower()
            };
        }
    }
}
