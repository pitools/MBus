using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using CsvHelper.Configuration;
using MBLite.Models.Connection;
using MBLite.Models.Csv;
using MBLite.Services;
using MBLite.ViewModels.Connection;
using MBLite.Views;

namespace MBLite.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private static bool _downloadToCanExecute = true;

    // Services
    private readonly IApplicationService _applicationService;

    private readonly List<RecordRegister> _recordRegisters = new List<RecordRegister>();

    // Commands
    private RelayCommand? _downloadToCommand;

    // Test fields
    private string _greeting = "Статус";
    private int _currentRowAddress = 12345;
    // Fields of the view models
    private Progress<double> _progress = new Progress<double>();
    //private Func<bool> _funcDownloadToCanExecute = DownloadToCanExecute;
    private RelayCommand? _uploadFromCommand;

    public MainViewModel()
    { }
    
    public MainViewModel(IApplicationService applicationService)
    {
        _applicationService = applicationService;

        OpenFileCommand = new AsyncRelayCommand(OpenFileActionAsync);
        SaveFileCommand = new AsyncRelayCommand(SaveFileActionAsync, SaveFileCanExecute);
        DownloadToCommand = new RelayCommand(DownloadTo, DownloadToCanExecute);
        UploadFromCommand = new RelayCommand(UploadFrom);

        TestConnectionCommand = new AsyncRelayCommand(TestConnectionActionAsync);

        //TestConnectionCommand = new AsyncRelayCommand(TestConnectionActionAsync);
        Connection = new();
        Connection.ComPorts = new ObservableCollection<string>(_applicationService.GetComPorts());

        if (Connection.ComPorts.Any<string>())
        {
            Connection.SelectedPort = Connection.ComPorts[0];
        }
    }

    public IRelayCommand DownloadToCommand { get; }

    public string? Greeting
    {
        get => _greeting;
        set => SetProperty(ref _greeting, value);
    }

    public int CurrentRowAddress
    {
        get => _currentRowAddress;
        set => SetProperty(ref _currentRowAddress, value);
    }

    // Properties
    public IAsyncRelayCommand OpenFileCommand { get; }

    public Progress<double>? Progress
    {
        get => _progress;
        set => SetProperty(ref _progress, value);
    }

    public IAsyncRelayCommand SaveFileCommand { get; }
    public IAsyncRelayCommand TestConnectionCommand { get; }
    public IRelayCommand UploadFromCommand { get; }
    // For design time

    private void DownloadTo()
    {
        Greeting = "DownloadTo";
    }

    private bool DownloadToCanExecute()
    {
        return Connection.Status;
    }

    private async Task OpenFileActionAsync()
    {
        //var progress = new Progress<double>();
        Progress.ProgressChanged += (sender, args) =>
        {
            Connection.Id = (int)args;
        };
        if (_applicationService is { })
        {
            await _applicationService.OpenFileAsync(
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
            await foreach (var register in records)
            {
                _recordRegisters.Add(register);
                await Task.Delay(5); // Simulate long-running operation
                percentComplete = (double)_recordRegisters.Count / 148 * 100;
                progress?.Report(percentComplete);
                CurrentRowAddress = register.Address; 
            }
        }
    }

    private async Task SaveFileActionAsync()
    {
        if (_applicationService is { } && Greeting is { })
        {
            await _applicationService.SaveFileAsync(
                SaveFileCallbackAsync,
                new List<string>(new[] { "Csv", "Json", "All" }),
                "Save as ...",
                Greeting ?? "ard3m",
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
    private async Task TestConnectionActionAsync()
    {
        Connection.Status = !Connection.Status;
        Connection.Id++;
        DownloadToCommand.NotifyCanExecuteChanged();
    }
    //private RelayCommand? openFileCommand;
    private void UploadFrom()
    {
        Greeting = "UploadFrom";
    }

    //private void OpenFile()
    //{
    //    if (_downloadToCanExecute)
    //    {
    //        _downloadToCanExecute = false;
    //        downloadToCommand.NotifyCanExecuteChanged();
    //    }
    //    else
    //    {
    //        _downloadToCanExecute = true;
    //        downloadToCommand.NotifyCanExecuteChanged();

    //    }
    //}
}

public class Foo
{
    public int Id { get; set; }
    public string Name { get; set; }
}