using MBTools.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MBTools.Models
{
    public class Device : EntityBase
    {
        /// <summary>
        /// Gets/sets the manufacturer
        /// </summary>
        public required string Manufacturer { get; set; }

        /// <summary>
        /// Gets/sets the model
        /// </summary>
        public required string Model { get; set; }

        /// <summary>
        /// Gets/sets the order code
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// Gets/sets the hardware version
        /// </summary>
        public string? Hardware { get; set; }

        /// <summary>
        /// Gets/sets the firmware version
        /// </summary>
        public string? Firmware { get; set; }

        /// <summary>
        /// Gets/sets the description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// One to many relation
        /// </summary>
        public ICollection<Param> Params { get; } = new List<Param>(); // Collection navigation containing dependents

        /// <summary>
        /// One to many relation
        /// </summary>
        public ICollection<Register> Registers { get; } = new List<Register>(); // Collection navigation containing dependents
    }
}

