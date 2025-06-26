#if DEBUG

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MBTools.Data.SQLite;

/// <summary>
/// Factory for creating a db context. Only used in dev mode
/// when creating migrations.
/// </summary>
//[NoCoverage]
public class DbFactory : IDesignTimeDbContextFactory<SQLiteDb>
{
    /// <summary>
    /// Creates a new db context.
    /// </summary>
    /// <param name="args">The arguments</param>
    /// <returns>The db context</returns>
    public SQLiteDb CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<SQLiteDb>();
        //builder.UseSqlite("Data Source=e:/Devs/.sqlite/db/mbtools/mbtoolsfactory.dev.db");
        builder.UseSqlite("Data Source=e:/Devs/.sqlite/db/mbtools/mbtoolsfactory.dev.db");
        return new SQLiteDb(builder.Options);
    }
}
#endif