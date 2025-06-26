using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using MBLite.Services;
using MBLite.ViewModels.Connection;

namespace MBLite.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    ConnectionViewModel? _connection;

    public ConnectionViewModel? Connection
    {
        get => _connection;
        set => SetProperty(ref _connection, value);
    }


}