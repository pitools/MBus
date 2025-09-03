using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
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

}