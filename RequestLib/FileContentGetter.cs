using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLib
{
	class FileContentGetter : Requester
	{
		public FileContentGetter() : base()
		{
		}

		private async Task<string> GetFileContent(Path path)
		{
			var stringTask = client.GetStringAsync("https://raw.githubusercontent.com/" + path.full_repo_name + "/master/" + path.path_to_file);

			return await stringTask;
		}
	}
}
