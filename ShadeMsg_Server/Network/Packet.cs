using System;
using System.Collections.Generic;
using System.Text;

namespace ShadeMsg.Network
{
    class Packet
    {
        public string data = string.Empty;

        public static Packet Empty
        {
            get
            {
                return new Packet();
            }
        }
    }
}
