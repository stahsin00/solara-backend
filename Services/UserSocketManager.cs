using System.Net.WebSockets;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Solara.Services
{
    public class UserSocketManager
    {
        private readonly ConcurrentDictionary<int, WebSocket> _sockets = new ConcurrentDictionary<int, WebSocket>();

        public Task AddSocket(int userId, WebSocket socket)
        {
            _sockets.TryAdd(userId, socket);
            return Task.CompletedTask;
        }

        public async Task CloseSocket(int userId, string closeStatusDescription = "Websocket connection closed.")
        {
            if (_sockets.TryRemove(userId, out var socket))
            {
                if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, closeStatusDescription, CancellationToken.None);
                }
                socket.Dispose();
            }
        }

        // TODO: send JSON info instead (?)
        public async Task SendMessageAsync(int userId, string message)
        {
            if (_sockets.TryGetValue(userId, out var socket) && socket.State == WebSocketState.Open)
            {
                var encodedMessage = System.Text.Encoding.UTF8.GetBytes(message);
                var buffer = new ArraySegment<byte>(encodedMessage, 0, encodedMessage.Length);
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
