namespace CoreDaemon.ServiceFile
{
    public class RestartType
    {
        private readonly string _value;

        public override string ToString()
        {
            return _value;
        }

        private RestartType(string value)
        {
            _value = value;
        }
        
        public static readonly RestartType No = new RestartType("no");
        public static readonly RestartType OnSuccess = new RestartType("on-success");
        public static readonly RestartType OnFailure = new RestartType("on-failure");
        public static readonly RestartType OnAbnormal = new RestartType("on-abnormal");
        public static readonly RestartType OnWatchDog = new RestartType("on-watchdog");
        public static readonly RestartType OnAbort = new RestartType("on-abort");
        public static readonly RestartType Always = new RestartType("always");
        
    }
}