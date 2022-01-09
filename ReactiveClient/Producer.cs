﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveClient
{
    public class ApiKeyProducer : IObservable<RequestLib.File>, IDisposable
    {
        //the subscriber list
        private readonly List<IObserver<RequestLib.File>> subscriberList = new List<IObserver<RequestLib.File>>();

        //the cancellation token source for starting stopping
        //inner observable working thread
        private readonly CancellationTokenSource cancellationSource;

        //the cancellation flag
        private readonly CancellationToken cancellationToken;

        //the running task that runs the inner running thread
        private readonly Task workerTask;

        public ApiKeyProducer()
        {
            cancellationSource = new CancellationTokenSource();
            cancellationToken = cancellationSource.Token;
            workerTask = Task.Factory.StartNew(OnInnerWorker, cancellationToken);
        }
        //add another observer to the subscriber list
        public IDisposable Subscribe(IObserver<RequestLib.File> observer)
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

            while (!cancellationToken.IsCancellationRequested)
            {
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
                        var file = resFiles[0];
                        file.text = await fileContentGetter.GetFileContent(file);
                        observer.OnNext(file);
                        //foreach(var f in resFiles)
                        //    observer.OnNext(f);
                    }
            }
            cancellationToken.ThrowIfCancellationRequested();
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
