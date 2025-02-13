using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.NewFolder
{
    internal class Client : ICommunication
    {
        private readonly IPEndPoint _ipEndPoint;
        private readonly IPAddress _ipAddress = IPAddress.Any;  // This listens on all available interfaces (local network or internet)
        private readonly int _port = 1100;

        public Client()
        {
            _ipEndPoint = new IPEndPoint(_ipAddress, _port);  // Setup endpoint for connection
        }

        public async Task RunAsync()
        {
            using Socket clientSocket = new(_ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            while (true)
            { 
                try
                {
                    // Establish connection to the server
                    await clientSocket.ConnectAsync(_ipEndPoint);
                    Console.WriteLine("Connected to the server.");

                    // Send a message to the server
                    Console.WriteLine("write something");
                    var message = Console.ReadLine();
                    if(message == "exit")
                    {
                        Console.WriteLine("byebye");
                        clientSocket.Shutdown(SocketShutdown.Both);
                        break;
                    }
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    await clientSocket.SendAsync(buffer, SocketFlags.None);
                    Console.WriteLine("Message sent to the server.");

                    // Receive server's response
                    byte[] responseBuffer = new byte[1024];
                    int bytesReceived = await clientSocket.ReceiveAsync(responseBuffer, SocketFlags.None);
                    string responseMessage = Encoding.UTF8.GetString(responseBuffer, 0, bytesReceived);
                    Console.WriteLine($"Server response: {responseMessage}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error connecting to the server: {ex.Message}");
                        break;
                }
            }
        }
    }
}
