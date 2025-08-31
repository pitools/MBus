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
    public void Exit()
    {
        throw new NotImplementedException();
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