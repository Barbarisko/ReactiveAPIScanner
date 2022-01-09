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

        
        private async Task<string> SearchRepositories(string search_keyword)
        {
            var query_string = string.Format("https://api.github.com/search/code?q={0}", search_keyword);
            Console.WriteLine(query_string);

            var stringTask = client.GetStringAsync(query_string);
            var msg = await stringTask;

            return msg;
        }

        private List<Path> ParseSearchResponce(string msg)
		{
            var resList = new List<Path>();
            dynamic results = JObject.Parse(msg);

            foreach (var m in results.items)
            {
                resList.Add(new Path(m.repository.full_name, m.path));
            }

            return resList;
        }
    }
}

