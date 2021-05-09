using System;
using System.Collections.Generic;
using System.Text;
using ShadeMsg.Network;
using ShadeMsg.Security;
using ShadeMsg.Network.Packets;
using Console = Colorful.Console;
using System.Drawing;
using System.Threading;

namespace ShadeMsg.Cli.Rooms
{
    class Login_Room : Room
    {
        private Client client;
        private ResponseCatcher responseCatcher;
        private static string nick;
        public Login_Room(Client client)
        {
            this.client = client;
            this.client.NewPacket += Client_NewPacket;
        }

        public override void Show()
        {
            Console.WriteLine("Login in to your account");
            nick = Input("Nick: ");
            string password = Input("Password: ");

            List<string> validationErrors = new List<string>();
            int minLenghtNick = 4;
            int maxLenghtNick = 16;
            int minLenghtPassword = 4;
            int maxLenghtPassword = 16;

            if (!DataValidation.CheckLenght(nick, minLenghtNick, maxLenghtNick)) { validationErrors.Add("Check nick lenght Min:" + minLenghtNick + " Max:" + maxLenghtNick); }
            if (!DataValidation.CheckLenght(password, minLenghtPassword, maxLenghtPassword)) { validationErrors.Add("Check password lenght Min:" + minLenghtPassword + " Max:" + maxLenghtPassword); }
            if (DataValidation.RegexCheck(nick, "[^A-Za-z0-9_.]")) { validationErrors.Add("Nick have illegal characters, use only letters and numbers."); }
            if (DataValidation.RegexCheck(password, "[^A-Za-z0-9_.]")) { validationErrors.Add("Password have illegal characters, use only letters and numbers."); }

            if (validationErrors.Count > 0)
            {
                foreach (string error in validationErrors)
                {
                    Console.WriteLine(error, Color.Tomato);
                }
                return;
            }

            Packet login_packet = new Packet() { name = "login", args = new Argument[] {
                new Argument("nick",nick),
                new Argument("password",password)
            } };

            
            client.Send(login_packet);
            responseCatcher = new ResponseCatcher(login_packet);
            responseCatcher.PacketCaught += ResponseCatcher_PacketCaught;
            while (!responseCatcher.packetWasCaught) { }
        }

        private void ResponseCatcher_PacketCaught(Packet packet)
        {
            Console.WriteLine("MAM KURWA");
            string error = packet.GetArgument("error").value;
            
            if (error == string.Empty)
            {
                Console.Clear();
                Console.WriteLine("Logged in, welcome in ShadeMSG", Color.GreenYellow);
                Program.client.logged = true;
                Program.client.nick = nick;
                return;
            }
            else
            {
                Console.WriteLine(error, Color.Tomato);
                return;
            }
        }

        private void Client_NewPacket(Packet packet)
        {
            responseCatcher.Catch(packet);
        }

    }
}
