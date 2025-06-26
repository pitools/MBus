using MBTools.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace MBTools.Data
{
    public abstract class Db<T> : DbContext, IDb where T : Db<T>
    {
        /// <summary>
        /// Gets/sets whether the db context as been initialized. This
        /// is only performed once in the application lifecycle.
        /// </summary>
        private static volatile bool IsInitialized = false;

        /// <summary>
        /// The object mutex used for initializing the context.
        /// </summary>
        private static readonly object Mutex = new object();

        public DbSet<Device> Devices { get; set; }
        public DbSet<Param> Params { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slave> Slaves { get; set; }
        public DbSet<Test> Tests { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="options">Configuration options</param>
        protected Db(DbContextOptions<T> options) : base(options)
        {
            var canConnect = Database.CanConnect();
            //if (canConnect) Database.Migrate();

            if (!IsInitialized && canConnect)
            {
                lock (Mutex)
                {
                    if (!IsInitialized)
                    {
                        //// Migrate database
                        //Database.Migrate();
                        // Seed
                        Seed();

                        IsInitialized = true;
                    }
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=mbtools.db");
                optionsBuilder.EnableDetailedErrors(true);
                optionsBuilder.EnableSensitiveDataLogging();
                // optionsBuilder.LogTo(message => Debug.WriteLine(message));

                //optionsBuilder.UseSqlite("Data Source=InMemorySample;Mode=Memory;Cache=Shared"); Edited
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.EnableAutoHistory(null);
        }

        /// <summary>
        /// Seeds the default data.
        /// </summary>
        private void Seed()
        {
            SaveChanges();

            Devices.Add(new Device
            {
                Manufacturer = "ARD3M",
                Model = "ARD3M K1 P100",
                Description = "Inetlectual drive protection",
                Firmware = "V1.0",
                Hardware = "V1.1",
                OrderCode = "12345",
                Params =
                {
                    new Param
                    {
                        Name = "Current",
                        Value= "100",
                        Description = "Rating current of device",
                        Order = 5
                    },
                    new Param
                    {
                        Name = "Voltage",
                        Value= "400",
                        Description = "Rating voltage of device",
                        Order = 2
                    },
                    new Param
                    {
                        Name = "Auxiliary power",
                        Value= "220",
                        Description = "Auxiliary power supply internal circuits",
                        Order = 3
                    },
                    new Param
                    {
                        Name = "Motor voltage",
                        Value= "380",
                        Description = "Rated voltage of the motor",
                        Order = 4
                    }

                }
            });

            var device = Devices.Where(dv => dv.Id == 1).FirstOrDefault();
            if (device != null)
            {
                device.Params.Add(new Param
                {
                    Name = "Max current",
                    Value = "120",
                    Description = "Maximum current of device",
                    Order = 12
                });
            }

            SaveChanges();
        }


    }
}
// Add-Migration InitialCreate -OutputDir EF/Migrations
// Update-Database

// Installing the tools
//dotnet ef can be installed as either a global or local tool.Most developers prefer installing dotnet ef as a global tool using the following command:
//.NET CLI
//dotnet tool install --global dotnet-ef

//To use it as a local tool, restore the dependencies of a project that declares it as a tooling dependency using a tool manifest file.
//Update the tool using the following command:
//.NET CLI
//dotnet tool update --global dotnet-ef

//Before you can use the tools on a specific project, you'll need to add the Microsoft.EntityFrameworkCore.Design package to it.
//.NET CLI
//dotnet add package Microsoft.EntityFrameworkCore.Design

// cd MBTools.Data
// and select default project to MBTools.Data
// dotnet ef migrations add NextMigration -o Migrations

// dotnet ef migrations add TimestampAdded -o Migrations
