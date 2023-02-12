using Newtonsoft.Json.Linq;
using System;
using System.Timers;
using WebSocketSharp;

namespace ThorQ_Rapid
{
    internal class WsClient : IDisposable
    {
        readonly WebSocket _socket;
        readonly Timer _keepaliveTimer;
        bool _disposed = false;

        public WsClient(string serverUrl)
        {
            _socket = new WebSocket(serverUrl);
            _socket.OnOpen += OnOpen;
            _socket.OnClose += OnClose;
            _socket.OnMessage += OnMessage;

            _keepaliveTimer = new Timer(10000);
            _keepaliveTimer.Elapsed += OnSendKeepAliveMsg;
            _keepaliveTimer.AutoReset = true;

            _socket.ConnectAsync();
        }

        public void Send(string data)
        {
            if (_disposed) return;

            _socket.Send(data);
        }
        public void Send(MessageFormat.IMessageContent messageContent)
        {
            Send(MessageFormat.Message.Serialize(messageContent));
        } 

        void OnOpen(object o, EventArgs e)
        {
            if (_disposed) return;

            Console.WriteLine("[ThorQ] Connected!");
            _keepaliveTimer.Enabled = true;
        }
        void OnClose(object o, EventArgs e)
        {
            Console.WriteLine("[ThorQ] Disconnected!");
            if (!_disposed)
            {
                _keepaliveTimer.Enabled = false;

                Console.WriteLine("[ThorQ] Reconnecting...");
                _socket.ConnectAsync();
            }
        }
        void OnMessage(object o, MessageEventArgs e)
        {
            if (_disposed) return;

            if (!e.IsText)
            {
                Console.WriteLine("[ThorQ] Got unexpected binary message!");
                return;
            }

            var msg = MessageFormat.Message.Deserialize(e.Data);

            switch (msg.id)
            {
                case MessageFormat.MessageID.Init:
                    MessageHandler.HandleMessage(msg.data.ToObject<MessageFormat.Init>());
                    break;
                case MessageFormat.MessageID.SelfProperties:
                    MessageHandler.HandleMessage(msg.data.ToObject<MessageFormat.SelfProperties>());
                    break;
                case MessageFormat.MessageID.IsUserOnlineResp:
                    MessageHandler.HandleMessage(msg.data.ToObject<MessageFormat.IsUserOnlineResp>());
                    break;
                default:
                    MessageHandler.HandleRawMessage(e.Data);
                    break;
            }
        }
        void OnSendKeepAliveMsg(object o, EventArgs e)
        {
            Send(new MessageFormat.KeepAlive());
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try { _keepaliveTimer.Dispose(); } catch (Exception) { }
            try { _socket.Close(CloseStatusCode.Normal, "BaiBai!"); } catch (Exception) { }
        }
    }
}
