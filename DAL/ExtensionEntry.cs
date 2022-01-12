using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ExtensionEntry
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		
		public ExtensionEntry() { }
		public ExtensionEntry(string name)
		{
			Name = name;
			Quantity = 1;			
		}
	}
}
