using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Common;
using DiscordBot.G2G;
using System.Text;

namespace DiscordBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// !ratewow command
        /// </summary>
        /// <param name="serverName">Server name</param>
        /// <returns></returns>
        [Command("ratewow")]
        public async Task RateWow([Remainder] string serverName)
        {
            await ScrapePricesAsyncWow(serverName);
        }

        /// <summary>
        /// !pricewow command
        /// </summary>
        /// <param name="serverName">Server name</param>
        /// <returns></returns>
        [Command("pricewow")]
        public async Task Price([Remainder] string serverName)
        {
            await ScrapePricesAsyncWow(serverName);
        }

        /// <summary>
        /// Mthod for scraping wow prices from g2g.com
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        private async Task ScrapePricesAsyncWow(string serverName)
        {
            try
            {
                List<Task<string>> tasks = new()
                {
                    PricesScraping.ScrapePricesAsync(Constants.ShadowladnsGameId, serverName, Constants.OneThousandGolds),
                    PricesScraping.ScrapePricesAsync(Constants.TBCGameId, serverName, Constants.OneGold)
                };

                await Task.WhenAll(tasks);

                var responses = tasks
                    .Where(task => !string.IsNullOrEmpty(task.Result))
                    .Select(task => task.Result)
                    .ToList();

                await WriteMessageAndReaction(Context.Message.Author, responses, serverName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task WriteMessageAndReaction(SocketUser author, List<string> responses, string serverName)
        {
            Emoji reaction;
            StringBuilder message = new();

            if (responses.Count > 0)
            {
                reaction = new Emoji("✅");

                message.Append($":coin: Contact <{Constants.UserToContact}> if you want to sell your gold! :coin:")
                    .AppendLine("```");

                foreach (var response in responses)
                {
                    message.AppendLine(response);
                }

                message.Append("```");
            }
            else
            {
                reaction = new Emoji("❌");
                message.Append($"'{serverName}' can not be found. Please check the server name (spaces, special characters etc).");
            }

            await Context.Message.AddReactionAsync(reaction);
            await Discord.UserExtensions.SendMessageAsync(author, message.ToString());
        }
    }
}
