using UnityEngine;

namespace Snake4D
{
    public class SnakeFood
    {
        public Vector4Int Position => _position;
        public bool HasSpawned => _hasSpawned;

        SnakeGameParameters _parameters;
        SnakeBody _snakeBody;
        Vector4Int _position;
        Vector4Int _previousPosition;
        bool _hasSpawned = false;

        public SnakeFood(SnakeGameParameters parameters, SnakeBody snakeBody)
        {
            _parameters = parameters;
            _snakeBody = snakeBody;

            AttemptToSpawn();
            _previousPosition = _position;
        }

        /// <summary>
        /// Returns true if snake food has been eaten.
        /// </summary>
        /// <returns></returns>
        public bool HasSnakeAteFood()
        {
            if (_snakeBody.SnakeHead.Position == _position)
            {
                _previousPosition = _position;
                _hasSpawned = false;
                return true;
            }

            return false;
        }

        public Vector4Int GetPreviousPosition()
        {
            return _previousPosition;
        }

        /// <summary>
        /// Returns true if successfully spawned.
        /// </summary>
        /// <returns></returns>
        public bool AttemptToSpawn()
        {
            if (Utilities.NumberOfUnoccupiedPositions(_snakeBody, _parameters.Dimension, _parameters.Size) == 0)
            {
                return false;
            }

            SetPositionToRandomUnoccupiedPosition();
            _hasSpawned = true;
            return true;
        }

        private void SetPositionToRandomUnoccupiedPosition()
        {
            _position = Utilities.GenerateRandomUnoccupiedPosition(_parameters.Dimension, _parameters.Size, _snakeBody);
        }
    }
}