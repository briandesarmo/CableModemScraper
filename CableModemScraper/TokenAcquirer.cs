using CableModemScraper.Models;
using System.Net;

namespace CableModemScraper
{
    internal class TokenAcquirer
    {
        private readonly Uris Uris;

        public TokenAcquirer(Uris uris) => Uris = uris;

        public string Acquire()
        {
            string token;

            using (var webclient = new WebClient())
            {
                webclient.Headers.Set(HttpRequestHeader.Accept, $"*/*");
                webclient.Headers.Set(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded; charset=utf-8");
                webclient.Headers.Set(HttpRequestHeader.Cookie, "HttpOnly: true, Secure: true");
                webclient.Headers.Set(HttpRequestHeader.Authorization, $"Basic {Uris.Auth}");

                token = webclient.DownloadString(Uris.ConnectionStatusAddressWithAuth);
            }

            return token;
        }
    }
}
