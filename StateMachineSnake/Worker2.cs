using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StateMachineSnake
{
    public class Worker2
    {
        private Snake player1;
        private Snake player2;
        private Snake player3;
        private List<Snake> allPlayersAlive = new List<Snake>(); //all players alive

        private List<Snake> allPlayersToReset = new List<Snake>();

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
            player1 = new Snake(new[] { 10, 10 }, 5, ConsoleKey.W, ConsoleKey.A, ConsoleKey.D);
            player2 = new Snake(new []{ 14, 5 }, 4, ConsoleKey.UpArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow);
            //player3 = new Snake(new []{ 14, 15 }, 8, ConsoleKey.NumPad8, ConsoleKey.NumPad4, ConsoleKey.NumPad6);
            allPlayersAlive.Add(player1);
            allPlayersAlive.Add(player2);
            //allPlayersAlive.Add(player3);
            allPlayersToReset.Add(player1);
            allPlayersToReset.Add(player2);
            //allPlayersToReset.Add(player3);
            //sm = new StateMachine();
            //snakePosition = (int[])startSnakePosition.Clone();
            playground = new int[playgroundHeight, playgroundWidth];
            //snakeTailPositions = new List<int[]>();
            //snakeLenght = 1;
            numberOfFood = 10;
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
            foreach (Snake player in allPlayersAlive)
            {
                foreach (int[] snack in food)
                {
                    //if there is a snack in the snake, either head, or body
                    if (player.Body.Exists(x => x.SequenceEqual(snack)))
                    {
                        player.Length++;
                        food.Remove(snack);
                        break;
                    }
                }
                if (food.Count < numberOfFood)
                {
                    food.Add(new[] { ran.Next(0, playgroundHeight), ran.Next(0, playgroundWidth) });
                    FoodChecker();
                }
            }
        }

        private void MoveSnake()
        {
            //foreach (Snake player in allPlayersAlive)
            //{
            //    player.Move(player.ToMove);
            //}

            #region killing stuff

            foreach (Snake player in allPlayersAlive)
            {
                if (!player.Active) continue;
                player.Move(player.ToMove);
                if (player.HeadPosition[0] < 0 || player.HeadPosition[0] > playgroundHeight - 1 || player.HeadPosition[1] < 0 || player.HeadPosition[1] > playgroundWidth - 1)
                {
                    //
                    //checks if the new location is out of bounds, and kills you
                    //
                    Console.WriteLine("YOU DIED - OUT OF BOUNDS");
                    //Console.WriteLine("PRESS \"R\" TO RETURN TO MAIN MENU");
                    //while (true)
                    //{
                    //    ConsoleKeyInfo key = Console.ReadKey(true);
                    //    if (key.Key == ConsoleKey.R)
                    //    {
                    //        break;
                    //    }
                    //}

                    player.Active = false;
                    //player.Reset();
                }
                else if (player.Body.Exists(x => x.SequenceEqual(player.HeadPosition)))
                {
                    if (player.Body.Last().SequenceEqual(player.HeadPosition) && player.Body.FindAll(x=>x.SequenceEqual(player.HeadPosition)).Count < 2) continue;
                    //
                    //checks if you hit your own body
                    //
                    Console.WriteLine("YOU DIED - TRIED TO EAT YOURSELF");
                    //Console.WriteLine("PRESS \"R\" TO RETURN TO MAIN MENU");
                    //while (true)
                    //{
                    //    ConsoleKeyInfo key = Console.ReadKey(true);
                    //    if (key.Key == ConsoleKey.R)
                    //    {
                    //        break;
                    //    }
                    //}

                    player.Active = false;
                    //player.Reset();
                }
            }


            //THIS TRIGGERS WHEN ALL PLAYERS ARE DEAD
            if (allPlayersAlive.FindAll(x => x.Active).Count == 0)
            {
                playing = false;
                Console.WriteLine("YOU DIED ALL DIED");
                Console.WriteLine("PRESS \"R\" TO RETURN TO MAIN MENU");
                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.R)
                    {
                        break;
                    }
                }
            }

            #endregion

            FoodChecker();



            DrawPlayground();
        }

        private void DrawPlayground()
        {
            Console.Clear();
            string borderChar = "X";
            string topAndBottomBorder = String.Concat(Enumerable.Repeat(borderChar, playground.GetLength(1) * 2 + 2));
            List<int[]> allSnakeBodies = new List<int[]>();
            foreach (Snake player in allPlayersAlive)
            {
                if (player.Active) allSnakeBodies.AddRange(player.Body);
            }
            //Console.WriteLine($"SNAKE AT: [{snakePosition[0]} , {snakePosition[1]}] , LENGTH: {snakeLenght}");
            Console.WriteLine(topAndBottomBorder);
            for (int i = 0; i < playground.GetLength(0); i++)
            {
                Console.Write(borderChar);
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    int[] currentPosition = new[] { i, j };
                    if (allSnakeBodies.Exists(x => x.SequenceEqual(currentPosition)))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("X" + " ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (food.Exists(x => x.SequenceEqual(currentPosition)))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("O" + " ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else Console.Write(" " + " ");
                    //else Console.Write(playground[i, j] + " ");
                }
                Console.Write(borderChar);
                Console.WriteLine();
            }
            Console.WriteLine(topAndBottomBorder);

            Console.WriteLine();
        }



        private void StartFresh()
        {
            Action a1 = () =>
            {
                //Making the snake move every x milliseconds
                while (playing && allPlayersAlive.FindAll(x => x.Active).Count != 0)
                {
                    MoveSnake();
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

                    foreach (Snake player in allPlayersAlive)
                    {
                        if (player.Active)
                        {
                            if (key == player.KeyLeft) player.ToMove = player.Sm.Output(StateMachine.Input.Left);
                            else if (key == player.KeyRight) player.ToMove = player.Sm.Output(StateMachine.Input.Right);
                            else if (key == player.KeyForward) player.ToMove = player.Sm.Output(StateMachine.Input.Forward);
                        }
                    }
                }
            };

            playing = true;

            foreach (Snake player in allPlayersAlive)
            {
                player.Reset();
            }

            food.Clear();
            Task.Run(a1);
            Task.Run(a2);
        }
    }
}
