namespace CoreDaemon.Models
{
    public class ServiceInfo
    {
        public string Name { get; set; }
        public string RunInBackgroundCommand { get; set; }
        
        public string KillAllInstancesCommand { get; set; }
        
        public string RunningInstanceName { get; set; }
        
        public ApplicationInfo Application { get; set; }
        
    }
}