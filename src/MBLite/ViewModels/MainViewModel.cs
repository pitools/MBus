using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using CsvHelper.Configuration;
using MBLite.Models;
using MBLite.Repositories;
using MBLite.Services;
using MBLite.ViewModels.Connection;
using MBLite.Views;
using Microsoft.Extensions.Logging;

namespace MBLite.ViewModels;

public partial class MainViewModel : ViewModelBase, IDisposable
{
    // Services
    private readonly IApplicationService _applicationService;
    private readonly IFileService _fileService;
    private readonly ILogger<MainViewModel> _logger;
    private readonly IRegisterRepository _registerRepository;

    private readonly EventHandler<double> _progressHandler;
    private bool _disposed;

    /// <summary>
    /// Поле для дочернего ViewModel с автоматическим созданием свойства
    /// Атрибут [ObservableProperty] создает свойство ConnectionViewModel
    /// с уведомлениями об изменении
    /// </summary>
    [ObservableProperty]
    private ConnectionViewModel _connectionView;

    [ObservableProperty]
    private ObservableCollection<CsvRecordRegister> _recordRegisters = new();
    //private readonly List<RecordRegister> _recordRegisters = new List<RecordRegister>();

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
        ConnectionViewModel connectionView,
        IRegisterRepository registerRepository)
    {
        _applicationService = applicationService;
        _fileService = fileService;
        _logger = logger;
        _connectionView = connectionView;
        _registerRepository = registerRepository;

        // Инициализация портов

        // Создаем экземпляр дочернего ViewModel

        // Подписываемся на события дочернего ViewModel (опционально)
        ConnectionView.PropertyChanged += (s, e) =>
        {
            // Можно реагировать на изменения в дочернем ViewModel
            if (e.PropertyName == nameof(ConnectionViewModel.IsConnected))
            {
                // Логика при изменении состояния подключения
                Status = ConnectionView.IsConnected ? "Подключено!" : "Не подключено";
            }
        };

        // Инициализируем команды
        InitializeCommands();

        _progressHandler = OnProgressChanged;
        Progress.ProgressChanged += _progressHandler;

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
        //return ConnectionView.IsConnecting;
        return true;
    }
    private void UploadFrom()
    {
        Status = "UploadFrom";
    }


    private async Task OpenFileActionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_fileService is { })
            {
                await _fileService.OpenFileAsync(
                    OpenFileCallbackAsync,
                    new List<string>(new[] { "Csv", "Json", "All" }),
                    "Open",
                    Progress,
                    cancellationToken);
            }
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "File operation was cancelled");
        }
        catch (CsvHelperException ex) // Ловим здесь исключение из колбэка
        {
            _logger.LogError(ex, "Ошибка при работе с файлом");
            // Уведомление пользователя
            // Например: ShowErrorDialog("Неверный формат файла");
        }
        catch (Exception ex) // Общий обработчик на всякий случай
        {
            _logger.LogError(ex, "Неизвестная ошибка при открытии файла");
            // Уведомление пользователя
            //throw; // Или не пробрасывать, если хотим обработать здесь
        }

    }

    private async Task OpenFileCallbackAsync(Stream stream, IProgress<double> progress, CancellationToken cancellationToken = default)
    {
            cancellationToken.ThrowIfCancellationRequested();
            var records = await _registerRepository.LoadFromCsvAsync(stream, progress, cancellationToken);
            RecordRegisters = new ObservableCollection<CsvRecordRegister>(records);
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

    private async Task SaveFileCallbackAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        if (RecordRegisters == null || !RecordRegisters.Any())
            return;

        await _registerRepository.SaveToCsvAsync(RecordRegisters, stream, cancellationToken);
    }

    private bool SaveFileCanExecute()
    {
        return true;
    }

    private async void OnProgressChanged(object sender, double progress)
    {
        try
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                UploadProgress = (int)progress;
            });
        }
        catch (TaskCanceledException)
        {
            // Обработка отмены
            _logger.LogInformation("Progress update was cancelled");
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            if (_progressHandler != null)
            {
                Progress.ProgressChanged -= _progressHandler;
            }
            _disposed = true;
        }
    }
}