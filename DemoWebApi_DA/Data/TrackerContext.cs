using DemoWebApi_DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi_DAL.Data
{
    public class TrackerContext : DbContext
    {
        public TrackerContext(DbContextOptions<TrackerContext> options) : base(options)
        {
            Database.EnsureCreated();

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
          
        }
        public DbSet<EntityProject> Projects { get; set; }
        public DbSet<EntityTask> Tasks { get; set; }
    }
}
