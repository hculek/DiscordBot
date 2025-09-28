using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Discord;
using Discord.WebSocket;
using DiscordBot.Commands;
using Discord.Interactions;
using Discord.Commands;

namespace DiscordBot
{
    internal class Program
    {
        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("configuration.json")
                .Build();

            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) => services
                    .AddSingleton(configuration)
                    .AddSingleton(s => new DiscordSocketClient(new DiscordSocketConfig
                    {
                        AlwaysDownloadUsers = true,
                        GatewayIntents = GatewayIntents.AllUnprivileged
                    }))
                    .AddSingleton(s => new InteractionService(s.GetRequiredService<DiscordSocketClient>()))
                    .AddSingleton<SlashCommandsHandler>()
                    .AddSingleton(s => new CommandService())
                ).Build();

            await RunAsync(host);
        }

        public async Task RunAsync(IHost host)
        {
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            var _client = provider.GetRequiredService<DiscordSocketClient>();
            var configuration = provider.GetRequiredService<IConfigurationRoot>();
            var slashCommands = provider.GetRequiredService<InteractionService>();
            await provider.GetRequiredService<SlashCommandsHandler>().InitAsync();


            _client.Log += async (LogMessage msg) =>
            {
                Console.WriteLine(msg.Message);
            };

            _client.Ready += async () =>
            {
                Console.WriteLine("Bot is ready.");
            };

            await _client.LoginAsync(TokenType.Bot, configuration.GetSection("Startup").GetValue<string>("Token"));
            await _client.StartAsync();

            _client.Ready += () => ReadyAsync(host);

            await Task.Delay(-1);
        }

        public async Task ReadyAsync(IHost host) 
        {
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            var configuration = provider.GetRequiredService<IConfigurationRoot>();
            var _interactionService = provider.GetRequiredService<InteractionService>();

            //register commands globally
            //throttled
            //default false, run once on first start or deployment of new version
            if (configuration.GetSection("Startup").GetValue<bool>("RegisterCommands"))
            {
                try
                {
                    await _interactionService.RegisterCommandsGloballyAsync();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
                
            }
        }
    }
}
