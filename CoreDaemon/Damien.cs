using System;

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

        public bool ExecuteCommands(string[] args)
        {
            
        }
    }
}
