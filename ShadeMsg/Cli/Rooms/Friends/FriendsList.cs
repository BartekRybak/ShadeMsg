using System;
using System.Collections.Generic;
using System.Text;
using ShadeMsg.Network;
using ShadeMsg.Network.Packets;

namespace ShadeMsg.Cli.Rooms.Friends
{
    class FriendsList
    {
        private ResponseCatcher responseCatcher;
        public FriendsList(Client client)
        {
            client.NewPacket += Client_NewPacket;
            Packet list_packet = new Packet()
            {
                name = "friends-list",
                args = new Argument[] {
                    new Argument("nick",client.nick)
                }
            };

            client.Send(list_packet);
            responseCatcher = new ResponseCatcher(list_packet);
            responseCatcher.PacketCaught += ResponseCatcher_PacketCaught;
            while (!responseCatcher.packetWasCaught) { }
        }

        private void ResponseCatcher_PacketCaught(Packet packet)
        {
            string[] friends = packet.GetArgument("friends").value.Split('+');

            if (friends.Length <= 0)
            {
                Console.WriteLine("You dont have friends");
            }
            else
            {
                Console.WriteLine("All your friends:");
                foreach (string f in friends)
                {
                    Console.WriteLine(f);
                }
            }
            Console.ReadLine();
        }

        private void Client_NewPacket(Packet packet)
        {
            responseCatcher.Catch(packet);
        }
    }
}
