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
        bool _hasSpawned = false;

        public SnakeFood(SnakeGameParameters parameters, SnakeBody snakeBody)
        {
            _parameters = parameters;
            _snakeBody = snakeBody;
            AttemptToSpawn();
        }

        public void UpdateSnakeFood()
        {
            // "Spawn" food when eaten, meaning the snake head position is over the food
            if (_snakeBody.SnakeHead.Position == _position)
            {
                AttemptToSpawn();
            }
        }

        private void AttemptToSpawn()
        {
            if (!Utilities.CheckIfHaveUnoccupiedPosition(_snakeBody, _parameters.Dimension, _parameters.Size))
            {
                _hasSpawned = false;
                return;
            }

            SetPositionToRandomUnoccupiedPosition();
            _hasSpawned = true;
        }

        private void SetPositionToRandomUnoccupiedPosition()
        {
            _position = Utilities.GenerateRandomUnoccupiedPosition(_parameters.Dimension, _parameters.Size, _snakeBody);
        }
    }
}