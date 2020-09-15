using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StateMachineSnake
{
    public class Worker
    {
        private int[] snakePosition;
        private int[] startSnakePosition = new[] { 10, 10 };
        private List<int[]> snakeTailPositions;
        private int snakeLenght;
        private List<int[]> food;
        private int numberOfFood;

        private int[,] playground;
        private StateMachine sm;
        private StateMachine.State toMove;
        private int playgroundWidth = 20;
        private int playgroundHeight = 20;
        bool playing = false;

        private int sleepTime = 100;

        Random ran = new Random();

        public void Init()
        {
            sm = new StateMachine();
            snakePosition = (int[])startSnakePosition.Clone();
            playground = new int[playgroundHeight, playgroundWidth];
            snakeTailPositions = new List<int[]>();
            snakeLenght = 1;
            numberOfFood = 3;
            food = new List<int[]>();
        }

        public void Start()
        {
            Init();

            while (true)
            {
                if (!playing)
                {
                    Thread.Sleep(100);
                    Console.Clear();
                    Console.WriteLine("PRESS ANY KEY TO START");
                    Console.ReadKey(true);
                    StartFresh(); //also sets playing to true
                }
            }


        }

        public void FoodChecker()
        {
            foreach (int[] snack in food)
            {
                //if there is a snack in the snake, either head, or body
                if (snakeTailPositions.Exists(x => x.SequenceEqual(snack)))
                {
                    snakeLenght++;
                    food.Remove(snack);
                    break;
                }
            }
            if (food.Count < numberOfFood)
            {
                food.Add(new []{ran.Next(0,playgroundHeight) , ran.Next(0, playgroundWidth)});
                FoodChecker();
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
                //
                //checks if the new location is out of bounds, and kills you
                //
                Console.WriteLine("YOU DIED - OUT OF BOUNDS");
                Console.WriteLine("PRESS KEY TO RETURN TO MAIN MENU");
                Console.ReadKey(true);
                playing = false;
            }
            else if (snakeTailPositions.Exists(x => x.SequenceEqual(snakePosition)))
            {
                //
                //checks if you hit your own body
                //
                Console.WriteLine("YOU DIED - TRIED TO EAT YOURSELF");
                Console.WriteLine("PRESS KEY TO RETURN TO MAIN MENU");
                Console.ReadKey(true);
                playing = false;
            }

            FoodChecker();

            if (snakeTailPositions.Count < snakeLenght) snakeTailPositions.Add((int[])snakePosition.Clone());
            else
            {
                snakeTailPositions.RemoveAt(0);
                snakeTailPositions.Add((int[])snakePosition.Clone());
            }



            DrawPlayground();
        }

        private void DrawPlayground()
        {
            Console.Clear();
            Console.WriteLine($"SNAKE AT: [{snakePosition[0]} , {snakePosition[1]}] , LENGTH: {snakeLenght}");
            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    int[] currentPosition = new[] {i, j};
                    if (snakeTailPositions.Exists(x=>x.SequenceEqual(currentPosition)))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("X" + " ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (food.Exists(x=>x.SequenceEqual(currentPosition)))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("O" + " ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else Console.Write("." + " ");
                    //else Console.Write(playground[i, j] + " ");
                }
                Console.WriteLine();
            }
        }



        private void StartFresh()
        {
            Action a1 = () =>
            {
                //Making the snake move every x milliseconds
                while (playing)
                {
                    MoveSnake(toMove);
                    Thread.Sleep(sleepTime);
                }
            };

            Action a2 = () =>
            {
                //Reading the input from the user and setting toMove to the new state
                while (playing)
                {
                    if (!Console.KeyAvailable) continue;
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.A) toMove = sm.Output(StateMachine.Input.Left);
                    else if (key == ConsoleKey.D) toMove = sm.Output(StateMachine.Input.Right);
                    else if (key == ConsoleKey.W) toMove = sm.Output(StateMachine.Input.Forward);
                }
            };

            playing = true;
            snakePosition = (int[])startSnakePosition.Clone();
            toMove = StateMachine.State.North;
            snakeTailPositions.Clear();
            snakeTailPositions.Add((int[])snakePosition.Clone());
            food.Clear();
            snakeLenght = 1;
            sm.ResetState();
            Task.Run(a1);
            Task.Run(a2);
        }
    }
}
