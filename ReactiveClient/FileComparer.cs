using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace ReactiveClient
{
	public class FileComparer
	{
		ReactiveDBContext context;

		public FileComparer(ReactiveDBContext context)
		{
			this.context = context;
		}


		public List<File> GetNewContent(List<File> allFilesWithKeyWord)
		{

			var keyWord = allFilesWithKeyWord[0].keyword;

			List<File> oldFilesWithKeyWord = context.Files
					.Where(f => f.keyword == keyWord).ToList();

			var listToDB = new List<File>(allFilesWithKeyWord);
			var loopList = new List<File>(allFilesWithKeyWord);

			if (oldFilesWithKeyWord.Count > 0)
			{
				foreach (var a in loopList)
				{
					foreach (var o in oldFilesWithKeyWord)
					{
						//check if it's the same file
						if (CheckIfFilesHaveSamePath(o, a))
						{
							listToDB.Remove(a);

							//check if content is same
							if (string.Equals(o.text, a.text))
							{
								allFilesWithKeyWord.Remove(a);
							}
							else
							{
								var fileWithUpdatedContent = context.Files.Find(o.id);
								fileWithUpdatedContent.text = a.text;
								context.SaveChanges();
							}
						}
					}
				}
			}

			foreach (var f in listToDB)
			{
				context.Files.Add(f);
			}

			context.SaveChanges();

			return allFilesWithKeyWord;			
		}

		public bool CheckIfFilesHaveSamePath(File file1, File file2)
		{
			return file1.full_repo_name + file1.path_to_file == file2.full_repo_name + file2.path_to_file;
		}

	}
}
