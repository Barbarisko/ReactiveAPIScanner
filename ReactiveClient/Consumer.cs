using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveClient
{
    public class FileConsumer : IObserver<RequestLib.Path>
    {
        RequestLib.FileContentGetter fileContentGetter;
        APIKeyFinder.PythonCaller fileProcessor;
        public FileConsumer()
        {
            fileContentGetter = new RequestLib.FileContentGetter();
            fileProcessor = new APIKeyFinder.PythonCaller();
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

        public async void OnNext(RequestLib.Path file)
        {
            if (finished)
                OnError(new Exception("This consumer finished its lifecycle"));

            List<APIKeyFinder.ScanResult> result;
            try
            {
                string text = await fileContentGetter.GetFileContent(file);
                result = fileProcessor.Scan(text);
            }
            catch(Exception e)
            {
                OnError(e);
                return;
            }

            if (result.Count() < 0)
            {
                Console.WriteLine("Checked file: " + file.name + " No Keys found!");

            }
            else
            {
                Console.WriteLine("Checked file: " + file.name + " Found " + result.Count() + " Keys");
                foreach(var key in result)
                {
                    Console.WriteLine(key.ToString());
                }
            }


        }    
    }
}
