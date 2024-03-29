﻿using System;
using System.Text;

namespace CableModemScraper.Models
{
    internal class Uris
    {
        internal Uri BaseAddress { get; }
        internal Uri ConnectionStatusAddress { get; }
        internal Uri ConnectionStatusAddressWithAuth { get; }
        internal Uri Logout { get; }
        internal string Auth { get; }

        private const string ConnectionStatusPath = "cmconnectionstatus.html";
        private const string LogoutPath = "logout.html";

        internal Uris(Settings settings)
        {
            var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.UserName}:{settings.Password}"));

            BaseAddress = new UriBuilder(settings.BaseAddress).Uri;
            ConnectionStatusAddress = new UriBuilder(BaseAddress) { Path = ConnectionStatusPath }.Uri;
            ConnectionStatusAddressWithAuth = new UriBuilder(ConnectionStatusAddress) { Query = $"login_{auth}" }.Uri;
            Logout = new UriBuilder(BaseAddress) { Path = LogoutPath }.Uri;
            Auth = auth;
        }
    }
}
