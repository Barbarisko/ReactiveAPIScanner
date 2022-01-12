using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveClient
{
    public class FileConsumer : IObserver<RequestLib.File>
    {
        APIKeyFinder.PythonCaller fileProcessor;
        StatisticsUtils statistics; 
        public FileConsumer()
        {
            fileProcessor = new APIKeyFinder.PythonCaller();
            statistics = StatisticsUtils.getInstance();
        }

        private bool finished = false;
        public void OnCompleted()
        {
            if (finished)
                OnError(new Exception("This consumer already finished it's lifecycle"));
            else
            {
                finished = true;
                statistics.PrintLanguageStats();
                Console.WriteLine("{0}: END", GetHashCode());
            }
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("{0}: {1}", GetHashCode(), error.Message);
        }

        public void OnNext(RequestLib.File file)
        {
            if (finished)
                OnError(new Exception("This consumer finished its lifecycle"));

            List<APIKeyFinder.ScanResult> result;
            try
            {
                result = fileProcessor.Scan(file.text);
            }
            catch(Exception e)
            {
                OnError(e);
                return;
            }

            if (result.Count() <= 0)
            {
                Console.WriteLine("Checked file: " + file.name + " No Keys found!");
            }
            else
            {
                statistics.GetFileLanguageStats(file.name);
                Console.WriteLine("Checked file: " + file.name + " Found " + result.Count() + " Keys");

                foreach (var key in result)
                {
                    Console.WriteLine(key.ToString());
                }

            }
        }
    }
}
