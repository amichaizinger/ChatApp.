using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using Amazon.AWSSupport.Model;

namespace ChatApp.NewFolder
{
    internal class Server : ICommunication
    {
        private readonly IPEndPoint _iPEndPoint;
        private readonly IPAddress _ipAddress = IPAddress.Any;  // This listens on all available interfaces (local network or internet)
        private readonly int port = 1100;

        public Server()
        {
            _iPEndPoint = new IPEndPoint(_ipAddress, port);
        }
        public async Task RunAsync()
        {
            using Socket listener = new(_iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(_iPEndPoint);
            listener.Listen(100);

            while (true)
            {
                var handler = await listener.AcceptAsync();
                Console.WriteLine($"new connection to the ip address: {handler.RemoteEndPoint}");
                var buffer = new byte[1024];
                var recived = await handler.ReceiveAsync(buffer);
                var recMessage = Encoding.UTF8.GetString(buffer, 0, recived);
                Console.WriteLine("recieved from " + handler.RemoteEndPoint + ":" + recMessage);
                if (recMessage == "exit")
                {
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    break;
                }

                Console.WriteLine("write something");
                string sentMessage = Console.ReadLine();
                var messageByte = Encoding.UTF8.GetBytes(sentMessage);
                await handler.SendAsync(messageByte, SocketFlags.None);// SocketFlags. none meaning no special behavior for the send needed

            }

        }

    }

}