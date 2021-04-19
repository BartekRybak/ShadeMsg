using System;
using ShadeMsg;
using ShadeMsg.Security;
using ShadeMsg.Network;
using ShadeMsg_Server.Network;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using ShadeMsg.Network.Packets;

namespace ShadeMsg_Server
{
    class Program
    {
        static List<Client> clients = new List<Client>();
        static readonly string password = "1234";
        static Server server = new Server(8001, password);

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server..");
            server.Start();
            Console.Write("Done!\n");

            server.NewSocketConnected += Server_NewSocketConnected;

            while (true) { 
                
                Thread.Sleep(1000); 
            } 
        }

        /// <summary>
        /// New Socket Connected
        /// </summary>
        /// <param name="socket">client socket</param>
        private static void Server_NewSocketConnected(Socket socket)
        {
            Console.WriteLine("Connected: {0}", socket.RemoteEndPoint.ToString());
            Client client = new Client(socket, password);
            clients.Add(client);
            client.NewPacket += Client_NewPacket;
            client.Disconected += Client_Disconected;
            
        }

        /// <summary>
        /// Client Disconnected
        /// </summary>
        /// <param name="client">disconnected client</param>
        private static void Client_Disconected(Client client)
        {
            for(int i =0;i< clients.ToArray().Length;i++)
            {
                if(clients[i].socket.RemoteEndPoint == client.socket.RemoteEndPoint)
                {
                    client.Stop();
                    clients.RemoveAt(i);
                    Console.WriteLine("Disconnected : {0}", client.socket.RemoteEndPoint);
                }
            }
        }

        /// <summary>
        /// Receive new packet from 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="socket"></param>
        private static void Client_NewPacket(Packet packet, Client client)
        {
            if(packet != Packet.Empty)
            {
                if(packet.name == "nick")
                {
                    client.nick = packet.GetArgument("nick").value;
                }

                if(packet.name == "msg")
                {
                    foreach (Client _client in clients.ToArray())
                    {
                        if(_client != client)
                        {
                            server.Send(new Packet()
                            {
                                name = "msg",
                                args = new Argument[] {
                                new Argument("text",packet.GetArgument("text").value),
                                new Argument("nick",client.nick)
                            }
                            }, _client);
                        }
                    }
                    Console.WriteLine("{0} : {1}", client.nick, packet.GetArgument("text").value);
                }
            }
        }
    }
}
