using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace ShadeMsg.Network.Packets
{
    public struct Argument
    {
        public string name;
        public string value;

        public Argument(string name,string value)
        {
            this.name = name;
            this.value = value;
        }
    }

    public class Packet
    {
        private int id;
        public string name = string.Empty;
        public Argument[] args;

        public Packet()
        {
            Random rand = new Random();
            id = rand.Next(int.MaxValue);
        }

        public Argument GetArgument(string name)
        {
            Argument argument = new Argument();
            foreach(Argument arg in args)
            {
                if(arg.name == name)
                {
                    argument = arg;
                }
            }
            return argument;
        }

        public static Packet Empty
        {
            get
            {
                return new Packet();
            }
        }

        public override string ToString()
        {
            string text = "Name:" + name + "\n";
            
            foreach(Argument arg in args)
            {
                text += "[" + arg.name + "] - [" + arg.value + "] \n";
            }
            return text;
        }
    }
}
