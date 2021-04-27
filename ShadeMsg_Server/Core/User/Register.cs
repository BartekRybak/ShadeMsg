using System;
using System.Collections.Generic;
using System.Text;
using ShadeMsg.Network.Packets;
using ShadeMsg_Server.Network;
using ShadeMsg_Server.DataBase;

namespace ShadeMsg_Server.Core
{
    class Register
    {
        private Packet packet;
        private Client client;

        public Register(Packet packet,Client client)
        {
            this.packet = packet;
            this.client = client;

            string nick = packet.GetArgument("nick").value;
            string password = packet.GetArgument("password").value;
            string error = string.Empty;

            if(!DB_Users.UserExits(nick))
            {
                DB_Users.CreateNewUser(nick, password);
                Console.WriteLine("Creatgin New user [{0}]",nick);
            }
            else
            {
                error = "This nickname is alredy taken";
            }

            Packet res_packet = new Packet() { name = "register", args = new Argument[] {
                new Argument("type","response"),
                new Argument("error",error)
            }};

            Program.server.Send(res_packet, client);
        }
    }
}
