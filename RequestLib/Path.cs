using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLib
{
	struct Path
	{
		public string full_repo_name, path_to_file;

		public Path(string full_name, string path)
		{
			full_repo_name = full_name;
			path_to_file = path;
		}
	}

}
