using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveClient
{
    public class FileConsumer : IObserver<DAL.File>
    {
        APIKeyFinder.PythonCaller fileProcessor;
        ReactiveDBContext context;
        StatisticsUtils statistics; 

        public FileConsumer(ReactiveDBContext context)
        {
            fileProcessor = new APIKeyFinder.PythonCaller();
            this.context = context;
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
                statistics.PrintLanguageStats(context);
                Console.WriteLine("{0}: END", GetHashCode());
            }
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("{0}: {1}", GetHashCode(), error.Message);
        }

        public void OnNext(DAL.File file)
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
                file.containsKey = true;
                context.Files.Add(file);
                context.SaveChanges();

                ///// part for stats
                var extension = statistics.GetFileLanguage(file.name);
                statistics.AddLanguage(extension, context);
                ////////

                DisplayUtils.SendSystemMessage($"Checked file: {file.name} -  Found {result.Count()} Keys", ConsoleColor.Green);

                foreach (var key in result)
                {
                    Console.WriteLine(key.ToString());
                }
            }
        }
    }
}
