using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NModbus;
using NModbus.Extensions.Enron;
using NModbus.Serial;
using NModbus.Utility;

namespace MBTools.Driver
{
    public static class ModbusRtu
    {
        private const string PrimarySerialPortName = "COM4";
        private const string SecondarySerialPortName = "COM2";

        /// <summary>
        ///     Simple Modbus serial RTU master write holding registers example
        /// </summary>
        public static void WriteRegisters()
        {
            using (SerialPort port = new SerialPort(PrimarySerialPortName))
            {
                // configure serial port
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();

                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateRtuMaster(port);

                byte slaveId = 1;
                ushort startAddress = 101;
                ushort[] registers = new ushort[] { 221 };

                // write three registers
                master.WriteMultipleRegisters(slaveId, startAddress, registers);
            }
        }

        /// <summary>
        ///     Simple Modbus serial RTU master read holding registers example
        /// </summary>
        public static void ReadRegisters()
        {
            using (SerialPort port = new SerialPort(PrimarySerialPortName))
            {
                // configure serial port
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();

                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateRtuMaster(port);

                byte slaveId = 2;
                ushort startAddress = 0;
                //ushort[] registers = new ushort[] { 1, 2, 3 };
                ushort numberOfPoints = 100;

                // write three registers
                // master.WriteMultipleRegisters(slaveId, startAddress, registers);

                // read one register
                ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numberOfPoints);
                for (int i = 0; i < numberOfPoints; i++)
                {
                    Console.WriteLine($"Holding register {(startAddress + i)}={registers[i]}");
                }
            }
        }
    }
}