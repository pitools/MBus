using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace MBTools.Data.SQLite
{
    [ExcludeFromCodeCoverage]
    public sealed class SQLiteDb : Db<SQLiteDb>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="options">Configuration options</param>
        public SQLiteDb(DbContextOptions<SQLiteDb> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlite("DataSource=mbtools.db");
        //        optionsBuilder.EnableDetailedErrors(true);
        //    }
        //    //base.OnConfiguring(optionsBuilder);
        //}
    }
}

// cd MBTools.Data.SQLite
// dotnet ef migrations add <NameOfMigration> -c SQLiteDb -o Migrations
// dotnet ef database update
