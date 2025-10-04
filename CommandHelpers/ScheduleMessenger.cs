using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.CommandHelpers
{
    public class ScheduleMessenger
    {

        private readonly Dictionary<ulong, Timer> _timers = new();

        public void StartMessaging(ISocketMessageChannel channel, int startDelay, int delay)
        {
            int i = 1;

            var timer = new Timer(async _ =>
            {
                try
                {
                    await channel.SendMessageAsync($"This is scheduled message number {i}.");
                    i++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Timer error: {ex.Message}");
                }
            },
            null,
            TimeSpan.FromSeconds(startDelay),
            TimeSpan.FromSeconds(delay));

            _timers[channel.Id] = timer;
        }

        public bool StopMessaging(ISocketMessageChannel channel)
        {
            if (_timers.TryGetValue(channel.Id, out var timer))
            {
                timer.Dispose();
                _timers.Remove(channel.Id);
                return true;
            }
            return false;
        }
    }
}
