using CableModemScraper.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace CableModemScraper
{
    internal class TokenAcquirer
    {
        private readonly Uris Uris;

        internal TokenAcquirer(Uris uris) => Uris = uris;

        private HttpClientHandler InsecureHttpClientHandler => new HttpClientHandler() { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator };

        internal async System.Threading.Tasks.Task<Tuple<string, string>> AcquireAsync()
        {
            string token = string.Empty;
            string sessionId = string.Empty; ;

            using (var client = new HttpClient(InsecureHttpClientHandler))
            {
                try { await client.GetAsync(Uris.Logout); } catch (Exception) { }

                var _ = client.GetAsync(Uris.BaseAddress);

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, Uris.ConnectionStatusAddressWithAuth))
                {
                    requestMessage.Headers.Clear();
                    requestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
                    requestMessage.Headers.Add(HttpRequestHeader.ContentType.ToString(), "application/x-www-form-urlencoded; charset=utf-8");
                    requestMessage.Headers.Add(HttpRequestHeader.Cookie.ToString(), "HttpOnly: true, Secure: true");
                    requestMessage.Headers.Add("Authorization", $"Basic {Uris.Auth}");

                    var response = await client.SendAsync(requestMessage);
                    token = await response.Content.ReadAsStringAsync();

                    sessionId  = response.Headers.SingleOrDefault(x => x.Key == "Set-Cookie").Value.First()?.ToString()?.Split(';')?.First()?.Split('=')?.Last();
                }
            }

            if (token.Contains("Login"))
                throw new Exception("Failed to acquire token!");

            if (sessionId == null)
                throw new Exception("Failed to aquire sessionId!");

            return new Tuple<string, string>(token, sessionId);
        }
    }
}
