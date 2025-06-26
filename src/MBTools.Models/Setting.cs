using MBTools.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MBTools.Models
{
    public class Setting : EntityBase
    {
        /// <summary>
        /// Name
        /// </summary>
        public required string Address { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Slave one to many realtion
        /// </summary>
        public int SlaveId { get; set; }  // Optional foreign key property

        /// <summary>
        /// Slave one to many realtion
        /// </summary>
        public required Slave Slave { get; set; }  // Optional reference navigation to principal


        /// <summary>
        /// One to many relation
        /// </summary>
        public ICollection<Register> Registers { get; } = new List<Register>(); // Collection navigation containing dependents
    }
}