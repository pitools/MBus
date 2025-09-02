using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MBLite.Services
{
    public interface IFileService
    {
        Task OpenFileAsync(Func<Stream, IProgress<double>, CancellationToken, Task> callback, List<string> fileTypes, string title, IProgress<double> progress, CancellationToken cancellationToken = default);
        Task SaveFileAsync(Func<Stream, Task> callback, List<string> fileTypes, string title, string fileName, string defaultExtension);
    }
}
