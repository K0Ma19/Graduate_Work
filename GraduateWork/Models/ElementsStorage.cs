using System.Data.Entity;

namespace WpfApp3.Models
{
    class ElementsStorage : DbContext
    {     
        public ElementsStorage() : base("DefaultConnection")
        {
            
        }
        public DbSet<Storage> Element { get; set; }

        public DbSet<Entrance> Entrances { get; set; }

        public DbSet<Remains> Remains { get; set; }

        public DbSet<Production> Productions { get; set; }

        public DbSet<Sale> Sales { get; set; }
    }
}
