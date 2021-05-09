using System;
using System.Collections.Generic;
using System.Text;
using ShadeMsg.Network.Packets;
using ShadeMsg_Server.Network;
using ShadeMsg_Server.DB;

namespace ShadeMsg_Server.Core.User.Friends
{
    class FriendsList
    {
        public FriendsList(Client client,Packet packet)
        {
            string nick = packet.GetArgument("nick").value;
            FriendsInfo friendsInfo = DataBase.Users.Friends.GetFriendsInfo(nick);
            string friends = DataBase.ArrayToField(friendsInfo.friends, '+');
            Packet resPacket = new Packet()
            {
                name = "friends-list",
                args = new Argument[] {
                    new Argument("type","response"),
                    new Argument("friends",friends)
                }
            };
            Program.server.Send(resPacket, client);
            Console.WriteLine("wysłane : {0}",friends);
           
        }
    }
}
