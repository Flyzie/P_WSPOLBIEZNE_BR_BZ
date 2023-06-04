using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.IO;


namespace Data
{
    public class Logger
    {
        BlockingCollection<string> fifo;
        StreamWriter logFile;

        public Logger(string filename)
        {
            fifo = new BlockingCollection<string>();
            logFile = new StreamWriter(filename);
            Task.Run(logging);
        }

        private void logging()
        {
            foreach (string i in fifo.GetConsumingEnumerable())
            {
                logFile.WriteLine(i);
            }
        }

        public void log(string t) => fifo.Add(DateTime.Now.ToString("HH:mm:ss ") + t);


    }
}
