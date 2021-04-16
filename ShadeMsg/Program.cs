using System;
using System.Text;
using ShadeMsg.Network;
using ShadeMsg.Security;

namespace ShadeMsg
{
    class Program
    {
        static void Main(string[] args)
        {
            Server s = new Server("asdf", 1234);

            Console.ReadKey();
        }
    }
}
