using System;
using System.Text;
using ShadeMsg.Network;
using ShadeMsg.Security;

namespace ShadeMsg
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Server s = new Server("asdf", 1234);
            Packet p = new Packet() { data = "elo co tam" };
            string packet_c = PacketEncryption.EncryptPacket(p, "smok20122");
            Console.WriteLine(PacketEncryption.DecryptPacket(packet_c, "smok20122").data);

            Console.ReadKey();
        }
    }
}
