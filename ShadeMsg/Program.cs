using System;
using System.Text;
using ShadeMsg.Network;
using ShadeMsg.Security;
using System.Threading;

namespace ShadeMsg
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Client client = new Client("127.0.0.1", 8001,"smok20122");
            client.Connect();
            Console.WriteLine("Connected");
            client.NewPacket += Client_NewPacket;
           
            while (true) {
                string text = Console.ReadLine();
                client.Send(new Packet() { data = text });
                Thread.Sleep(1000); 
            }
        }

        private static void Client_NewPacket(Packet packet)
        {
            Console.WriteLine(packet.data);
        }
    }
}
