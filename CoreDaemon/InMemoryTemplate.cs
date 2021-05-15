namespace CoreDaemon
{
    internal class InMemoryTemplate
    {
        public string InitDTemplate(string runInBackgroundCommand,
                                    string killRunningInstances,
                                    string runningProcessName)
        {
            return "#!/bin/sh\n" +
                   "### BEGIN INIT INFO\n" +
                   "# Provides:          cron\n" +
                   "# Required-Start:    $remote_fs $syslog $time\n" +
                   "# Required-Stop:     $remote_fs $syslog $time\n" +
                   "# Should-Start:      $network $named slapd autofs ypbind nscd nslcd winbind\n" +
                   "# Should-Stop:       $network $named slapd autofs ypbind nscd nslcd winbind\n" +
                   "# Default-Start:     2 3 4 5\n" +
                   "# Default-Stop:\n" +
                   "# Short-Description: Regular background program processing daemon\n" +
                   "# Description:       cron is a standard UNIX program that runs user-specified \n" +
                   "#                    programs at periodic scheduled times. vixie cron adds a \n" +
                   "#                    number of features to the basic UNIX cron, including better\n" +
                   "#                    security and more powerful configuration options.\n" +
                   "### END INIT INFO\n\n" +
                   "start() {\n" +
                   $"    {runInBackgroundCommand}\n" +
                   "}\n" +
                   "stop() {\n" +
                   $"    {killRunningInstances}\n" +
                   "}\n\n" +
                   "case \"$1\" in\n" +
                   "    start)\n" +
                   "        start\n" +
                   "        ;;\n" +
                   "    stop)\n" +
                   "        stop\n" +
                   "        ;;\n" +
                   "    restart)\n" +
                   "        stop\n" +
                   "        start\n" +
                   "        ;;\n" +
                   "    status)\n" +
                   $"        PIDS=$(pgrep {runningProcessName})\n" +
                   "         if [ ${#PIDS[@]} -eq 0 ]; then\n" +
                   "             exit 1;\n" +
                   "         else\n" +
                   "             exit 0;\n" +
                   "         fi\n" +
                   "         ;;\n" +
                   "    *)\n" +
                   "        echo \"Usage: $0 {start|stop|status|restart}\"\n" +
                   "esac\n" +
                   "\n" +
                   "exit 0";
        } 
    }
}