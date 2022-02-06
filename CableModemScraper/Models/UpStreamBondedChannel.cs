using CsvHelper.Configuration.Attributes;
using System;

namespace CableModemScraper.Models
{
    internal class UpStreamBondedChannel
    {
        [Index(0)]
        [Format("yyyy-MM-dd hh:mm:ss")]
        [Name("Time Stamp")]
        public DateTime TimeStamp { get; set; }

        [Index(1)]
        [Name("Channel")]
        public int Channel { get; set; }

        [Index(2)]
        [Name("Channel ID")]
        public int ChannelId { get; set; }

        [Index(3)]
        [Name("Lock Status")]
        public string LockStatus { get; set; }

        [Index(4)]
        [Name("US Channel Type")]
        public string USChannelType { get; set; }

        [Index(5)]
        [Name("Frequency")]
        public int Frequency { get; set; }

        [Index(6)]
        [Name("Width")]
        public int Width { get; set; }

        [Index(7)]
        [Name("Power")]
        public decimal Power { get; set; }
    }
}
