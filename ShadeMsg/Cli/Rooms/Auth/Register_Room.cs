using System;
using System.Collections.Generic;
using System.Text;
using Console = Colorful.Console;
using System.Drawing;
using ShadeMsg.Network;
using ShadeMsg.Network.Packets;
using System.Text.RegularExpressions;
using System.Threading;
using ShadeMsg.Security;

namespace ShadeMsg.Cli.Rooms
{
    class Register_Room : Room
    {
        private Client client;
        private ResponseCatcher responseCatcher;

        public Register_Room(Client client)
        {
            this.client = client;
            client.NewPacket += Client_NewPacket;
        }

        public override void Show()
        {
            Console.WriteLine("Welcome in ShadeMSG now u can create a free account");
            string nick = Input("Nick: ");
            Console.WriteLine("It will not be possible to change your password in the future");
            string password = Input("Password: ");

            List<string> validationErrors = new List<string>();
            int minLenghtNick = 4;
            int maxLenghtNick = 16;
            int minLenghtPassword = 4;
            int maxLenghtPassword = 16;

            if (!DataValidation.CheckLenght(nick, minLenghtNick, maxLenghtNick)) { validationErrors.Add("Check nick lenght Min:"+ minLenghtNick +" Max:" + maxLenghtNick); }
            if (!DataValidation.CheckLenght(password,minLenghtPassword,maxLenghtPassword)) { validationErrors.Add("Check password lenght Min:" + minLenghtPassword + " Max:" + maxLenghtPassword); }
            if (DataValidation.RegexCheck(nick, "[^A-Za-z0-9_.]")) { validationErrors.Add("Nick have illegal characters, use only letters and numbers."); }
            if (DataValidation.RegexCheck(password, "[^A-Za-z0-9_.]")) { validationErrors.Add("Password have illegal characters, use only letters and numbers."); }

            if(validationErrors.Count > 0)
            {
                foreach (string error in validationErrors)
                {
                    Console.WriteLine(error, Color.Tomato);
                }
                return;
            }

            Packet reg_packet = new Packet() { name = "register", args = new Argument[] {
                    new Argument("nick",nick),
                    new Argument("password",password)
                }
            };

            Console.WriteLine("Creating Account..");

            client.Send(reg_packet);
            responseCatcher = new ResponseCatcher(reg_packet);
            responseCatcher.PacketCaught += ResponseCatcher_PacketCaught;
            while(!responseCatcher.packetWasCaught) { Thread.Sleep(100); }
        }

        private void ResponseCatcher_PacketCaught(Packet packet)
        {
            string res_error = packet.GetArgument("error").value;

            if (res_error != string.Empty)
            {
                Console.WriteLine("Ups.. {0}", res_error);
                Console.Read();
                return;
            }
            else
            {
                Console.WriteLine("Done! Success!", Color.GreenYellow);
                Console.Read();
                return;
            }
        }

        private void Client_NewPacket(Packet packet)
        {
            responseCatcher.Catch(packet);
        }

       
    }
}
