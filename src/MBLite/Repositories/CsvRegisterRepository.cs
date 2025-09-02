using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using MBLite.Models;

namespace MBLite.Repositories
{
    public class CsvRegisterRepository : IRegisterRepository
    {

        public async Task<IEnumerable<CsvRecordRegister>> LoadFromCsvAsync(
            Stream stream,
            IProgress<double> progress,
            CancellationToken cancellationToken)
        {
            // Перенесите сюда логику из ParseCsvFileAsync
            var records = new List<CsvRecordRegister>();

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            // Первый проход: подсчет общего количества строк
            var totalRecords = await CountTotalRecordsAsync(memoryStream, cancellationToken);
            // Сохраняем оригинальную позицию
            memoryStream.Position = 0;

            // Второй проход: парсинг с правильным прогрессом
            using var reader = new StreamReader(memoryStream, leaveOpen: true);
            //stream.Position = 0;
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";" };
            using var csv = new CsvReader(reader, csvConfig);
            var asyncRecords = csv.GetRecordsAsync<CsvRecordRegister>();

            await foreach (var register in asyncRecords.WithCancellation(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                records.Add(register);
                await Task.Delay(5, cancellationToken); // Simulate long-running operation
                //CurrentRowAddress = register.Address;

                var percentComplete = (double)records.Count / totalRecords * 100;
                progress?.Report(percentComplete);
            }

            return records;
        }

        public async Task SaveToCsvAsync(IEnumerable<CsvRecordRegister> registers, Stream stream, CancellationToken cancellationToken)
        {
            using var writer = new StreamWriter(stream, leaveOpen: true);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";" });
            cancellationToken.ThrowIfCancellationRequested();

            await csv.WriteRecordsAsync(registers);
            await writer.FlushAsync();
        }

        private async Task<int> CountTotalRecordsAsync(Stream stream, CancellationToken cancellationToken)
        {
            var totalRecords = 0;

            using var reader = new StreamReader(stream, leaveOpen: true);
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";" };
            using var csv = new CsvReader(reader, csvConfig);
            var asyncRecords = csv.GetRecordsAsync<CsvRecordRegister>();

            await foreach (var register in asyncRecords.WithCancellation(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                totalRecords++;
            }
            return totalRecords;
        }
    }
}
