using System;
using System.IO;
using System.Threading.Tasks;

namespace SystemLogger
{
    public class AsyncLogger: LogBase, ILog
    {
        private readonly static object _locker = new object();

        public void Write(string message)
        {
            Task.Factory.StartNew(() => WriteTheMessage(message)); 
        }

        private static void WriteTheMessage(string message)
        {
            lock (_locker)
            {
                using (StreamWriter writer = File.AppendText(path))
                {
                    writer.WriteLine(message);
                }
            }
        }
    }
}
