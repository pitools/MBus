using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MBLite.Models
{
    public partial class ConnectionSettings : ObservableObject
    {
        [ObservableProperty]
        private string _port = "COM1";
        [ObservableProperty]
        private string _baudrate = "19200";
        [ObservableProperty]
        private int _unitId = 12;
    }
}