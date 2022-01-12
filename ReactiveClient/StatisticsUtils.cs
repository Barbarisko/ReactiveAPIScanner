using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveClient
{
    public class StatisticsUtils
    {
        private static StatisticsUtils instance;
        Dictionary<string, int> ExtensionStats;

        private StatisticsUtils() {
            ExtensionStats = new Dictionary<string, int>();
        }

        public static StatisticsUtils getInstance()
        {
            if (instance == null)
                instance = new StatisticsUtils();
            return instance;
        }

        public Dictionary<string, int> GetFileLanguageStats(string filename)
        {
            string file_ext = filename.Split(".").Last();

            if (ExtensionStats.ContainsKey(file_ext))  
            {
                int value = ExtensionStats[file_ext];
                ExtensionStats[file_ext] = value + 1;
            }
            else
            {
                ExtensionStats.Add(file_ext, 1);     
            }
            return ExtensionStats;
        }

        public void PrintLanguageStats()
        {
            Console.WriteLine();
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Most revealed file extensions:");
            var ordered = ExtensionStats.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            foreach (KeyValuePair<string, int> kvp in ordered)
            {
                Console.WriteLine($"{kvp.Key} indexed {kvp.Value} times");
            }
            Console.ReadKey();
        }
    }
}
