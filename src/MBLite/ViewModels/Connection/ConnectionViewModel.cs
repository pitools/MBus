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
using MBLite.Models;

namespace MBLite.ViewModels.Connection
{
    public partial class ConnectionViewModel : ViewModelBase
    {
        private readonly IApplicationService _applicationService;
        private readonly ILogger<ConnectionViewModel> _logger;

        [ObservableProperty]
        private ObservableCollection<string> _comPorts = new();
        [ObservableProperty]
        private string _selectedPort = string.Empty;
        [ObservableProperty]
        private bool _isConnecting;


        /// <summary>
        /// Настройки подключения - публичное свойство для привязки из View
        /// </summary>
        [ObservableProperty]
        private ConnectionSettings _connectionSettings = new();

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
            IApplicationService applicationService,
            ILogger<ConnectionViewModel> logger)
        {
            ComPorts = new ObservableCollection<string>(_applicationService.GetComPorts());
            if (ComPorts.Any<string>())
            {
                SelectedPort = ComPorts[0];
            }

            ConnectionSettings.Port = SelectedPort;

            _logger = logger;

            InitializeCommands();

            // Логируем инициализацию
            _logger.LogInformation("ConnectionViewModel инициализирован");

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
