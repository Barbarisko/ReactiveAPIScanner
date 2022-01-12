using DAL;
using System;
using System.Threading.Tasks;

namespace ReactiveClient
{
    class Program
    {
        //To complete the example, here there is the program Main:
        static async Task Main(string[] args)
        {
            using var context = new ReactiveDBContext();
            //this is the message observable responsible of producing messages
            using (var observer = new ApiKeyProducer(context))
            //those are the message observer that consume messages
            using (var consumer1 = observer.Subscribe(new FileConsumer(context)))
                observer.Wait();

            Console.WriteLine("END");
            Console.ReadLine();
        }
    }
}
