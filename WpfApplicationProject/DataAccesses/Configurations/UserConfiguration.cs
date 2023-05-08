using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplicationProject.DataAccesses.Entities;

namespace WpfApplicationProject.DataAccesses.Configurations
{
    public class UserConfiguration: EntityTypeConfiguration<User> 
    {
        public UserConfiguration()
        {
            this.ToTable("Users");

            this.HasKey(x => x.Id);

            this.Property(x => x.Id).IsRequired().HasMaxLength(50).HasColumnType("varchar");

            this.Property(x => x.Firstname).IsRequired().HasMaxLength(50).HasColumnType("nvarchar");

            this.Property(x => x.Lastname).IsRequired().HasMaxLength(50).HasColumnType("nvarchar");

            this.Property(x => x.Username).IsRequired().HasMaxLength(50).HasColumnType("varchar");

            this.Property(x => x.Password).IsRequired().HasMaxLength(100).HasColumnType("nvarchar");

            this.Property(x => x.Active).IsRequired();
        }
    }
}
