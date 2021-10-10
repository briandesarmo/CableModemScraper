using CsvHelper.Configuration.Attributes;
using System;

namespace CableModemScraper.Models
{
    public class DownBondedStreamChannel
    {
        [Index(0)] public DateTime TimeStamp { get; set; }
        [Index(1)] public int ChannelId { get; set; }
        [Index(2)] public string LockStatus { get; set; }
        [Index(3)] public string Modulation { get; set; }
        [Index(4)] public int Frequency { get; set; }
        [Index(5)] public decimal Power { get; set; }
        [Index(6)] public decimal SNRMER { get; set; }
        [Index(7)] public long Corrected { get; set; }
        [Index(8)] public long Uncorrectables { get; set; }
    }
}
