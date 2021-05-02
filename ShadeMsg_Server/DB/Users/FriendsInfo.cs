using System;
using System.Collections.Generic;
using System.Text;

namespace ShadeMsg_Server.DB
{
    public class FriendsInfo
    {
        public string nick;
        public string[] friends;
        public string[] invites_in;
        public string[] invites_out;
        public string[] blocked;

        public FriendsInfo()
        {
            nick = string.Empty;
            friends = new string[] { };
            invites_in = new string[] { };
            invites_out = new string[] { };
            blocked = new string[] { };
        }

        public FriendsInfo(string nick,string[] friends,string[] invites_in,string[] invites_out,string[] blocked)
        {
            this.nick = nick;
            this.friends = friends;
            this.invites_in = invites_in;
            this.invites_out = invites_out;
            this.blocked = blocked;
        }
    }
}
