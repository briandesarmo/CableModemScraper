using System.Collections.Generic;

namespace CableModemScraper.Models
{
    internal class Scraping
    {
        internal List<DownStreamBondedChannel> DownStreamBondedChannels { get; set; }
        internal List<UpStreamBondedChannel> UpStreamBondedChannels { get; set; }
        internal List<StartupProcedure> StartupProcedures { get; set; }
    }
}
