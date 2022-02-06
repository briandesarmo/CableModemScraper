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
        internal Scraper(Settings settings) => Settings = settings;

        private HttpClientHandler InsecureHttpClientHandler(CookieContainer cookieContainer)
            => new HttpClientHandler() 
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                CookieContainer = cookieContainer
            };

        internal async Task<Scraping> ScrapeAsync()
        {
            var uris = new Uris(Settings);

            var (token, cookie) = await new TokenAcquirer(uris).AcquireAsync();
            var content = await FetchPageAsync(uris, token, cookie);
            var tables = GetTables(content);

            return ParseTables(tables);
        }

        private async Task<string> FetchPageAsync(Uris uris, string token, string sessionId)
        {
            string content;
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(uris.BaseAddress, new Cookie("sessionId", sessionId));
            using (var handler = InsecureHttpClientHandler(cookieContainer))
            using (var client = new HttpClient(handler) { BaseAddress = uris.BaseAddress })
            {
                var result = await client.GetAsync(new UriBuilder(uris.ConnectionStatusAddress) { Query = $"ct_{token}" }.Uri);
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
                DownStreamBondedChannels = ParseDownStreamBondedChannelsTable(tables, timeStamp),
                UpStreamBondedChannels = ParseUpStreamBondedChannelsTable(tables, timeStamp)
            };
        }

        private List<StartupProcedure> ParseStartupProcedureTable(HtmlNodeCollection tables, DateTime timeStamp)
        {
            var table = tables[0];
            var startupProcedures = new List<StartupProcedure>();

            foreach (var row in table.SelectNodes("tr").Skip(2))
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

        private List<DownStreamBondedChannel> ParseDownStreamBondedChannelsTable(HtmlNodeCollection tables, DateTime timeStamp)
        {
            var table = tables[1];
            var downStreamChannels = new List<DownStreamBondedChannel>();

            foreach (var row in table.SelectNodes("tr").Skip(1))
            {
                var tds = row.SelectNodes("td");

                downStreamChannels.Add(new DownStreamBondedChannel()
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

        private List<UpStreamBondedChannel> ParseUpStreamBondedChannelsTable(HtmlNodeCollection tables, DateTime timeStamp)
        {
            var table = tables[2];
            var upStreamBondedChannels = new List<UpStreamBondedChannel>();

            foreach (var row in table.SelectNodes("tr").Skip(1))
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
