using System;
using System.IO;

namespace CoreDaemon.ServiceFile
{
    public class ServiceUnitFile : PropertyFile
    {
        public ServiceUnitFile() : this("background-application")
        {
        }

        public ServiceUnitFile(string identifier)
        {
            UnitDescription("Application Description");
            ServiceWorkingDirectory(".");
            Restart(RestartType.Always);
            RestartSec(5);
            KillSignal("SIGINT");
            SyslogIdentifier(identifier);
            User(Environment.UserName);

            Write("Install", "WantedBy", "multi-user.target");
        }

        public ServiceUnitFile UnitDescription(string description)
        {
            Write("Unit", "Description", description, true);

            return this;
        }

        public ServiceUnitFile ServiceWorkingDirectory(string directoryPath)
        {
            var directory = new DirectoryInfo(directoryPath).FullName;

            Write("Service", "WorkingDirectory", directoryPath, true);

            return this;
        }

        public ServiceUnitFile ServiceExecStart(string filePath)
        {
            var path = new FileInfo(filePath).FullName;

            Write("Service", "ExecStart", filePath, true);

            return this;
        }

        public ServiceUnitFile Restart(RestartType restartType)
        {
            var type = restartType.ToString();

            Write("Service", "Restart", type, true);

            return this;
        }

        public ServiceUnitFile RestartSec(int seconds)
        {
            var value = seconds.ToString();

            Write("Service", "RestartSec", value, true);

            return this;
        }

        public ServiceUnitFile KillSignal(string signal)
        {
            Write("Service", "KillSignal", signal, true);

            return this;
        }

        public ServiceUnitFile SyslogIdentifier(string syslogIdentifier)
        {
            Write("Service", "SyslogIdentifier", syslogIdentifier, true);

            return this;
        }

        public ServiceUnitFile User(string user)
        {
            Write("Service", "User", user, true);

            return this;
        }
    }
}