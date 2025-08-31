using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MBLite.Services;
using Microsoft.Extensions.Logging;

namespace MBLite.ViewModels.Connection
{
    public partial class ConnectionViewModel : ViewModelBase
    {
        private readonly ILogger<ConnectionViewModel> _logger;

        [ObservableProperty]
        private string _port = "COM1";
        [ObservableProperty]
        private ObservableCollection<string> _comPorts = new();
        [ObservableProperty]
        private string _baudrate = "19200";
        [ObservableProperty]
        private int _unitId = 1;
        [ObservableProperty]
        private string _selectedPort = string.Empty;
        [ObservableProperty]
        private bool _isConnecting;

        //public string Port
        //{
        //    get => _port;
        //    set => SetProperty(ref _port, value);
        //}
        //public ObservableCollection<string> ComPorts
        //{
        //    get => _comPorts;
        //    set => SetProperty(ref _comPorts, value);
        //}

        //public string Baudrate
        //{
        //    get => _baudrate;
        //    set => SetProperty(ref _baudrate, value);
        //}
        //public int Id
        //{
        //    get => _id;
        //    set => SetProperty(ref _id, value);
        //}

        //public string SelectedPort
        //{
        //    get => _selectedPort;
        //    set => SetProperty(ref _selectedPort, value);
        //}

        public ConnectionViewModel()
        { }
        
        public ConnectionViewModel(
            ILogger<ConnectionViewModel> logger)
        {
            _logger = logger;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            //ConnectCommand = new AsyncRelayCommand(ConnectAsync, CanConnect);
            //CancelCommand = new RelayCommand(Cancel);
            //SelectRecentAddressCommand = new RelayCommand<string>(SelectRecentAddress);
        }




        //private async Task TestConnectionActionAsync()
        //{
        //    ConnectionStatus = !ConnectionStatus;
        //    OpenFileCommand.NotifyCanExecuteChanged();

        //    Connection.Id++;
        //    Connection = Connection;
        //}


    }
}
