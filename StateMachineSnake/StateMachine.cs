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

        //	        north	south	east	west
        //forward	1	    2	    3	    4
        //right	    3	    4	    2	    1
        //left	    4	    3	    1	    2

        public int[,] Table = new int[,]
        {
            { 0, 1, 2, 3 }, 
            { 2, 3, 1, 0 }, 
            { 3, 2, 0, 1 }
        };

        

        public State SetState(State s, Input i)
        {
            _state = (State) Enum.ToObject(typeof(State), Table[(int)i, (int)s]);
            return _state;

            #region if else
            /*
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
            */
            #endregion
        }

        public State Output(Input i)
        {
            SetState(_state, i);
            return _state;
        }

        public void ResetState()
        {
            _state = State.North;
        }
    }
}
