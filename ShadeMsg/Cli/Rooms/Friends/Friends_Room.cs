using System;
using System.Collections.Generic;
using System.Text;
using ShadeMsg.Network;
using ShadeMsg.Network.Packets;
using ShadeMsg.Cli.Rooms.Friends;

namespace ShadeMsg.Cli.Rooms
{
    class Friends_Room : Room
    {
        private Client client;

        public Friends_Room(Client client)
        {
            this.client = client;
        }

        public override void Show()
        {
            switch (Input("$>", new string[] { "/list", "/add", "/dell", "/block", "/unblock", "/msg", "/back" }))
            {
                case "/list":
                    new FriendsList(client);
                    break;
                case "/add":
                    break;
                case "/delete":
                    break;
                case "/block":
                    break;
                case "/unblock":
                    break;
                case "/msg":
                    break;
                case "/back":
                    return;
            }
            base.Show();
        }

        private void List(Client client)
        {
           
            
        }

        

       
    }
}
