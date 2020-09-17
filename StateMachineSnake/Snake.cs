using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StateMachineSnake
{
    public class Snake
    {
        private int _id;
        private static int _counter = 0;
        private int[] _headPosition;
        private int[] _startHeadPosition;
        private List<int[]> _body;
        private int _length;
        private int _startLength;
        private StateMachine _sm;
        private bool _active = false;

        private ConsoleKey _keyForward;
        private ConsoleKey _keyRight;
        private ConsoleKey _keyLeft;

        private int sleepTime = 100;
        private StateMachine.State toMove;

        public Snake(int[] startHeadPosition, int startLength, ConsoleKey keyForward, ConsoleKey keyLeft, ConsoleKey keyRight)
        {
            _id = _counter;
            _counter++;
            _headPosition = startHeadPosition;
            _startHeadPosition = startHeadPosition;
            _length = startLength;
            _startLength = startLength;

            _keyForward = keyForward;
            _keyRight = keyRight;
            _keyLeft = keyLeft;

            _body = new List<int[]>();
            _body.Add(startHeadPosition);
            _sm = new StateMachine();
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public int[] HeadPosition
        {
            get => _headPosition;
            set => _headPosition = value;
        }

        public int[] StartHeadPosition
        {
            get => _startHeadPosition;
            set => _startHeadPosition = value;
        }

        public List<int[]> Body
        {
            get => _body;
            set => _body = value;
        }

        public int Length
        {
            get => _length;
            set => _length = value;
        }
        public StateMachine Sm
        {
            get => _sm;
        }

        public bool Active
        {
            get => _active;
            set => _active = value;
        }

        public StateMachine.State ToMove
        {
            get => toMove;
            set => toMove = value;
        }

        public ConsoleKey KeyLeft
        {
            get => _keyLeft;
        }
        public ConsoleKey KeyRight
        {
            get => _keyRight;
        }
        public ConsoleKey KeyForward
        {
            get => _keyForward;
        }
        public void Move(StateMachine.State direction)
        {
            if (direction == StateMachine.State.North) HeadPosition[0] -= 1;
            else if (direction == StateMachine.State.South) HeadPosition[0] += 1;
            else if (direction == StateMachine.State.West) HeadPosition[1] -= 1;
            else if (direction == StateMachine.State.East) HeadPosition[1] += 1;

            if (_body.Count < _length) _body.Add((int[])HeadPosition.Clone());
            else
            {
                _body.RemoveAt(0);
                _body.Add((int[])HeadPosition.Clone());
            }
        }

        public void Reset()
        {
            _headPosition = (int[]) StartHeadPosition.Clone();
            _length = _startLength;
            _body = new List<int[]>();
            _body.Add((int[])HeadPosition.Clone());
            _active = true;
            _sm.ResetState();
            toMove = StateMachine.State.North;
            _active = true;

            //Action a1 = () =>
            //{
            //    //Making the snake move every x milliseconds
            //    while (Active)
            //    {
            //        Move(toMove);
            //        Thread.Sleep(sleepTime);
            //    }
            //};
            //Task.Run(a1);

            //Task.Run(() =>
            //{
            //    while (_active)
            //    {
            //        //if (!Console.KeyAvailable) continue;
            //        ConsoleKey key = Console.ReadKey(true).Key;
            //        if (key == _keyLeft) toMove = Sm.Output(StateMachine.Input.Left);
            //        else if (key == _keyRight) toMove = Sm.Output(StateMachine.Input.Right);
            //        else if (key == _keyForward) toMove = Sm.Output(StateMachine.Input.Forward);
            //    }
            //});
        }


    }
}
