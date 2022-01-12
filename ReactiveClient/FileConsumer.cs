using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveClient
{
    public class FileConsumer : IObserver<File>
    {
        APIKeyFinder.PythonCaller fileProcessor;
        ReactiveDBContext context;

        public FileConsumer(ReactiveDBContext context)
        {
            fileProcessor = new APIKeyFinder.PythonCaller();
            this.context = context;
        }

        private bool finished = false;
        public void OnCompleted()
        {
            if (finished)
                OnError(new Exception("This consumer already finished it's lifecycle"));
            else
            {
                finished = true;
                Console.WriteLine("{0}: END", GetHashCode());
            }
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("{0}: {1}", GetHashCode(), error.Message);
        }

        public void OnNext(File file)
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
                var fileToUpdate = context.Files.Find(file.id);
                fileToUpdate.containsKey = true;
                context.SaveChanges();

                Console.WriteLine("Checked file: " + file.name + " Found " + result.Count() + " Keys");

                List<Key> results = new List<Key>();

                
                foreach(var key in result)
                {
                    results.Add(new Key() { KeyString = key.ToString()});
                    Console.WriteLine(key.ToString());
                }
                                
                context.Results.Add(new SearchResults() { FileName = file.name, NumOfKeys = result.Count(), 
                    SearchResult = new List<Key>(results), KeyWord = file.keyword });
                context.SaveChanges();
            }
        }    
    }
}
