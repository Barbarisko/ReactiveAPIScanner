using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;

namespace RequestLib
{
    public class KeyWordSearcher : Requester
    {
        public KeyWordSearcher() : base()
        {
        }

        
        public async Task<string> SearchRepositories(string search_keyword)
        {
            var query_string = string.Format("https://api.github.com/search/code?q={0}&s=indexed&o=desc", search_keyword);
            Console.WriteLine(query_string);

            var stringTask = client.GetStringAsync(query_string);
            var msg = await stringTask;

            return msg;
        }

        public List<File> ParseSearchResponce(string msg)
		{
            var resList = new List<File>();
            dynamic results = JObject.Parse(msg);

            foreach (var m in results.items)
            {
                string fname = m.repository.full_name;
                string path = m.path;
                string name = m.name;
                resList.Add(new File(fname, path, name));
            }

            return resList;
        }
    }
}

