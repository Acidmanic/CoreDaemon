namespace CoreDaemon.ServiceFile
{
    public class DotnetCoreServiceUnitFile:ServiceUnitFile
    {
        public DotnetCoreServiceUnitFile(string identifier, string environment):base(identifier)
        {
            Write("Service","Environment","ASPNETCORE\\_ENVIRONMENT="+environment);
            Write("Service","Environment","DOTNET\\_PRINT\\_TELEMETRY\\_MESSAGE=false");
        }

        public DotnetCoreServiceUnitFile(string identifier):this(identifier,"Development")
        {
            
        }

        public DotnetCoreServiceUnitFile():this("background-application")
        {
            
        }
    }
}