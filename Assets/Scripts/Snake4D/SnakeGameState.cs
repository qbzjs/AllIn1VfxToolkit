using System.Collections.Generic;

namespace Snake4D
{
    public class SnakeGameState
    {
        public Vector4Int SnakeHeadPosition => _snakeHead.Position;

        Dimension _dimension;
        Vector4Int _size;

        Vector4Int _currentInput;

        SnakeHead _snakeHead => _snakeBody.SnakeHead;
        SnakeBody _snakeBody = new SnakeBody();
        List<Vector4Int> _foodPositions = new List<Vector4Int>();

        public SnakeGameState(Dimension dimension, Vector4Int size)
        {
            _dimension = dimension;
            _size = size;

            _snakeBody.Clear();
            _foodPositions.Clear();

            SpawnSnakeHead();
        }

        public void UpdateState()
        {
            _snakeBody.UpdateSnakeBody();
        }

        private void SpawnSnakeHead()
        {
            if (_snakeBody.Count > 0) _snakeBody.Clear();

            Vector4Int randomPosition = Utilities.GenerateRandomPosition(_dimension, _size);
            Vector4Int randomDirection = Utilities.GenerateRandomDirection(_dimension);

            SnakeHead snakeHead = new SnakeHead(randomPosition, randomDirection);
            _snakeBody.Add(snakeHead);
        }
    }
}