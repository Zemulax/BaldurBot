using BaldurBot.SlashCommand;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BaldurBot
{
	/// <summary>
	/// A generic class that represents a bot 
	/// and defines its configuration
	/// </summary>
	public class Bot
	{
		public static DiscordClient Dclient { get; private set; }
		public InteractivityExtension Interactivity { get; private set; }
		public CommandsNextExtension CommandsNext { get; private set; }

		/// <summary>
		/// contains all config stuff to get the bot running
		/// set the configurations for our bot:
		/// what commands will be registered,
		/// token used
		/// </summary>
		/// <returns></returns>
		public async Task RunBaldurAsync()
		{
			//reading the json file
			var json = string.Empty;
			var openJsonfile = File.OpenRead("Config.json");
			var streamRead = new StreamReader(openJsonfile, new UTF8Encoding(false));
			json = await streamRead.ReadToEndAsync();

			//parse jscon values into the ConfigJSON class variables
			var configJson = JsonConvert.DeserializeObject<ConfigJSON>(json);

			//set the bot token for authentication
			var config = new DiscordConfiguration()
			{
				Intents = DiscordIntents.All,
				Token = configJson.Token,
				AutoReconnect = true,
			};

			//get the bot online
			Dclient = new DiscordClient(config);

			Dclient.MessageCreated += RegularMessage;
            //setting up commands
			var commandsConfig = new CommandsNextConfiguration()
			{
				EnableMentionPrefix = true,
				EnableDms = false,
				EnableDefaultHelp = false
			};

			//enable commands
			CommandsNext = Dclient.UseCommandsNext(commandsConfig);

			//register commands to a specific server
			var slashCommandsConfig = Dclient.UseSlashCommands();
			slashCommandsConfig.RegisterCommands<SlCommand>(1112511568203239577);

			//connect the bot to the server indefinitely
			await Dclient.ConnectAsync();
			await Task.Delay(-1);
		}

		/// <summary>
		/// This method deletes non command messages in the #bot channel
		/// </summary>
		/// <param name="sender"> the sender of the message, discord </param>
		/// <param name="e">a message received by the bot</param>
		/// <returns>returns an acction taken on the received message</returns>
		public async Task RegularMessage(DiscordClient sender, MessageCreateEventArgs e)
		{
			if (e.Channel.Id == 1114222366520782920 && !e.Author.IsBot && !e.Message.Content.StartsWith("/"))
			{
				await e.Channel.DeleteMessageAsync(e.Message);
			}
		}

		/// <summary>
		/// This method simply reports that the bot is ready
		/// </summary>
		/// <param name="e"></param>
		/// <returns>returns task completed</returns>
		private Task OnClientReady(ReadyEventArgs e)
		{
			return Task.CompletedTask;
		}

	}
}
