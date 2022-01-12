using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public class SearchResults
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public virtual ICollection<Key> SearchResult { get; set; }
		public int NumOfKeys { get; set; }
		public string KeyWord { get; set; }

	}
}
