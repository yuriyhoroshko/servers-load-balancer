using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using Contract.ByteDefinition;

namespace worker_server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), Int32.Parse(args[0])));
            socket.Listen(1);
            while (true)
            {
                byte[] buffer = new byte[268435456];
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
                            CommandHandler.Handle(ref buffer,handler);
                        }
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        handler.Disconnect(true);
                        buffer = null;
                        GC.Collect();
                        Console.WriteLine("Waiting for new connection....");
                        isDisconnected = true;
                    }
            }
        }
    }
}