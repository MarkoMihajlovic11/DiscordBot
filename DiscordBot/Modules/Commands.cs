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
        /// !pricewow or !ratewow command
        /// </summary>
        /// <param name="serverName">Server name</param>
        /// <returns></returns>
        [Command("pricewow")]
        [Alias("ratewow")]
        public async Task PriceWow([Remainder] string serverName)
        {
            try
            {
                List<Task<string>> tasks = new()
                {
                    PriceScraping.ScrapePricesAsync(Constants.ShadowladnsGameId, serverName, Constants.OneThousandGolds),
                    PriceScraping.ScrapePricesAsync(Constants.TBCGameId, serverName, Constants.OneGold)
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
