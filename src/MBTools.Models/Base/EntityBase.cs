using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MBTools.Models.Base
{
    public class EntityBase
    {
        /// <summary>
        /// Gets/sets the unique id.
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}