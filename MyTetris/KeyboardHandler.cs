using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MyTetris
{
    public class KeyboardHandler
    {

        private Thread myThread;

        private void CallToInputThread()
        { 
            while (listening == true)
            {

                PressedKey = Console.ReadKey(true).Key;
            }
        }

        public bool listening { get; private set; }

        public ConsoleKey? PressedKey { get; set; }

        public KeyboardHandler()
        {
            listening = true;

            ThreadStart newThread = new ThreadStart(CallToInputThread);
            myThread = new Thread(newThread);
            myThread.Start();


        }
    }
}
