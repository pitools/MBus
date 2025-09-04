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
        private bool _isConnected;

        [ObservableProperty]
        private string _connectionStatus = "Disconnected";

        [ObservableProperty]
        private string _port = "COM1";

        [ObservableProperty]
        private BaudrateItem? _selectedBaudrate;

        // Коллекция доступных скоростей
        [ObservableProperty]
        private ObservableCollection<BaudrateItem> _availableBaudrates = new();

        // Коллекция доступных последовательных портов
        [ObservableProperty]
        private ObservableCollection<string> _comPorts = new();

        [ObservableProperty]
        private int _unitId = 12;

        // Вычисляемое свойство
        public string SelectedBaudrateInfo =>
            SelectedBaudrate != null ? $"{SelectedBaudrate.Value} baud ({SelectedBaudrate.Description})" : "Not selected";

        public ConnectionViewModel()
        {
            IsConnected = true;
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
            InitializeBaudrates();
            // Установка значения по умолчанию
            SelectedBaudrate = AvailableBaudrates.FirstOrDefault(b => b.Value == 9600);

            // Логируем инициализацию
            _logger.LogInformation("ConnectionViewModel инициализирован");

        }

        // Commands
        // Команда для обработки выбора скорости
        [RelayCommand]
        private void BaudrateSelected(BaudrateItem? selectedItem)
        {
            if (selectedItem != null)
            {
                Console.WriteLine($"Selected baudrate: {selectedItem.Value} - {selectedItem.Description}");
                // Дополнительная логика при выборе
            }
        }
        // Команда для подключения
        [RelayCommand]
        private void Connect()
        {
            if (SelectedBaudrate != null)
            {
                Console.WriteLine($"Connecting with {SelectedBaudrate.Value} baud");
                // Логика подключения
            }
        }

        public IAsyncRelayCommand TestConnectionCommand { get; private set; } = null!;
        private void InitializeCommands()
        {
            TestConnectionCommand = new AsyncRelayCommand(TestConnectionActionAsync);
        }
        private void InitializeBaudrates()
        {
            AvailableBaudrates.Add(new BaudrateItem(1200, "boy, this is really fast..."));
            AvailableBaudrates.Add(new BaudrateItem(2400, "the speed modem that every machine came with after 9600 was available..."));
            AvailableBaudrates.Add(new BaudrateItem(9600, "at last, fast enough!"));
            AvailableBaudrates.Add(new BaudrateItem(14400, "but then came..."));
            AvailableBaudrates.Add(new BaudrateItem(19200, "2 x 9 600"));
            AvailableBaudrates.Add(new BaudrateItem(28800, "fastest base signal rate on a phone line"));
            AvailableBaudrates.Add(new BaudrateItem(38400, "4 x 9 600, popular RS-232 and modem speed"));
            AvailableBaudrates.Add(new BaudrateItem(57600, "popular RS-232 speed"));
            AvailableBaudrates.Add(new BaudrateItem(115200, "rocket"));
        }

        private async Task TestConnectionActionAsync()
        {
            IsConnected = !IsConnected;
            UnitId++;
        }
    }
}
