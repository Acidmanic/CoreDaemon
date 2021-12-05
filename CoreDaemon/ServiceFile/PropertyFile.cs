using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CoreDaemon.ServiceFile
{
    public class PropertyFile
    {
        private readonly Dictionary<string, List<KeyValuePair<string, string>>> _content;

        public PropertyFile()
        {
            _content = new Dictionary<string, List<KeyValuePair<string, string>>>();
        }


        public void Write(string sessionName, string name, string value,bool overwrite = false)
        {
            var session = GetSession(sessionName);

            if (overwrite)
            {
                var removing = session.Where(p => p.Key.Equals(name, StringComparison.OrdinalIgnoreCase));
                
                removing.ToList().ForEach( r => session.Remove(r));
            }
                
            session.Add(new KeyValuePair<string, string>(name,value));
        }


        public void Save(string filePath)
        {
            var lines = new List<string>();

            foreach (var session in _content.Keys)
            {
                lines.Add("[" + session + "]");

                foreach (var record in _content[session])
                {
                    lines.Add(record.Key + "=" + record.Value);
                }

                lines.Add("");
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.WriteAllLines(filePath, lines);
        }

        private List<KeyValuePair<string,string>> GetSession(string sessionName)
        {
            foreach (var name in _content.Keys)
            {
                if (name.Equals(sessionName, StringComparison.OrdinalIgnoreCase))
                {
                    return _content[name];
                }
            }

            var session = new List<KeyValuePair<string, string>>();

            _content.Add(sessionName, session);

            return session;
        }
    }
}