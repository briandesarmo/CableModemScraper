using CableModemScraper.Models;
using System;
using System.Linq;
using System.Net;

namespace CableModemScraper
{
    internal class TokenAcquirer
    {
        private readonly Uris Uris;

        internal TokenAcquirer(Uris uris) => Uris = uris;

        internal Tuple<string, string> Acquire()
        {
            string token;
            string sessionId;

            using (var webclient = new WebClient())
            {
                try { webclient.DownloadString(Uris.Logout); } catch (Exception) { }

                var _ = webclient.DownloadString(Uris.BaseAddress);

                webclient.Headers.Set(HttpRequestHeader.Accept, $"*/*");
                webclient.Headers.Set(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded; charset=utf-8");
                webclient.Headers.Set(HttpRequestHeader.Cookie, "HttpOnly: true, Secure: true");
                webclient.Headers.Set(HttpRequestHeader.Authorization, $"Basic {Uris.Auth}");

                token = webclient.DownloadString(Uris.ConnectionStatusAddressWithAuth);

                sessionId = webclient.ResponseHeaders["Set-Cookie"]?.Split(';')?.First()?.Split('=')?.Last();

                if (token.Contains("Login"))
                    throw new Exception("Failed to acquire token!");

                if (sessionId == null)
                    throw new Exception("Failed to aquire sessionId!");
            }

            return new Tuple<string, string>(token, sessionId);
        }
    }
}
