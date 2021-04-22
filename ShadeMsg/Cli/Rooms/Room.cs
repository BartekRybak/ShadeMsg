using System;
using System.Collections.Generic;
using System.Text;
using Console = Colorful.Console;
using System.Drawing;

namespace ShadeMsg.Cli
{
    class Room
    {
        public string title
        {
            set
            {
                Console.Title = value;
            }
        }

        public virtual void Show()
        {
           
        }

        public string Input(string prompt)
        {
            Console.Write(prompt, Color.Yellow);
            Console.ForegroundColor = Color.Cyan;
            string text = Console.ReadLine();
            Console.ResetColor();
            return text;
        }

        public string Input(string prompt,string[] avalilableCommands)
        {
            foreach(string command in avalilableCommands)
            {
                Console.Write(command + ' ', Color.BlueViolet);
            }

            Console.Write(prompt,Color.Yellow);
            Console.ForegroundColor = Color.Cyan;
            string text = Console.ReadLine();
            Console.ResetColor();
            return text;
        }
    }
}
