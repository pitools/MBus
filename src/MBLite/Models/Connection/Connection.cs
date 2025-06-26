using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MBLite.Models.Connection
{
    public class Connection
    {
        public string Port { get; set; }
        public string Baudrate { get; set; }
        public int? Id  { get; set; }
        public bool Status { get; set; }
    }
}