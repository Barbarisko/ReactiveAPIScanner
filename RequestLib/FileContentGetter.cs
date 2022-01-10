using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RequestLib
{
	public class FileContentGetter : Requester
	{
		public FileContentGetter() : base()
		{
		}

		public async Task<string> GetFileContent(File path)
		{
			string text = "";

			try
            {
				string url = "https://raw.githubusercontent.com/" + path.full_repo_name + "/master/" + path.path_to_file;
				var stringTask = client.GetStringAsync(url);

				text = await stringTask;
			}
			catch(Exception e)
            {
				Console.WriteLine(e.Message);
            }
			return text;
		}
	}
}
