using System;
using ShadeMsg;
using ShadeMsg.Security;
using ShadeMsg.Network;
using ShadeMsg_Server.Network;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using ShadeMsg.Network.Packets;
using ShadeMsg_Server.Core;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;
using ShadeMsg_Server.DB;
namespace ShadeMsg_Server
{
    class Program
    {
        public static List<Client> clients = new List<Client>();
        static readonly string password = "1234";
        public static Server server = new Server(8001, password);

        static void Main(string[] args)
        {
            Test();
            Console.Read();

            Console.WriteLine("Starting Server..");
            server.Start();
            Console.Write("Done!\n");

            server.NewSocketConnected += Server_NewSocketConnected;

            while (true) { 
                
                Thread.Sleep(1000); 
            } 
        }



        private static void Test()
        {
            //SQLiteConnection.CreateFile("DataBase/Users.db");
           // DataBase.Users.CreateEmptyTables();
            //DataBase.Users.CreateNew("test1", "qwerty");

            /* RESET
            SQLiteConnection.CreateFile("DataBase/Users.db");
                DB_Users.CreateEmptyTable();
                DB_Friends.CreateEmptyTable();
            */

           
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
        /// <param name="client"></param>
        private static void Client_NewPacket(Packet packet, Client client)
        {
            if(packet != Packet.Empty)
            {
                Console.WriteLine(packet.ToString());

                switch(packet.name)
                {
                    case "register":
                            new Register(packet, client);
                        break;
                    case "login":
                        new Login(packet, client);
                        break;
                }
            }
        }
    }
}
