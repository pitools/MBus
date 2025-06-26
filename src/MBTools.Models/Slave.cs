using MBTools.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MBTools.Models
{
    public class Slave : EntityBase
    {
        /// <summary>
        /// Name
        /// </summary>
        public required string Address { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Device one to many realtion
        /// </summary>
        public int? DeviceId { get; set; }  // Optional foreign key property

        /// <summary>
        /// Device one to many realtion
        /// </summary>
        public Device? Device { get; set; }  // Optional reference navigation to principal

        /// <summary>
        /// One to many relation
        /// </summary>
        public ICollection<Setting> Settings { get; } = new List<Setting>(); // Collection navigation containing dependents





    }
}