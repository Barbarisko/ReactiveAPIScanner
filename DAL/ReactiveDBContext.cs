using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ReactiveDBContext : DbContext
    { 
        public DbSet<File> Files { get; set; }

        public ReactiveDBContext() : base("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ReactiveDB;Integrated Security=True")
        { 
        
        }
    }
}
