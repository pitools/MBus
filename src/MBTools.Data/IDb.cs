using MBTools.Models;
using Microsoft.EntityFrameworkCore;

namespace MBTools.Data
{
    /// <summary>
    /// Interface for the MBTools Db Context.
    /// </summary>
    public interface IDb : IDisposable
    {
        /// <summary>
        /// Gets/sets the device set
        /// </summary>
        DbSet<Device> Devices { get; set; }

        /// <summary>
        /// Gets/sets the param set
        /// </summary>
        DbSet<Param> Params { get; set; }

        /// <summary>
        /// Gets/sets the register set
        /// </summary>
        DbSet<Register> Registers { get; set; }

        /// <summary>
        /// Gets/sets the settings set
        /// </summary>
        DbSet<Setting> Settings { get; set; }

        /// <summary>
        /// Gets/sets the slave set
        /// </summary>
        DbSet<Slave> Slaves { get; set; }

        /// <summary>
        /// Gets/sets the test set
        /// </summary>
        DbSet<Test> Tests { get; set; }

        /// <summary>
        /// Gets the entity set for the specified type
        /// </summary>
        DbSet<T> Set<T>() where T : class;

        /// <summary>
        /// Saves the changes made to the context
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Saves the changes made to the context async
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}