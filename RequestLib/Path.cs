using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLib { 
//{
//	public struct File
//	{
//		public string full_repo_name, path_to_file, name;
//		public string text;
//		public File(string full_name, string path, string name)
//		{
//			full_repo_name = full_name;
//			path_to_file = path;
//			this.name = name;
//			text = "";
//		}
//	}
	public class File
	{
		public string full_repo_name, path_to_file, name;
		public string text;
		public File(string full_name, string path, string name)
		{
			full_repo_name = full_name;
			path_to_file = path;
			this.name = name;
			text = "";
		}
	}

}
