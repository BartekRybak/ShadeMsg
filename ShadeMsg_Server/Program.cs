using System;
using ShadeMsg;
using ShadeMsg.Security;
using ShadeMsg.Network;
using ShadeMsg_Server.Network;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;

namespace ShadeMsg_Server
{
    class Program
    {
        static List<Client> clients = new List<Client>();

        static void Main(string[] args)
        {
            Server server = new Server(8001, "smok20122");

            Console.WriteLine("Starting Server..");
            server.Start();
            Console.Write("Done!");

            server.NewSocketConnected += Server_NewSocketConnected;

            while (true) { Thread.Sleep(1000); }
        }

        private static void Server_NewSocketConnected(Socket socket)
        {
            Console.WriteLine("New Socket Connected: {0}", socket.RemoteEndPoint.ToString());
            Client client = new Client(socket, socket.RemoteEndPoint.ToString(), "smok20122");
            client.NewPacket += Client_NewPacket;
            clients.Add(client);
        }

        private static void Client_NewPacket(Packet packet, Socket socket)
        {
            Console.WriteLine("{0} : {1}", socket.RemoteEndPoint.ToString(), packet.data);
        }
    }
}
