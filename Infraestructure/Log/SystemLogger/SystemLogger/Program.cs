using System;

namespace SystemLogger
{
    class Program
    {
        static int MAX_COUNTER = 100000;
        static string dateFormat = "dd/MM/yyyy hh:mm:ss.fff";

        static void Main(string[] args)
        {
            var normalLogger = new Logger();
            var asyncLogger = new AsyncLogger();

            Logging(normalLogger);
            Logging(asyncLogger);

            Console.ReadLine();
        }

        private static void Logging(ILog logger)
        {
            string message = string.Empty;
            var counter = 0;
            
            Console.WriteLine("the process start : {0} of: {1}", DateTime.Now.ToString(dateFormat), logger.GetType().ToString());

            System.Threading.Thread.Sleep(50);

            for (counter = 0; counter < MAX_COUNTER; counter++)
            {
                message = string.Format("The Flow number: {0} of {1}", counter, logger.GetType().ToString());
                logger.Write(message);
            }

            System.Threading.Thread.Sleep(50);

            Console.WriteLine("the process end : {0} of: {1}", DateTime.Now.ToString(dateFormat), logger.GetType().ToString());
        }
    }
}
