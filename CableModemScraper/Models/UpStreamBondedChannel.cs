using CsvHelper.Configuration.Attributes;
using System;

namespace CableModemScraper.Models
{
    public class UpStreamBondedChannel
    {
        [Index(0)] public DateTime TimeStamp { get; set; }
        [Index(1)] public int Channel { get; set; }
        [Index(2)] public int ChannelId { get; set; }
        [Index(3)] public string LockStatus { get; set; }
        [Index(4)] public string USChannelType { get; set; }
        [Index(5)] public int Frequency { get; set; }
        [Index(6)] public int Width { get; set; }
        [Index(7)] public decimal Power { get; set; }
    }
}
