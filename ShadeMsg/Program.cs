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

            Packet p = new Packet() { data = "testownaie na ostro kurde felek" };

            Console.WriteLine("szyfrowanie");
            string crypted = PacketEncryption.EncryptPacket(p, "1234");
            Console.WriteLine("deszyfrownaie");
            Console.WriteLine(PacketEncryption.DecryptPacket(crypted, "1234").data);

            Console.ReadKey();
        }
    }
}
