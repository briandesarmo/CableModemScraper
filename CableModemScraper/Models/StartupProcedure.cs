using CsvHelper.Configuration.Attributes;
using System;

namespace CableModemScraper.Models
{
    public class StartupProcedure
    {
        [Index(0)] public DateTime TimeStamp { get; set; }
        [Index(1)] public string Procedure { get; set; }
        [Index(2)] public string Status { get; set; }
        [Index(3)] public string Comment { get; set; }
    }
}
