using Discord.Interactions;

namespace DiscordBot.Commands
{
    public class SlashCommandsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("test", "Receive a test message.")]
        public async Task TestCommand()
        {
            await RespondAsync("Test sucesss");
        }
    }
}
