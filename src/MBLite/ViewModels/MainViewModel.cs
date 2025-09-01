using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using CsvHelper.Configuration;
using MBLite.Models.Csv;
using MBLite.Services;
using MBLite.ViewModels.Connection;
using MBLite.Views;
using Microsoft.Extensions.Logging;

namespace MBLite.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    // Services
    private readonly IApplicationService _applicationService;
    private readonly IFileService _fileService;
    private readonly ILogger<MainViewModel> _logger;

    /// <summary>
    /// Поле для дочернего ViewModel с автоматическим созданием свойства
    /// Атрибут [ObservableProperty] создает свойство ConnectionViewModel
    /// с уведомлениями об изменении
    /// </summary>
    [ObservableProperty]
    private ConnectionViewModel _connection;


    private readonly List<RecordRegister> _recordRegisters = new List<RecordRegister>();

    [ObservableProperty]
    private int _currentRowAddress = 12345;

    [ObservableProperty]
    private Progress<double> _progress = new Progress<double>();
    [ObservableProperty]
    private int _uploadProgress = 0;


    public MainViewModel()
    {
    }

    public MainViewModel(
        IApplicationService applicationService,
        IFileService fileService,
        ILogger<MainViewModel> logger,
        ConnectionViewModel connection)
    {
        _applicationService = applicationService;
        _fileService = fileService;
        _logger = logger;
        _connection = connection;

        // Инициализация портов

        // Создаем экземпляр дочернего ViewModel
        
        // Подписываемся на события дочернего ViewModel (опционально)
        Connection.PropertyChanged += (s, e) =>
        {
            // Можно реагировать на изменения в дочернем ViewModel
            if (e.PropertyName == nameof(ConnectionViewModel.IsConnecting))
            {
                // Логика при изменении состояния подключения
            }
        };

        // Инициализируем команды
        InitializeCommands();

        // Логируем инициализацию
        _logger.LogInformation("MainViewModel инициализирован");
    }

    // Commands
    public IAsyncRelayCommand OpenFileCommand { get; private set; } = null!;
    public IAsyncRelayCommand SaveFileCommand { get; private set; } = null!;
    public IRelayCommand DownloadToCommand { get; private set; } = null!;
    public IRelayCommand UploadFromCommand { get; private set; } = null!;

    private void InitializeCommands()
    {
        OpenFileCommand = new AsyncRelayCommand(OpenFileActionAsync);
        SaveFileCommand = new AsyncRelayCommand(SaveFileActionAsync, SaveFileCanExecute);
        DownloadToCommand = new RelayCommand(DownloadTo, DownloadToCanExecute);
        UploadFromCommand = new RelayCommand(UploadFrom);
    }

    // For design time
    private void DownloadTo()
    {
        Status = "DownloadTo";
    }

    private bool DownloadToCanExecute()
    {
        //return Connection.IsConnecting;
        return true;
    }
    private void UploadFrom()
    {
        Status = "UploadFrom";
    }


    private async Task OpenFileActionAsync()
    {
        //var progress = new Progress<double>();
        Progress.ProgressChanged += (sender, args) =>
        {
            UploadProgress = (int)args;
        };
        if (_fileService is { })
        {
            await _fileService.OpenFileAsync(
                OpenFileCallbackAsync,
                new List<string>(new[] { "Csv", "Json", "All" }),
                "Open",
                Progress);
        }
    }

    private async Task OpenFileCallbackAsync(Stream stream, IProgress<double> progress)
    {
        IAsyncEnumerable<RecordRegister> records;
        var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            Delimiter = ";"
        };

        //using (var reader = new StreamReader("d:\\dev\\vs\\MBus\\materials\\test.csv"))
        using (var reader = new StreamReader(stream))

        using (var csv = new CsvReader(reader, csvConfig))
        {
            records = csv.GetRecordsAsync<RecordRegister>();

            double percentComplete;

            try
            {
                await foreach (var register in records)
                {
                    _recordRegisters.Add(register);
                    await Task.Delay(5); // Simulate long-running operation
                    percentComplete = (double)_recordRegisters.Count / 148 * 100;
                    progress?.Report(percentComplete);
                    CurrentRowAddress = register.Address;
                }
                _logger.LogInformation("Парсинг CSV!");

            }
            catch (CsvHelperException ex)
            {
                _logger.LogError(ex, "Ошибка парсинга CSV");
            }
        }
    }

    private async Task SaveFileActionAsync()
    {
        if (_fileService is { } && Status is { })
        {
            await _fileService.SaveFileAsync(
                SaveFileCallbackAsync,
                new List<string>(new[] { "Csv", "Json", "All" }),
                "Save as ...",
                Status ?? "ard3m",
                "json");
        }
    }

    private async Task SaveFileCallbackAsync(Stream stream)
    {
        if (_recordRegisters is null)
        {
            return;
        }
        using (var writer = new StreamWriter(stream))

        using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
        {
            await csv.WriteRecordsAsync(_recordRegisters);
        }
    }
    private bool SaveFileCanExecute()
    {
        return true;
    }

}