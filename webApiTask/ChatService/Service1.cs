using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using vtortola.WebSockets;

namespace ChatService
{
    public partial class ChatService : ServiceBase
    {
        public static List<WebSocket> Sockets;
        public static Dictionary<string, string> Names;
        private WebSocketEventListener _server;

        public ChatService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Sockets = new List<WebSocket>();
            Names = new Dictionary<string, string>();
            _server = new WebSocketEventListener(new IPEndPoint(IPAddress.Any, 8002),
                  new WebSocketListenerOptions()
                  {
                      SubProtocols = new String[] { "123456" },
                      NegotiationTimeout = TimeSpan.FromSeconds(30)
                  });
            SetEvents(_server);
            _server.Start();

        }

        public static void SetEvents(WebSocketEventListener server)
        {
            server.OnConnect += (ws) =>
            {
                Sockets.Add(ws);
                System.IO.File.WriteAllText(@"C:\qwerty\WriteLines.txt", "Connected");
            };
            server.OnDisconnect += (ws) =>
            {
                Sockets.Remove(ws);

            };
            server.OnError += (ws, ex) =>
            {

            };
            server.OnMessage += (ws, msg) =>
            {
                var wsContext = ws;

                var msgData = msg.Split(":".ToArray(), StringSplitOptions.RemoveEmptyEntries)[1];
                var msgToSend = string.Empty;
                if (msg.StartsWith("Name"))
                {
                    Names.Add(ws.RemoteEndpoint.ToString(), msgData);
                    msgToSend = ComposeMsg(Names[ws.RemoteEndpoint.ToString()], "Joined");
                }
                else
                {
                    msgToSend = ComposeMsg(Names[ws.RemoteEndpoint.ToString()], msgData);
                }

                //var task = Task.Factory.StartNew(() =>
                //{
                // util.ProcessMessage(wsContext);
                foreach (var w in Sockets)
                {
                    if (w != ws)
                    {
                        w.WriteString(msgToSend);
                    }
                }
                //});
            };
        }

        public static string ComposeMsg(string name, string msg)
        {
            return $"<div><strong>{name}</strong><span>  {msg}</span></div>";
        }

        protected override void OnStop()
        {
            _server.Stop();
        }
    }
}

