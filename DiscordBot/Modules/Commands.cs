using Discord;
using Discord.Commands;
using DiscordBot.G2G;

namespace DiscordBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// !rate command
        /// </summary>
        /// <param name="serverName">Server name</param>
        /// <returns></returns>
        [Command("rate")]
        public async Task Rate([Remainder] string serverName)
        {
            await DoCommand(serverName);
        }

        /// <summary>
        /// !price command
        /// </summary>
        /// <param name="serverName">Server name</param>
        /// <returns></returns>
        [Command("price")]
        public async Task Price([Remainder] string serverName)
        {
            await DoCommand(serverName);
        }

        private async Task DoCommand(string serverName)
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
