using MBTools.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace MBTools.Data
{
    //public class MBToolsContext : DbContext
    public class MBToolsContext : DbContext
    {

        public MBToolsContext(DbContextOptions<MBToolsContext> options)
        : base(options)
        {
        }

        public DbSet<Param> Params { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.EnableAutoHistory(null);
        }
    }







    //public MBToolsContext()
    //{
    //}

    //public MBToolsContext(DbContextOptions<MBToolsContext> options)
    //    : base(options)
    //{
    //}

    //public virtual DbSet<Param> Param { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        optionsBuilder.UseSqlite("DataSource=mbtools.db");
    //        optionsBuilder.EnableDetailedErrors(true);
    //        optionsBuilder.LogTo(message => Debug.WriteLine(message));

    //        //optionsBuilder.UseSqlite("Data Source=InMemorySample;Mode=Memory;Cache=Shared"); Edited
    //    }
    //}

    // Add-Migration InitialCreate -OutputDir EF/Migrations
    // Update-Database
}
