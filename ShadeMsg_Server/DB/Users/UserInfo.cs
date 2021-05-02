using System;
using System.Collections.Generic;
using System.Text;

namespace ShadeMsg_Server.DB
{
    public class UserInfo
    {
        public string id;
        public string nick;
        public string key;
        public string iv;

        public UserInfo()
        {
            id = string.Empty;
            nick = string.Empty;
            key = string.Empty;
            iv = string.Empty;
        }

        public UserInfo(string id, string nick, string key, string iv)
        {
            this.id = id;
            this.nick = nick;
            this.key = key;
            this.iv = iv;
        }

        public override string ToString()
        {
            string res = string.Empty;
            res += "id:" + id + "\n";
            res += "nick:" + nick + "\n";
            res += "key:" + key + "\n";
            res += "iv:" + iv + "\n";
            return res;
        }
    }
}
