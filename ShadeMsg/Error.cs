using System;
using System.Collections.Generic;
using System.Text;

namespace ShadeMsg
{
    class Error
    {
        public Error(string msg,int exitCode)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
            Console.ReadKey();
            Environment.Exit(exitCode);
        }
    }
}
