using Discord;
using Discord.WebSocket;
using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Abadakor
{
    class Program
    {
        private readonly DiscordSocketClient discordSocketClient;

        public Settings.Settings Settings => Abadakor.Settings.Settings.Instance;

        public Database Database => Database.Instance;

        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
            // It is recommended to Dispose of a client when you are finished
            // using it, at the end of your app's lifetime.
            discordSocketClient = new DiscordSocketClient();

            discordSocketClient.Log += LogAsync;
            discordSocketClient.Ready += ReadyAsync;
            discordSocketClient.MessageReceived += MessageReceivedAsync;
        }

        public async Task MainAsync()
        {
            // Tokens should be considered secret data, and never hard-coded.
            await discordSocketClient.LoginAsync(TokenType.Bot, Settings.Token);
            await discordSocketClient.StartAsync();

            // Block the program until it is closed.
            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        // The Ready event indicates that the client has opened a
        // connection and it is now safe to access the cache.
        private Task ReadyAsync()
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + $" { discordSocketClient.CurrentUser} est connecté!");

            Database.Open();

            return Task.CompletedTask;
        }

        // This is not the recommended way to write a bot - consider
        // reading over the Commands Framework sample.
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            // The bot should never respond to itself.
            if (message.Author.Id == discordSocketClient.CurrentUser.Id)
                return;

            if (message.Content.StartsWith('!'))
                CommandParser.Parse(message);

            if (message.Content == "!ping")
                await message.Channel.SendMessageAsync("pong!");
        }
    }
}
