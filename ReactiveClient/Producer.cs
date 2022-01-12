using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveClient
{
    public class ApiKeyProducer : IObservable<File>, IDisposable
    {
        //the subscriber list
        private readonly List<IObserver<File>> subscriberList = new List<IObserver<File>>();

        //the cancellation token source for starting stopping
        //inner observable working thread
        private readonly CancellationTokenSource cancellationSource;

        //the cancellation flag
        private readonly CancellationToken cancellationToken;

        //the running task that runs the inner running thread
        private readonly Task workerTask;
        StatisticsUtils statistics; 

        ReactiveDBContext context;

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

            Console.WriteLine("Subscribing for {0}", observer.GetHashCode());
            subscriberList.Add(observer);

            return null;
        }

        //this code executes the observable infinite loop
        //and routes messages to all observers on the valid
        //message handler
        private async void OnInnerWorker()
        {
            var fileContentGetter = new RequestLib.FileContentGetter();
            var g = new RequestLib.KeyWordSearcher();
            //while (true)
            //{
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Please specify the keyword for search: ");
                        var input = Console.ReadLine();

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
                                var resFiles = g.ParseSearchResponce(res);

                                
                                foreach (var a in resFiles)
                                {
                                    a.text = await fileContentGetter.GetFileContent(a);
                                    observer.OnNext(a);
                                }

                            statistics.PrintLanguageStats();

                        }
                }
                    cancellationToken.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine($"Task ended, no more files to search.");
                    Console.ReadKey();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\n{e.Message}");
                    Console.ReadKey();
                }
            //}
            
            
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
                Thread.Sleep(100);
        }
    }
}
