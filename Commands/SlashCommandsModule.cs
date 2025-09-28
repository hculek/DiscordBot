using Discord.Interactions;

namespace DiscordBot.Commands
{
    public class SlashCommandsModule : InteractionModuleBase<SocketInteractionContext>
    {
        private static readonly ScheduleMessenger _messenger = new ScheduleMessenger();

        [SlashCommand("test", "Receive a test message.")]
        public async Task TestCommand()
        {
            await RespondAsync("Test sucesss");
        }

        [SlashCommand("start", "Start scheduled test messages.")]
        
        public async Task StartCommand([Summary("Delay start in seconds")] int startDelay = 0,
            [Summary("Delay in seconds")] int delay = 5)
        {
            _messenger.StartMessaging(Context.Channel, startDelay, delay);
            await RespondAsync("Starting scheduled messages");
        }


        [SlashCommand("stop", "Stop scheduled test message.")]
        public async Task StopCommand()
        {
            _messenger.StopMessaging(Context.Channel);
            await RespondAsync("Stopping scheduled test messages.");
        }
    }
}
