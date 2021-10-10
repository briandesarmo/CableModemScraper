using CsvHelper.Configuration.Attributes;
using System;

namespace CableModemScraper.Models
{
    public class DownBondedStreamChannel
    {
        [Index(0)]
        [Name("Time Stamp")] 
        public DateTime TimeStamp { get; set; }
        
        [Index(1)]
        [Name("Channel ID")]
        public int ChannelId { get; set; }
        
        [Index(2)]
        [Name("Lock Status")]
        public string LockStatus { get; set; }

        [Index(3)]
        [Name("Modulation")] 
        public string Modulation { get; set; }

        [Index(4)] 
        [Name("Frequency")]
        public int Frequency { get; set; }

        [Index(5)]
        [Name("Power")] 
        public decimal Power { get; set; }

        [Index(6)]
        [Name("SNR/MER")]
        public decimal SNRMER { get; set; }

        [Index(7)]
        [Name("Corrected")]
        public long Corrected { get; set; }

        [Index(8)]
        [Name("Uncorrectables")] 
        public long Uncorrectables { get; set; }
    }
}
