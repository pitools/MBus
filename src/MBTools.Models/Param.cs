using MBTools.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MBTools.Models
{
    public class Param : EntityBase
    {
        /// <summary>
        /// Name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Sorting order
        /// </summary>
        public int? Order { get; set; } = 10;

        /// <summary>
        /// Sorting order
        /// </summary>
        public string? Type { get; set; } = "Integer";

        /// <summary>
        /// Device one to many realtion
        /// </summary>
        public int? DeviceId { get; set; }  // Optional foreign key property

        /// <summary>
        /// Device one to many realtion
        /// </summary>
        public Device? Device { get; set; }  // Optional reference navigation to principal



    }
}
