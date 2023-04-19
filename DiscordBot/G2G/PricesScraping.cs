using DiscordBot.Common;
using DiscordBot.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace DiscordBot.G2G
{
    public class PricesScraping
    {
        private static string _serverName;
        private static bool _haveOutput = false;
        private static StringBuilder _content = new();

        public const string _oneThousand = "1k";
        public const string _oneGold = "1 gold";
        public PricesScraping(string serverName)
        {
            _serverName = serverName;
        }

        /// <summary>
        /// Method for scraping wow prices from g2g.com
        /// </summary>
        /// <returns></returns>
        public async Task<string> ScrapeWoWPrices()
        {
            try
            {
                _content.AppendLine(":coin: Contact <@960222058518810664> if you want to sell your gold! :coin:");
                _content.Append("```");

                List<Task> tasks = new();

                //Scrape shadowlands
                tasks.Add(Scrape(Constants.ShadowladnsGameId, _serverName, _oneThousand));

                //Scrape TBC
                tasks.Add(Scrape(Constants.TBCGameId, _serverName, _oneGold));

                await Task.WhenAll(tasks);
                _content.Append("```");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (_haveOutput)
                return _content.ToString();
            else
                return $"'{_serverName}' can not be found. Please check the server name (spaces, special characters etc).";

        }
        private static async Task Scrape(string gameId, string serverName, string currency)
        {
            try
            {
                RestResponse response = CommonMethods.G2GGetRequest(Constants.ShadowladnsGameId, _serverName);

                GetResponseMsg(response, currency);
            }
            catch (Exception ex)
            {

            }
        }

        private static void GetResponseMsg(RestResponse response, string currency)
        {
            var jsonDeserialized = JsonConvert.DeserializeObject<G2GResponse>(response.Content);

            if (jsonDeserialized.Payload.Results != null)
            {
                foreach (var item in jsonDeserialized.Payload.Results)
                {
                    _content.AppendLine($"{item.Title} - {item.Converted_Unit_Price - (item.Converted_Unit_Price * 25 / 100)}$ / {currency}");
                    _haveOutput = true;
                }
            }
        }

    }
}
