using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using worker_server.ByteDefinition;

namespace worker_server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 9915));
            socket.Listen(1);
            while (true)
            {
                byte[] buffer = new byte[2073741824];
                var handler = socket.Accept();
                var isDisconnected = false;
                while (!isDisconnected)
                    try
                    {
                        handler.Receive(buffer);
                        if (buffer[0] == (byte)101)
                        {
                            Console.WriteLine("Connected with message " + buffer[0] + " waiting for command");
                            handler.Send(Bytes.type["connection_response"]);
                        }
                        else
                        {
                            CommandHandler.Handle(buffer);
                        }
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        handler.Disconnect(true);
                        buffer = null;
                        GC.Collect();
                        Console.ReadKey();
                        Console.WriteLine("Waiting for new connection....");
                        isDisconnected = true;
                    }
            }
        }
    }
}