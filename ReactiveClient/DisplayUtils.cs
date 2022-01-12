using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveClient
{
    public static class DisplayUtils
    {
        public static void CreateColorLabel(string text)
        {
            Random r = new Random();
            Console.BackgroundColor = (ConsoleColor)r.Next(0, 16);
            Console.WriteLine($"{text}");
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public static void SendSystemMessage(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"{text}");
            Console.ResetColor();
        }
    }
}
