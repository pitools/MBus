using CsvHelper.Configuration.Attributes;
using MBTools.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MBTools.Models
{
    public class Register : EntityBase
    {
        /// <summary>
        /// Address
        /// </summary>
        public required ushort Address { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Minimum value
        /// </summary>
        public short? Min { get; set; }

        /// <summary>
        /// Maximum value
        /// </summary>
        public short? Max { get; set; }

        /// <summary>
        /// Default value
        /// </summary>
        public short? Default { get; set; }

        /// <summary>
        /// Units
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// Scale factor
        /// </summary>
        public double? Scale { get; set; }

        /// <summary>
        /// The type of the register
        /// </summary>
        public RegisterType? Type { get; set; }

        /// <summary>
        /// Read, write or read and write
        /// </summary>
        public string? Access { get; set; }

        /// <summary>
        /// Device one to many realtion
        /// </summary>
        [Ignore]
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

    /// <summary>
    /// Type of register value
    /// </summary>
    public enum RegisterType
    {
        BOOL,
        BYTES,
        ENUM,
        WHIGH,
        WLOW,
        WORD
    }
}

