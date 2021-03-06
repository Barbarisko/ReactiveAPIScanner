using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveClient
{
    public class ApiKeyProducer : IObservable<File>, IDisposable
    {
        private readonly List<IObserver<File>> subscriberList = new List<IObserver<File>>();
        //the cancellation token source for starting stopping inner observable working thread
        private readonly CancellationTokenSource cancellationSource;
        //the cancellation flag
        private CancellationToken cancellationToken;
        //the running task that runs the inner running thread
        private Task workerTask;

        private StatisticsUtils statistics; 
        private ReactiveDBContext context;

        public ApiKeyProducer(ReactiveDBContext context)
        {
            cancellationSource = new CancellationTokenSource();
            cancellationToken = cancellationSource.Token;
            workerTask = Task.Factory.StartNew(OnInnerWorker, cancellationToken);
            this.context = context;
            statistics = StatisticsUtils.getInstance();
        }

        //add another observer to the subscriber list
        public IDisposable Subscribe(IObserver<File> observer)
        {
            if (subscriberList.Contains(observer))
                throw new ArgumentException("The observer is already subscribed to this observable");

            DisplayUtils.SendSystemMessage(string.Format("Subscribing for {0}", observer.GetHashCode()), ConsoleColor.DarkYellow);
            subscriberList.Add(observer);

            return null;
        }

        //this code executes the observable infinite loop
        //and routes messages to all observers on the valid
        //message handler
        private async void OnInnerWorker()
        {

            var fileContentGetter = new RequestLib.FileContentGetter();
            var fileComparer = new FileComparer(context);
            var g = new RequestLib.KeyWordSearcher();

            DisplayUtils.SendSystemMessage("If you need to exit, type EXIT. " +
                    "\nPlease specify the keyword for search: ", ConsoleColor.DarkYellow);
                var input = Console.ReadLine();
            while (!cancellationToken.IsCancellationRequested)
            { 
                Console.WriteLine();

                try
                {
                    foreach (var observer in subscriberList)
                        if (string.IsNullOrEmpty(input))
                            break;
                        else if (input.Equals("EXIT"))
                        {
                            cancellationSource.Cancel();
                            break;
                        }
                        else
                        {
                            var res = await g.SearchRepositories(input);
                            var resFiles = g.ParseSearchResponce(input, res);
                            
                            foreach (var a in resFiles)
                            {
                                a.text = await fileContentGetter.GetFileContent(a);
                            }

                            var filteredFiles = fileComparer.GetNewContent(resFiles);

                            PreviousResults(input);

                            Console.WriteLine("\nNew results:\n");

                            if (filteredFiles.Count() > 0)
                            {
                                foreach (var a in filteredFiles)
                                {
                                    observer.OnNext(a);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No new results.\n");
                            }

                            statistics.PrintLanguageStats(context);
                        }
                    cancellationToken.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException)
                {
                    DisplayUtils.SendSystemMessage($"Task ended, no more files to search.", ConsoleColor.DarkYellow);

                    cancellationToken = cancellationSource.Token;
                    workerTask = Task.Factory.StartNew(OnInnerWorker, cancellationToken);

                    Console.ReadKey();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\n{e.Message}");
                    Console.ReadKey();
                }
            //}
                     
        }
            

        public void PreviousResults(string keyword)
		{
            Console.WriteLine("\nPreviously found results by this keyword:\n");

            List<SearchResults> previousResults = context.Results
                    .Where(f => f.KeyWord == keyword).ToList();

            if (previousResults.Count() > 0)
			{
                foreach (var p in previousResults)
                {
                    Console.WriteLine("Checked file: " + p.FileName + " Found " + p.NumOfKeys + " Keys");
					foreach (var key in p.SearchResult)
					{
						Console.WriteLine(key.KeyString);
					}
				}
            }
			else
			{
                Console.WriteLine("No previous results.");
            }
        }

        //cancel main task and ack all observers
        //by sending the OnCompleted message
        public void Dispose()
        {
            if (!cancellationSource.IsCancellationRequested)
            {
                cancellationSource.Cancel();
                while (!workerTask.IsCanceled)
                    Thread.Sleep(100);
            }

            cancellationSource.Dispose();
            workerTask.Dispose();

            foreach (var observer in subscriberList)
                observer.OnCompleted();
        }

        //wait until the main task completes or went cancelled
        public void Wait()
        {
            while (!(workerTask.IsCompleted || workerTask.IsCanceled))
                Thread.Sleep(50);
        }

       
    }
}
