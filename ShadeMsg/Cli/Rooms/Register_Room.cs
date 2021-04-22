using System;
using System.Collections.Generic;
using System.Text;
using Console = Colorful.Console;
using System.Drawing;
using ShadeMsg.Network;
using ShadeMsg.Network.Packets;
using System.Text.RegularExpressions;
using System.Threading;

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

            // user data validation
            string error = string.Empty;
            // nick
            string nick_error = UserInputValidation(nick);
            string passwd_error = UserInputValidation(password);

            if(nick_error != string.Empty)
            {
                Console.Write("Nick: ", Color.Tomato);
                Console.WriteLine(nick_error);
                Console.Read();
                Show();
            }

            if(passwd_error != string.Empty)
            {
                Console.Write("Password: ", Color.Tomato);
                Console.WriteLine(passwd_error);
                Console.Read();
                Show();
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

        private string UserInputValidation(string text)
        {
            return string.Empty;
        }
    }
}
