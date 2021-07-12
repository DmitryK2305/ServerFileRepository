using Microsoft.EntityFrameworkCore;
using ServerFileRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public UserContext()
        {
        }

        public DbSet<User> Users { get; set; }

    }
}
