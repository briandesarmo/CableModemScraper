using CableModemScraper.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CableModemScraper
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var settings = new Settings();
            var scraper = new Scraper(settings);

            while (true)
            {
                var scraping = await scraper.ScrapeAsync();
                WriteCsv(settings.StartupProceduresCsv, scraping.StartupProcedures);

                foreach (var grp in scraping.DownStreamBondedChannels.GroupBy(c => c.ChannelId))
                    WriteCsv($"{grp.Key}_{settings.DownStreamCsv}", grp.ToList());

                foreach (var grp in scraping.UpStreamBondedChannels.GroupBy(c => c.ChannelId))
                    WriteCsv($"{grp.Key}_{settings.UpStreamCsv}", grp.ToList());

                Thread.Sleep(300000);
            }
        }

        internal static void WriteCsv<T>(string path, List<T> records) where T : class
        {
            var fileExists = File.Exists(path);

            using (var writer = new StreamWriter(path, append: true))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = !fileExists }))
                csv.WriteRecords(records);
        }
    }
}
