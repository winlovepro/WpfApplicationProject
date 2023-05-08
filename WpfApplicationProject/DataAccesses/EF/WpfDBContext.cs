using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplicationProject.DataAccesses.Configurations;
using WpfApplicationProject.DataAccesses.Entities;

namespace WpfApplicationProject.DataAccesses.EF
{
    public class WpfDBContext : DbContext
    {

        public WpfDBContext() : base("ConnectionString")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
        }

        public DbSet<User> Users { get; set; }
    }
}
