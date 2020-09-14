using System;

namespace StateMachineSnake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            Worker w = new Worker();
            w.Start();

            Console.WriteLine("PROGRAM IS DONE PRESS A KEY");
            Console.ReadKey();

        }
    }
}
