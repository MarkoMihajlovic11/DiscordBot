using DiscordBot.Common;
using DiscordBot.Models;
using Newtonsoft.Json;
using System.Text;

namespace DiscordBot.G2G
{
    public static class PriceScraping
    {
        /// <summary>
        /// Method for scraping prices from g2g.com
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="serverName"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static async Task<string> ScrapePricesAsync(string gameId, string serverName, string currency)
        {
            StringBuilder content = new();

            try
            {
                var response = await Requests.G2GGetRequest(gameId, serverName);

                var jsonDeserialized = JsonConvert.DeserializeObject<G2GResponse>(response.Content!);

                if (jsonDeserialized!.Payload.Results != null)
                {
                    AppendContent(content, currency, jsonDeserialized);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return content.ToString();
        }


        private static void AppendContent(StringBuilder content, string currency, G2GResponse jsonDeserialized)
        {
            foreach (var item in jsonDeserialized.Payload.Results)
            {
                content.AppendLine($"{item.Title} - {item.Converted_Unit_Price - (item.Converted_Unit_Price * Constants.Precentage / 100)}$ / {currency}");
            }
        }

    }
}
