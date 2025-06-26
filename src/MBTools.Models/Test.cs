using MBTools.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MBTools.Models
{
    public class Test : EntityBase
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = "No name";

        /// <summary>
        /// Timestamp
        /// </summary>
        public string? Timestamp { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Deltatime
        /// </summary>
        public string? DeltaTime { get; set; }
    }
}