using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineSnake
{
    public class StateMachine
    {
        private State _state = State.North;
        
        public enum State
        {
            North, South, East, West
        }
        public enum Input
        {
            Forward, Right, Left
        }

        public State SetState(State s, Input i)
        {
            if (s == State.North)
            {
                if (i == Input.Forward) _state = State.North;
                else if (i == Input.Left) _state = State.West;
                else if (i == Input.Right) _state = State.East;
            }
            else if (s == State.West)
            {
                if (i == Input.Forward) _state = State.West;
                else if (i == Input.Left) _state = State.South;
                else if (i == Input.Right) _state = State.North;
            }
            else if (s == State.South)
            {
                if (i == Input.Forward) _state = State.South;
                else if (i == Input.Left) _state = State.East;
                else if (i == Input.Right) _state = State.West;
            }
            else if (s == State.East)
            {
                if (i == Input.Forward) _state = State.East;
                else if (i == Input.Left) _state = State.North;
                else if (i == Input.Right) _state = State.South;
            }

            return _state;
        }

        public State Output(Input i)
        {
            SetState(_state, i);
            return _state;
        }
    }
}
