using System;
using System.IO;

namespace SystemLogger
{
    public class Logger : LogBase, ILog
    {
        public void Write(string message)
        {
            using(var writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
