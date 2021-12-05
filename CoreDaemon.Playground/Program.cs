using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using CoreDaemon.Models;

namespace CoreDaemon.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine(Assembly.GetEntryAssembly()?.Location);
            
            Console.WriteLine(Assembly.GetEntryAssembly()?.CodeBase);
            
            Console.WriteLine(Process.GetCurrentProcess().MainModule?.FileName);
            
            Console.WriteLine(Assembly.GetEntryAssembly()?.FullName);
            
            Console.WriteLine(Assembly.GetEntryAssembly()?.EntryPoint?.Name);

            Console.WriteLine("-----------------------------------------------");

            ApplicationInfo info = Damien.Summon().GetApplicationInfo();

            Console.WriteLine("ApplicationName: " + info.ApplicationName);
            
            Console.WriteLine("BinaryDirectory: " + info.BinaryDirectory);
            
            Console.WriteLine("ServiceName: " + info.ServiceName);

            Console.WriteLine("-----------------------------------------------");

            if (Damien.Summon().ExecuteCommands(args) == ExecutionResult.NoActionTaken)
            {
                // Start sync (blocking) service here

                while (true)
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
