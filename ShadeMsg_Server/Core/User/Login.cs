using System;
using System.Collections.Generic;
using System.Text;
using ShadeMsg.Network.Packets;
using ShadeMsg_Server.Network;
using ShadeMsg_Server.DataBase;
using ShadeMsg.Network.Packets;

namespace ShadeMsg_Server.Core.User
{
    class Login
    {
        private Client client;
        public Login(Packet packet,Client client)
        {
            this.client = client;

            string nick = packet.GetArgument("nick").value;
            string password = packet.GetArgument("password").value;

            string error = string.Empty;

            if(DB_Users.LogIn(nick,password))
            {
                Console.WriteLine("logged {0}", nick);
            }
            else
            {
                Console.WriteLine("error {0}", "pierdole");
            }

            Packet login_response = new Packet() { name = "login", args = new Argument[] {
                new Argument("type","response"),
                new Argument("error",error)
            } };

            Program.server.Send(login_response,client);
        }
    }
}
