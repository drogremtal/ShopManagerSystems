using DataLayer.DataLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ApplicationDBContext:DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserInformation> UserInformation { get; set; }
        public DbSet<Check> Check{ get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
