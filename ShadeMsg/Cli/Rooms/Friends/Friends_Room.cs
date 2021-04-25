using System;
using System.Collections.Generic;
using System.Text;
using ShadeMsg.Network;
using ShadeMsg.Network.Packets;

namespace ShadeMsg.Cli.Rooms
{
    class Friends_Room : Room
    {
        public Friends_Room(Client client)
        {
            switch(Input("$>", new string[] { "/list","/add","/dell","/block","/msg" }))
            {
                case "/list":
                    break;
                case "/add":
                    break;
                case "/dell":
                    break;
                case "/block":
                    break;
                case "/msg":
                    break;
            }
        }

        public override void Show()
        {


            base.Show();
        }
    }
}
