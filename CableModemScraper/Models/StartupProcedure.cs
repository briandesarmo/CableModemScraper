using CsvHelper.Configuration.Attributes;
using System;

namespace CableModemScraper.Models
{
    internal class StartupProcedure
    {
        [Index(0)]
        [Format("yyyy-MM-dd hh:mm:ss")]
        [Name("Time Stamp")]
        public DateTime TimeStamp { get; set; }

        [Index(1)]
        [Name("Procedure")]
        public string Procedure { get; set; }

        [Index(2)]
        [Name("Status")]
        public string Status { get; set; }

        [Index(3)]
        [Name("Comment")]
        public string Comment { get; set; }
    }
}
