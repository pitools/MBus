using MBTools.Data;
using MBTools.Data.Repository;
using MBTools.Data.SQLite;
using MBTools.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBTools.Services;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        // Рабочий кусок с MBToolsContext
        //collection.AddDbContext<MBToolsContext>(options => options.UseSqlite("DataSource=e:/Devs/.sqlite/db/mbtools/mbtoolsfactory.dev.db"));
        //collection.AddScoped<IRepoFactory, UnitOfWork<MBToolsContext>>();
        //collection.AddScoped<IUnitOfWork, UnitOfWork<MBToolsContext>>();
        //collection.AddScoped<IUnitOfWork<MBToolsContext>, UnitOfWork<MBToolsContext>>();
        ////collection.AddSingleton<IRepo, Repo>();
        ////collection.AddTransient<IUnitOfWork, UnitOfWork>();
        //collection.AddTransient<MainViewModel>();

        // Try SQLiteDb
        //collection.AddDbContext<SQLiteDb>(options => options.UseSqlite("Data Source=e:/Devs/.sqlite/db/mbtools/mbtoolsfactory.dev.db"));
        collection.AddDbContext<SQLiteDb>(options => options.UseSqlite("Data Source=e:/Devs/.sqlite/db/mbtools/mbtoolsfactory.dev.db"));
        collection.AddScoped<IRepoFactory, UnitOfWork<SQLiteDb>>();
        collection.AddScoped<IUnitOfWork, UnitOfWork<SQLiteDb>>();
        collection.AddScoped<IUnitOfWork<SQLiteDb>, UnitOfWork<SQLiteDb>>();
        //collection.AddSingleton<IRepo, Repo>();
        //collection.AddTransient<IUnitOfWork, UnitOfWork>();
        collection.AddTransient<MainViewModel>();
    }
}
//services.AddScoped<IRepositoryFactory, UnitOfWork<TContext>>();
// Following has a issue: IUnitOfWork cannot support multiple dbcontext/database, 
// that means cannot call AddUnitOfWork<TContext> multiple times.
// Solution: check IUnitOfWork whether or null
//services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
//services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();

//var path = System.Environment.GetEnvironmentVariable("USERPROFILE")
//var os = System.Environment.OSVersion;
//string conectionString = "DataSource=" + path + "\\mbtools.dev.db";
