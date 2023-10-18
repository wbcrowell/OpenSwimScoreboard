using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Threading.Tasks;

namespace OpenSwimScoreboard.Messenger
{
    public class ScoreboardStatusMessage: ValueChangedMessage<string>
    {
        //Formats a simple message summarizing the current status of the live scoreboard data service (ScoreboardBackgroundService).
        public ScoreboardStatusMessage(string message) : base(message)
        {

        }

        public static async Task SendMessageAsync(string messageText)
        {
            await Task.Run(() => (WeakReferenceMessenger.Default.Send(new ScoreboardStatusMessage(messageText))));
        }
    }
}
