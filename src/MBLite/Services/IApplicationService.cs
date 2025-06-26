using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBLite.Services;

public interface IApplicationService
{
    Task OpenFileAsync(Func<Stream, IProgress<double>, Task> callback, List<string> fileTypes, string title, IProgress<double> progress);
    Task SaveFileAsync(Func<Stream, Task> callback, List<string> fileTypes, string title, string fileName, string defaultExtension);
    List<string> GetComPorts();
    void Exit();
}