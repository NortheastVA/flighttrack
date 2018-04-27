/*********************************************************************
 *  Northeast VA Flight Tracker
 *  Copyright (C) 2018 Daniel A. Hawton (daniel@hawton.com)
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *  
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *  
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlightTrack
{
    class API
    {
        private static string _BaseURL = "http://api.northeastva.devel";
        public static Login LoginInstance;
        public static bool LoggedIn = false;

        public static async Task<bool> Login(string username, string password)
        {
            APIDataModel output = new APIDataModel();
            WebClient client = new WebClient();
            Uri uri = new System.Uri(_BaseURL + "/auth/login?username=" + username + "&password=" + password + "&token");
            output.data = await client.DownloadStringTaskAsync(uri);

            Login logincheck = JsonConvert.DeserializeObject<Login>(output.data);

            if (logincheck.status == "OK")
            {
                LoginInstance = logincheck;
                LoggedIn = true;
                return true;
            }

            LoggedIn = false;
            return false;
        }

        public API()
        {
        }
    }

    public class APIDataModel
    {
        public string url { get; set; } = "";
        public string data { get; set; } = "";
    }

    public class apiBase
    {
        public string status { get; set; } = "";
    }

    public class Login : apiBase
    {
        public static string token { get; set; } = "";
    }
}
