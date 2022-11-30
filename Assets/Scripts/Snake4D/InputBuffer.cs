using System.Collections.Generic;

namespace Snake4D
{
    public class InputBuffer
    {
        Queue<UserInputType> _inputQueue = new Queue<UserInputType>();
        int _bufferCapacity;

        public InputBuffer(int bufferCapacity)
        {
            _bufferCapacity = bufferCapacity;
        }

        /// <summary>
        /// Returns true if input is valid.
        /// Input is valid for the current dimension and if it is not repeated.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="dimension"></param>
        public bool AddInput(UserInputType input, Dimension dimension)
        {
            if (dimension == Dimension.DimensionOne && (int)input > (int)UserInputType.X_Negative)
                return false;

            if (dimension == Dimension.DimensionTwo && (int)input > (int)UserInputType.Y_Negative)
                return false;

            if (dimension == Dimension.DimensionThree && (int)input > (int)UserInputType.Z_Negative)
                return false;

            // Prevent the same input from being added multiple times
            if (_inputQueue.Count > 0 && _inputQueue.Peek() == input)
                return false;

            if (_inputQueue.Count == _bufferCapacity)
            {
                _inputQueue.Dequeue();
            }

            _inputQueue.Enqueue(input);
            return true;
        }

        public UserInputType GetInput()
        {
            if (_inputQueue.Count > 0)
                return _inputQueue.Dequeue();

            return UserInputType.NONE;
        }

        public override string ToString()
        {
            string output = "InputBuffer{";

            for (int i = 0; i < _inputQueue.Count; i++)
            {
                output += _inputQueue.ToArray()[i].ToString();
                if (i != _inputQueue.Count - 1)
                {
                    output += ", ";
                }
            }
            output += "}";

            return output;
        }
    }
}