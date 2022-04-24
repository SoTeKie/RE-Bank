using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using osu.Framework.Bindables;

namespace rebank.Game
{
    public static class GlobalState
    {
        public static Bindable<int> PlayerMoney = new Bindable<int>(100);
        public static HttpClient Client = new HttpClient();
        public static string AccessToken;

        static GlobalState()
        {
            Client.BaseAddress = new Uri("http://localhost:8000/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
