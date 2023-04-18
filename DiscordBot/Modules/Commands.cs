using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.G2G;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("rate")]
        public async Task Rate([Remainder] string serverName)
        {
            try
            {
                var author = Context.Message.Author;
                PricesScraping pricesScraping = new(serverName);
                var response = await pricesScraping.ScrapeWoWPrices();
                await Discord.UserExtensions.SendMessageAsync(author, response);

                Emoji reaction;
                if(response.Contains("Please check the server name"))
                {
                    reaction = new Emoji("❌");
                }
                else
                {
                    reaction = new Emoji("✅");
                }

                await Context.Message.AddReactionAsync(reaction);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [Command("price")]
        public async Task Price([Remainder] string serverName)
        {
            try
            {
                var author = Context.Message.Author;
                PricesScraping pricesScraping = new(serverName);
                var response = await pricesScraping.ScrapeWoWPrices();
                await Discord.UserExtensions.SendMessageAsync(author, response);

                Emoji reaction;
                if (response.Contains("Please check the server name"))
                {
                    reaction = new Emoji("❌");
                }
                else
                {
                    reaction = new Emoji("✅");
                }

                await Context.Message.AddReactionAsync(reaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
