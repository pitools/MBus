using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MBLite.Models;

namespace MBLite.Repositories
{
    public interface IRegisterRepository
    {
        Task<IEnumerable<CsvRecordRegister>> LoadFromCsvAsync(Stream stream,
            IProgress<double> progress, CancellationToken cancellationToken);
        Task SaveToCsvAsync(IEnumerable<CsvRecordRegister> registers, Stream stream, CancellationToken cancellationToken);
    }
}
