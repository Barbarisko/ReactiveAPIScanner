using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveClient
{
    public class IntegerConsumer : IObserver<string>
    {
        readonly int validDivider;
        //the costructor asks for a divider
        public IntegerConsumer(int validDivider)
        {
            this.validDivider = validDivider;
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

        public void OnNext(string value)
        {
            if (finished)
                OnError(new Exception("This consumer finished its lifecycle"));

            //the simple business logic is made by checking divider result
            else if (value != null)
                Console.WriteLine("{0}: {1} divisible by {2}", GetHashCode(), value, validDivider);
        }
        //Observable able to parse strings from the Console
        //and route numeric messages to all subscribers    
    }
}
