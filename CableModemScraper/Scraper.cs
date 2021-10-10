using CableModemScraper.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CableModemScraper
{
    internal class Scraper
    {
        private Settings Settings;
        public Scraper(Settings settings) => Settings = settings;

        public async Task<Scraping> ScrapeAsync()
        {
            var uris = new Uris(Settings);
            
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var token = new TokenAcquirer(uris).Acquire();
            var content = await FetchPageAsync(uris, token);
            var tables = GetTables(content);

            return ParseTables(tables);
        }

        private async Task<string> FetchPageAsync(Uris uris, string token)
        {
            string content;
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(uris.BaseAddress, new Cookie("credential", token));
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = uris.BaseAddress })
            {
                var result = await client.GetAsync(uris.ConnectionStatusAddress);
                content = await result.Content.ReadAsStringAsync();
                try { _ = await client.GetAsync(uris.Logout); } catch (Exception) { }
            }

            return content;
        }

        private HtmlNodeCollection GetTables(string content)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            return doc.DocumentNode.SelectNodes("//table[@class='simpleTable']");
        }

        private Scraping ParseTables(HtmlNodeCollection tables)
        {
            var timeStamp = DateTime.Now;

            return new Scraping()
            {
                StartupProcedures = ParseStartupProcedureTable(tables, timeStamp),
                DownBondedStreamChannels = ParseDownBondedStreamChannelTable(tables, timeStamp),
                UpStreamBondedChannels = ParseUpStreamBondedChannelTable(tables, timeStamp)
            };
        }

        private List<StartupProcedure> ParseStartupProcedureTable(HtmlNodeCollection tables, DateTime timeStamp)
        {
            var startupProcedureChannelTable = tables[0];
            var startupProcedures = new List<StartupProcedure>();

            foreach (var row in startupProcedureChannelTable.SelectNodes("tr").Skip(2))
            {
                var tds = row.SelectNodes("td");

                startupProcedures.Add(new StartupProcedure()
                {
                    TimeStamp = timeStamp,
                    Procedure = tds[0].InnerText,
                    Status = tds[1].InnerText,
                    Comment = tds[2].InnerText
                });
            }

            return startupProcedures;
        }

        private List<DownBondedStreamChannel> ParseDownBondedStreamChannelTable(HtmlNodeCollection tables, DateTime timeStamp)
        {
            var downBondedStreamChannelTable = tables[1];
            var downStreamChannels = new List<DownBondedStreamChannel>();

            foreach (var row in downBondedStreamChannelTable.SelectNodes("tr").Skip(2))
            {
                var tds = row.SelectNodes("td");

                downStreamChannels.Add(new DownBondedStreamChannel()
                {
                    TimeStamp = timeStamp,
                    ChannelId = int.Parse(tds[0].InnerText),
                    LockStatus = tds[1].InnerText,
                    Modulation = tds[2].InnerText,
                    Frequency = int.Parse(tds[3].InnerText.Split(' ').First()),
                    Power = decimal.Parse(tds[4].InnerText.Split(' ').First()),
                    SNRMER = decimal.Parse(tds[5].InnerText.Split(' ').First()),
                    Corrected = long.Parse(tds[6].InnerText),
                    Uncorrectables = long.Parse(tds[7].InnerText),
                });
            }

            return downStreamChannels;
        }

        private List<UpStreamBondedChannel> ParseUpStreamBondedChannelTable(HtmlNodeCollection tables, DateTime timeStamp)
        {
            var upStreamChannelTable = tables[2];
            var upStreamBondedChannels = new List<UpStreamBondedChannel>();

            foreach (var row in upStreamChannelTable.SelectNodes("tr").Skip(2))
            {
                var tds = row.SelectNodes("td");

                upStreamBondedChannels.Add(new UpStreamBondedChannel()
                {
                    TimeStamp = timeStamp,
                    ChannelId = int.Parse(tds[0].InnerText),
                    Channel = int.Parse(tds[1].InnerText),
                    LockStatus = tds[2].InnerText,
                    USChannelType = tds[3].InnerText,
                    Frequency = int.Parse(tds[4].InnerText.Split(' ').First()),
                    Width = int.Parse(tds[5].InnerText.Split(' ').First()),
                    Power = decimal.Parse(tds[6].InnerText.Split(' ').First()),
                });
            }

            return upStreamBondedChannels;
        }
    }
}
