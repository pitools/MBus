using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using Avalonia.VisualTree;
using NModbus;
using NModbus.Extensions.Enron;
using NModbus.IO;
using NModbus.Serial;

namespace MBLite.Services;

public class ApplicationService : IApplicationService
{
    private static FilePickerFileType All { get; } = new("All")
    {
        Patterns = new[] { "*.*" },
        MimeTypes = new[] { "*/*" }
    };

    private static FilePickerFileType Json { get; } = new("Json")
    {
        Patterns = new[] { "*.json" },
        AppleUniformTypeIdentifiers = new[] { "public.json" },
        MimeTypes = new[] { "application/json" }
    };

    private static FilePickerFileType Text { get; } = new("Text")
    {
        Patterns = new[] { "*.txt" },
        AppleUniformTypeIdentifiers = new[] { "public.text" },
        MimeTypes = new[] { "text/plain" }
    };

    private static FilePickerFileType Csv { get; } = new("Csv")
    {
        Patterns = new[] { "*.csv" },
        AppleUniformTypeIdentifiers = new[] { "public.csv" },
        MimeTypes = new[] { "text/csv" }
    };

    public void Exit()
    {
        throw new NotImplementedException();
    }

    public async Task OpenFileAsync(Func<Stream, IProgress<double>, Task> callback, List<string> fileTypes, string title, IProgress<double> progress)
    {
        var storageProvider = GetStorageProvider();
        if (storageProvider is null)
        {
            return;
        }

        var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = title,
            FileTypeFilter = GetFilePickerFileTypes(fileTypes),
            AllowMultiple = false
        });

        var file = result.FirstOrDefault();
        if (file is not null)
        {
            try
            {
#if NETFRAMEWORK
                using var stream = await file.OpenReadAsync();
#else
                await using var stream = await file.OpenReadAsync();
#endif
                await callback(stream, progress);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    public async Task SaveFileAsync(Func<Stream, Task> callback, List<string> fileTypes, string title, string fileName, string defaultExtension)
    {
        var storageProvider = GetStorageProvider();
        if (storageProvider is null)
        {
            return;
        }

        var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = title,
            FileTypeChoices = GetFilePickerFileTypes(fileTypes),
            SuggestedFileName = fileName,
            DefaultExtension = defaultExtension,
            ShowOverwritePrompt = true
        });

        if (file is not null)
        {
            try
            {
#if NETFRAMEWORK
                using var stream = await file.OpenWriteAsync();
#else
                await using var stream = await file.OpenWriteAsync();
#endif
                await callback(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    private static IStorageProvider? GetStorageProvider()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window })
        {
            return window.StorageProvider;
        }

        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime { MainView: { } mainView })
        {
            var visualRoot = mainView.GetVisualRoot();
            if (visualRoot is TopLevel topLevel)
            {
                return topLevel.StorageProvider;
            }
        }

        return null;
    }

    private static List<FilePickerFileType> GetFilePickerFileTypes(List<string> fileTypes)
    {
        var fileTypeFilters = new List<FilePickerFileType>();

        foreach (var fileType in fileTypes)
        {
            switch (fileType)
            {
                case "All":
                    {
                        fileTypeFilters.Add(All);
                        break;
                    }
                case "Json":
                    {
                        fileTypeFilters.Add(Json);
                        break;
                    }
                case "Text":
                    {
                        fileTypeFilters.Add(Text);
                        break;
                    }
                case "Csv":
                    {
                        fileTypeFilters.Add(Csv);
                        break;
                    }
            }
        }

        return fileTypeFilters;
    }

    public List<string> GetComPorts()
    {
        List<string> comPorts = new();

        var ports = SerialPort.GetPortNames();

        foreach (string port in ports)
        {
            comPorts.Add(port);
        }
        return comPorts;
    }

    public static bool ReadRegisters()
    {
        try
        {
            ModbusTcpMasterReadHoldingRegistersAsync();
        }

        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }


        return true;
    }

    /// <summary>
    ///     Modbus TCP master read holding registers
    /// </summary>
    public static async Task ModbusTcpMasterReadHoldingRegistersAsync()
    {
        using (TcpClient client = new TcpClient("192.168.1.210", 502))
        {
            var factory = new ModbusFactory();
            IModbusMaster master = factory.CreateMaster(client);

            byte slaveId = 1;
            ushort startAddress = 7165;
            ushort numInputs = 5;

            ushort[] registers = await master.ReadHoldingRegistersAsync(slaveId, startAddress, numInputs);

            for (int i = 0; i < numInputs; i++)
            {
                Console.WriteLine($"Input {(startAddress + i)}={registers[i]}");
            }
        }
    }

    /// <summary>
    ///     Modbus TCP master write holding registers example
    /// </summary>
    public static async Task ModbusTcpMasterWriteHoldingRegistersAsync()
    {
        using (TcpClient client = new TcpClient("192.168.1.210", 502))
        {
            var factory = new ModbusFactory();
            IModbusMaster master = factory.CreateMaster(client);

            byte slaveId = 1;
            ushort startAddress = 7165;
            //ushort numInputs = 5;
            //UInt32 www = 0x42c80083;
            ushort[] data = new ushort[] { 10, 20, 30 };

            await master.WriteMultipleRegistersAsync(slaveId, startAddress, data);

        }
    }


}