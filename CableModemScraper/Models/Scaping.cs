using System.Collections.Generic;

namespace CableModemScraper.Models
{
    public class Scaping
    {
        public List<DownBondedStreamChannel> DownBondedStreamChannels { get; set; }
        public List<UpStreamBondedChannel> UpStreamBondedChannels { get; set; }
        public List<StartupProcedure> StartupProcedures { get; set; }
    }
}
