using System;
using System.Text;
using ShadeMsg.Network;
using ShadeMsg.Security;
using System.Threading;
using ShadeMsg.Network.Packets;
using System.Collections.Specialized;
using ShadeMsg.Cli.Rooms;
using Console = Colorful.Console;
using System.Drawing;

namespace ShadeMsg
{
    class Program
    {
        static string nick = string.Empty;

        static void Main(string[] args)
        {
            Console.WriteLine("ShadeMSG",Color.Tomato);
            Console.WriteLine("Press any key to continue...");
            Console.Clear();
            Console.WriteLine("Connecting..");
            Client client = new Client("127.0.0.1", 8001,"1234");
            client.Connect();
            client.NewPacket += Client_NewPacket;
            Console.Write(" Sucess!");
            Console.Clear();

            Console.Write("Your Nickname: ");
            nick = Console.ReadLine();

            Packet nick_packet = new Packet() { name = "nick", args = new Argument[] { 
                new Argument("nick",nick)
            } };

            client.Send(nick_packet);

            while (true) {
                string msg_text = Console.ReadLine();

                Packet msg_packet = new Packet() { name = "msg",args = new Argument[] { 
                    new Argument("text",msg_text)
                } };
                client.Send(msg_packet);

                Thread.Sleep(100); 
            }
        }

        private static void Client_NewPacket(Packet packet)
        {
            if(packet != Packet.Empty)
            {
                if(packet.name == "msg")
                {
                    Console.WriteLine("{0} : {1}", packet.GetArgument("nick").value, packet.GetArgument("text").value);
                }
            }
        }
    }
}
