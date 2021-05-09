using System;
using System.Collections.Generic;
using System.Text;
using ShadeMsg.Network.Packets;
using ShadeMsg_Server.Network;
using ShadeMsg_Server.DB;

namespace ShadeMsg_Server.Core
{
    class Login
    {
        public Login(Packet packet,Client client)
        {
            string nick = packet.GetArgument("nick").value;
            string password = packet.GetArgument("password").value;
            string error = string.Empty;

            if(!DataBase.Users.GetAuth(nick,password)) { error = "Wrong password or nickname"; }
            if (DataBase.Users.IsBanned(nick)) { error = "Your account is banned :P"; }
            
            Packet login_response = new Packet() { name = "login", args = new Argument[] {
                new Argument("type","response"),
                new Argument("error",error)
            } };

            Program.server.Send(login_response,client);
        }
    }
}
