using System;

namespace StateMachineSnake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            //Worker w = new Worker();
            //w.Start();

            Worker2 w2 = new Worker2();
            w2.Start();

            Console.WriteLine("PROGRAM IS DONE PRESS A KEY");
            Console.ReadKey();

        }
    }
}
