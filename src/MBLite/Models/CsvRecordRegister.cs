using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBLite.Models
{
    public class CsvRecordRegister
    {
        public int Address { get; set; }

        public string? Hex { get; set; }
        public string? Description { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        public int? Default { get; set; }
        public string? Unit { get; set; }
        public float? Scale { get; set; }
        public string? Access { get; set; }
        public RegisterType? Type { get; set; }
        public string? EnumSource { get; set; }
        public string? Enum { get; set; }
        public bool? Reserve { get; set; }
        public string? Category { get; set; }
    }
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