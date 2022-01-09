﻿using System;

namespace ReactiveClient
{
    class Program
    {
        //To complete the example, here there is the program Main:
        static void Main(string[] args)
        {
            //this is the message observable responsible of producing messages
            using (var observer = new ApiKeyProducer())
            //those are the message observer that consume messages
            using (var consumer1 = observer.Subscribe(new FileConsumer()))
                observer.Wait();

            Console.WriteLine("END");
            Console.ReadLine();
        }
    }
}
