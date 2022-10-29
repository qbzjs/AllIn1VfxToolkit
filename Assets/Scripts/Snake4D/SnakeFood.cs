using UnityEngine;

namespace Snake4D
{
    public class SnakeFood
    {
        public Vector4Int Position => _position;

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
        /// Returns true if snake food has spawned.
        /// </summary>
        /// <returns></returns>
        public bool UpdateSnakeFood()
        {
            bool hasSpawned = false;

            // "Spawn" food when eaten, meaning the snake head position is over the food
            if (_snakeBody.SnakeHead.Position == _position)
            {
                _previousPosition = _position;
                hasSpawned = AttemptToSpawn();
            }

            return hasSpawned;
        }

        public Vector4Int GetPreviousPosition()
        {
            return _previousPosition;
        }

        /// <summary>
        /// Returns true if successfully spawned.
        /// </summary>
        /// <returns></returns>
        private bool AttemptToSpawn()
        {
            if (!Utilities.CheckIfHaveUnoccupiedPosition(_snakeBody, _parameters.Dimension, _parameters.Size))
            {
                _hasSpawned = false;
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