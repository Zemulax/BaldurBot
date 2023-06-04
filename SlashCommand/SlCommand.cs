using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using OpenAI_API;
using System.Threading.Tasks;

namespace BaldurBot.SlashCommand
{
	public class SlCommand : ApplicationCommandModule
	{
		/// <summary>
		/// This method defines a command that helps users find their appropriate CSC
		/// </summary>
		/// <param name="ctx">information about the command and interaction(user,channel, etc)</param>
		/// <returns>returns an embedded message</returns>
		[SlashCommand("AskBaldur", "Interact with Baldur")]
		public async Task CSCCommand(InteractionContext ctx, [Option("Query", "Ask me anything")] string message)
		{
			//this will check if the message is coming from a specific channel
			//called bot channel
			//if not, the bot will not respond.
			if (ctx.Channel.Id != 1114222366520782920)
			{
				var restrictedEmbed = new DiscordEmbedBuilder()
				{
					Title = "I am Restricted from this Realm.",
					Color = DiscordColor.Red,
					Description = "To interact with Baldur visit the bot channel realm.",
				};
				await ctx.CreateResponseAsync(restrictedEmbed);
				return;
			}
			await ctx.DeferAsync();
			var api = new OpenAIAPI(ConfigJSON.ApiSecret);

			var prompt = "You are Baldur, an expert in Samsung mobile phones and firmware flashing.\n" +
			 "You have extensive knowledge about Samsung devices and can assist users with their queries.\n" +
			 "You are particularly knowledgeable about the CSC and the process of flashing firmware.\n" +
			 "If users need help finding their CSC, you can guide them through the following method:\n" +
			 "Go to Settings > About Phone > Status Information > Model Code.\n" +
			 "The last three letters of the Model Code represent the CSC.\n" +
			 "Your expertise also extends to using Frija for firmware downloads.\n" +
			 "You are here to provide support and answer any questions related to Samsung devices and firmware ONLY.\n" +
			 "Refrain from talking about other brands such as Apple,Oppo,Google,Xiaomi,Redmi,LG etc" +
			 "Please note that your responses are limited to providing assistance within your area of expertise.\n" +
			 $"User: {message}\n" +
			 "Baldur: ";


			//starting a new chat
			var chat = api.Chat.CreateConversation();

			//the user query
			chat.AppendUserInput(prompt);

			//response from chatgpt
			string response = await chat.GetResponseFromChatbotAsync();

			var responseEmbed = new DiscordEmbedBuilder()
			{
				Title = "You: " + message,
				Color = DiscordColor.Purple,
				Description = response,
			};
			//respond to user query
			await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(responseEmbed));
		}
	}
}
