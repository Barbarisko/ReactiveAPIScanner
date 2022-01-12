using DAL;
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
        public Dictionary<string, int> ExtensionStats;

        private StatisticsUtils() {
            ExtensionStats = new Dictionary<string, int>();
        }

        public static StatisticsUtils getInstance()
        {
            if (instance == null)
                instance = new StatisticsUtils();
            return instance;
        }

        public string GetFileLanguage(string filename)
        {         
            return filename.Split(".").Last();   
        }

        public void AddLanguage(string extension, ReactiveDBContext context)
        {
            var entry = context.ExtensionEntries
                    .Where(e => e.Name == extension)
                    .FirstOrDefault();

            if (entry != null)
                entry.Quantity += 1;
            else
                context.ExtensionEntries.Add(new ExtensionEntry(extension));
            context.SaveChanges();
        }

        public void PrintLanguageStats(ReactiveDBContext context)
        {
            Console.WriteLine();
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Most revealed file extensions:");
            var ordered = context.ExtensionEntries.OrderBy(x => x.Quantity).ToDictionary(x => x.Name, x => x.Quantity);

            foreach (KeyValuePair<string, int> kvp in ordered)
            {
                Console.WriteLine($"{kvp.Key} indexed {kvp.Value} times");
            }
            Console.ReadKey();
        }


        //public Dictionary<string, int> GetFileLanguageStats(string filename)
        //{
        //    string file_ext = filename.Split(".").Last();

        //    if (ExtensionStats.ContainsKey(file_ext))  
        //    {
        //        int value = ExtensionStats[file_ext];
        //        ExtensionStats[file_ext] = value + 1;
        //    }
        //    else
        //    {
        //        ExtensionStats.Add(file_ext, 1);     
        //    }
        //    return ExtensionStats;
        //}

    }
}
