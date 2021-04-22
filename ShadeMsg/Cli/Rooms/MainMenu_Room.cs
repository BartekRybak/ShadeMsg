using System;
using System.Collections.Generic;
using System.Text;

namespace ShadeMsg.Cli.Rooms
{
    class MainMenu_Room : Room
    {
        public MainMenu_Room()
        {
            title = "MainMenu";
        }

        public override void Show()
        {
                switch (Input("$>", new string[] { "/register", "/login", "/info", "/exit" }))
                {
                    case "/register":
                        new Register_Room(Program.client).Show();
                        break;
                    case "/login":
                        break;
                    case "/info":
                        break;
                    case "/exit":
                        return;
                    default:
                        Console.WriteLine("Unknown Command");
                        Show();
                        break;
                }
            base.Show();
        }
    }
}
