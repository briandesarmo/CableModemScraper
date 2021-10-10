using CableModemScraper.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace CableModemScraper
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var settings = new Settings();
            
            var scraping = await new Scaper(settings).ScrapeAsync();

            WriteCsv(settings.StartupProceduresCsv, scraping.StartupProcedures);
            WriteCsv(settings.DownStreamCsv, scraping.DownBondedStreamChannels);
            WriteCsv(settings.UpStreamCsv, scraping.UpStreamBondedChannels);
        }

        static void WriteCsv<T>(string path, List<T> records) where T : class
        {
            var fileExists = File.Exists(path);

            using (var writer = new StreamWriter(path, append: true))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = !fileExists }))
                csv.WriteRecords(records);
        }
    }
}
