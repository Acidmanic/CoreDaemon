using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using CoreDaemon.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.LightWeight;

namespace CoreDaemon.Playground
{
    class Program
    {
        static void Main(string[] args)
        {

            var logger = new ConsoleLogger().EnableAll();

            Damien.Summon().UseLogger(logger);
            
            logger.LogDebug("Hello World!");

            logger.LogDebug(Assembly.GetEntryAssembly()?.Location);
            
            logger.LogDebug(Assembly.GetEntryAssembly()?.CodeBase);
            
            logger.LogDebug(Process.GetCurrentProcess().MainModule?.FileName);
            
            logger.LogDebug(Assembly.GetEntryAssembly()?.FullName);
            
            logger.LogDebug(Assembly.GetEntryAssembly()?.EntryPoint?.Name);

            logger.LogDebug("-----------------------------------------------");

            ApplicationInfo info = Damien.Summon().GetApplicationInfo();

            logger.LogDebug("ApplicationName: " + info.ApplicationName);
            
            logger.LogDebug("BinaryDirectory: " + info.BinaryDirectory);
            
            logger.LogDebug("ServiceName: " + info.ServiceName);

            logger.LogDebug("-----------------------------------------------");

            if (Damien.Summon().ExecuteCommands(args) == ExecutionResult.NoActionTaken)
            {
                // Start sync (blocking) service here

                while (true)
                {
                    var content =
                        "Im Am Alive: " +
                        DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt zz");

                    var path = Path.Combine("/","var", "log", "daemon.example.log");

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    File.WriteAllText(path,content);
                    
                    Thread.Sleep(1000);
                    
                }
            }
        }
    }
}
