using System;

namespace DAL
{
	public class File
	{
		public int id { get; set; }
		public string full_repo_name { get; set; }
		public string path_to_file { get; set; }
		public string name { get; set; }
		public string text { get; set; }
		public bool containsKey { get; set; }

		public File(string full_name, string path, string name)
		{
			full_repo_name = full_name;
			path_to_file = path;
			this.name = name;
			text = "";
		}
	}
}
