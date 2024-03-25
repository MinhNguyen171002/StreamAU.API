using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;

namespace SVMusic.Hubs
{
    public class MusicHub : Hub
    {

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public async IAsyncEnumerable<int> Counter(int count,int delay, [EnumeratorCancellation]CancellationToken cancellationToken)
        {
            for (var i = 0; i < count; i++)
            {
                
                cancellationToken.ThrowIfCancellationRequested();

                yield return i;

                // Use the cancellationToken in other APIs that accept cancellation
                // tokens so the cancellation can flow down to them.
                await Task.Delay(delay, cancellationToken);
            }
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
