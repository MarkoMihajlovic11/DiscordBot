using RestSharp;

namespace DiscordBot.Common
{
    public static class CommonMethods
    {
        /// <summary>
        /// Finds lowest prices on g2g.com based on game id and server name
        /// </summary>
        /// <param name="gameId">aGame id</param>
        /// <param name="serverName">Server name</param>
        /// <returns></returns>
        public static RestResponse G2GGetRequest(string gameId, string serverName)
        {
            var client = new RestClient($"https://sls.g2g.com/offer/search?service_id=lgc_service_1&brand_id=lgc_game_{gameId}&q={serverName}&sort=lowest_price&page_size=48&currency=USD&country=RS");
            var request = new RestRequest($"https://sls.g2g.com/offer/search?service_id=lgc_service_1&brand_id=lgc_game_{gameId}&q={serverName}&sort=lowest_price&page_size=48&currency=USD&country=RS", Method.Get);
            request.AddHeader("authority", "sls.g2g.com");
            request.AddHeader("accept", "application/json, text/plain, */*");
            request.AddHeader("accept-language", "en-GB,en-US;q=0.9,en;q=0.8");
            request.AddHeader("authorization", "null");
            request.AddHeader("origin", "https://www.g2g.com");
            request.AddHeader("referer", "https://www.g2g.com/");
            request.AddHeader("sec-ch-ua", "\".Not/A)Brand\";v=\"99\", \"Google Chrome\";v=\"103\", \"Chromium\";v=\"103\"");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-site", "same-site");
            request.AddHeader("x-api-key", "0DWATzuevY8r91rPCySTl91p2Odp6itK23sOskIX");
            RestResponse response = client.Execute(request);
            return response;
        }
    }
}
