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
        public async Task StartCommand([Summary("Delay before starting (in seconds)")] int start_delay = 0,
            [Summary("Delay between messages (in seconds)")] int interval_delay = 5)
        {
            _messenger.StartMessaging(Context.Channel, start_delay, interval_delay);
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
