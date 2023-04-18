using DiscordBot.Common;
using DiscordBot.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscordBot.G2G
{
    public  class PricesScraping
    {
        private static string _serverName;
        private static bool _haveOutput = false;
        private static StringBuilder _content = new();
        public PricesScraping(string serverName)
        {
            _serverName = serverName;
        }
        public async Task<string> ScrapeWoWPrices()
        {
            try
            {
                _content.AppendLine(":coin: Contact <@960222058518810664> if you want to sell your gold! :coin:");
                _content.Append("```");

                List<Task> tasks = new();
                tasks.Add(ShadowladnsScraping());
                tasks.Add(TBCScraping());

                await Task.WhenAll(tasks);
                _content.Append("```");            
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);           
            }

            if(_haveOutput)
                return _content.ToString();
            else
                return $"'{_serverName}' can not be found. Please check the server name (spaces, special characters etc).";

        }
        private static async Task ShadowladnsScraping()
        {
            try
            {
                RestResponse response = CommonMethods.G2GGetRequest(Constants.ShadowladnsGameId);

                GetResponseMsg(response, "1k");
            }
            catch (Exception ex)
            {

            }
        }


        public static async Task TBCScraping()
        {
            try
            {
                RestResponse response = CommonMethods.G2GGetRequest(Constants.TBCGameId);

                GetResponseMsg(response, "1 gold");
            }
            catch (Exception ex)
            {
            }
        }

        private static void GetResponseMsg(RestResponse response, string gold)
        {
            var jsonDeserialized = JsonConvert.DeserializeObject<G2GResponse>(response.Content);

            if (jsonDeserialized.Payload.Results != null)
            {
                foreach (var item in jsonDeserialized.Payload.Results)
                {
                    _content.AppendLine($"{item.Title} - {item.Converted_Unit_Price - (item.Converted_Unit_Price * 25 / 100)}$ / {gold}");
                    _haveOutput = true;
                }
            }
        }

    }
}
