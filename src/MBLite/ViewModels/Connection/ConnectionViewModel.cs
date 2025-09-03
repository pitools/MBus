using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MBLite.Models;
using MBLite.Services;
using Microsoft.Extensions.Logging;

namespace MBLite.ViewModels.Connection
{
    public partial class ConnectionViewModel : ViewModelBase
    {
        private readonly IApplicationService _applicationService;
        private readonly ILogger<ConnectionViewModel> _logger;

        [ObservableProperty]
        private string _selectedPort = string.Empty;
        [ObservableProperty]
        private bool _isConnecting = false;
        [ObservableProperty]
        private string _port = "COM1";
        [ObservableProperty]
        private string _baudrate = "19200";

        // Коллекция доступных скоростей
        public ObservableCollection<BaudrateItem> AvailableBaudrates { get; } = new();

        // Коллекция доступных последовательных портов
        [ObservableProperty]
        private ObservableCollection<string> _comPorts = new();

        [ObservableProperty]
        private int _unitId = 12;

        public ConnectionViewModel()
        {
            IsConnecting = true;
        }

        public ConnectionViewModel(
            IApplicationService applicationService,
            ILogger<ConnectionViewModel> logger)
        {
            _applicationService = applicationService;
            _logger = logger;

            ComPorts = new ObservableCollection<string>(_applicationService.GetComPorts());

            if (ComPorts.Any<string>())
            {
                SelectedPort = ComPorts[0];
            }

            Port = SelectedPort;

            _logger = logger;

            InitializeCommands();

            // Логируем инициализацию
            _logger.LogInformation("ConnectionViewModel инициализирован");

        }


        // Commands
        public IAsyncRelayCommand TestConnectionCommand { get; private set; } = null!;
        private void InitializeCommands()
        {
            TestConnectionCommand = new AsyncRelayCommand(TestConnectionActionAsync);
        }

        private async Task TestConnectionActionAsync()
        {
            IsConnecting = !IsConnecting;
            UnitId++;
        }


    }
}
