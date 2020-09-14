using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StateMachineSnake
{
    public class Worker
    {
        private int[] snakePosition;
        private int[] startSnakePosition = new[] { 10, 10 };
        private int[,] playground;
        private StateMachine sm;
        private StateMachine.State toMove;
        private int playgroundWidth = 20;
        private int playgroundHeight = 20;
        bool playing = false;

        public void Start()
        {
            sm = new StateMachine();
            snakePosition = (int[])startSnakePosition.Clone();
            playground = new int[playgroundHeight, playgroundWidth];

            Action a1 = () =>
            {
                //Making the snake move every x milliseconds
                while (playing)
                {
                    MoveSnake(toMove);
                    Thread.Sleep(100);
                }
            };

            Action a2 = () =>
            {
                //Reading the input from the user and setting toMove to the new state
                while (playing)
                {
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.A) toMove = sm.Output(StateMachine.Input.Left);
                    else if (key == ConsoleKey.D) toMove = sm.Output(StateMachine.Input.Right);
                    else if (key == ConsoleKey.W) toMove = sm.Output(StateMachine.Input.Forward);
                }
            };

            while (true)
            {
                if (!playing)
                {
                    Thread.Sleep(100);
                    Console.Clear();
                    Console.WriteLine("PRESS ANY KEY TO START");
                    Console.ReadKey();

                    playing = true;
                    snakePosition = (int[])startSnakePosition.Clone();
                    toMove = StateMachine.State.North;
                    Task.Run(a1);
                    Task.Run(a2);
                }
            }


        }

        private void MoveSnake(StateMachine.State direction)
        {
            if (direction == StateMachine.State.North) snakePosition[0] -= 1;
            else if (direction == StateMachine.State.South) snakePosition[0] += 1;
            else if (direction == StateMachine.State.West) snakePosition[1] -= 1;
            else if (direction == StateMachine.State.East) snakePosition[1] += 1;

            if (snakePosition[0] < 0 || snakePosition[0] > playgroundHeight-1 || snakePosition[1] < 0 || snakePosition[1] > playgroundWidth-1)
            {
                playing = false;
                //throw new Exception($"[{snakePosition[0]} , {snakePosition[1]}] GET FUCKED");
            }

            DrawPlayground();
        }

        private void DrawPlayground()
        {
            Console.Clear();
            Console.WriteLine($"SNAKE AT: [{snakePosition[0]} , {snakePosition[1]}]");
            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    if (i == snakePosition[0] && j == snakePosition[1])
                    {
                        Console.Write("X" + " ");
                    }
                    else Console.Write("." + " ");
                    //else Console.Write(playground[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
