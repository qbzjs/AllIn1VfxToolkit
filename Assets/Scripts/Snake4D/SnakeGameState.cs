using System.Collections.Generic;

namespace Snake4D
{
    public class SnakeGameState
    {
        public Vector4Int SnakeHeadPosition => _snakeHead.Position;
        public Vector4Int PredictedSnakeHeadPosition => _snakeHead.GetPredictedPosition();

        SnakeGameParameters _parameters;
        Dimension _dimension => _parameters.Dimension;
        Vector4Int _size => _parameters.Size;
        bool _passThroughWalls => _parameters.PassThroughWalls;

        Vector4Int _currentInput;

        SnakeHead _snakeHead => _snakeBody.SnakeHead;
        SnakeBody _snakeBody;
        List<Vector4Int> _foodPositions = new List<Vector4Int>();

        public SnakeGameState(SnakeGameParameters parameters)
        {
            _parameters = parameters;

            _snakeBody = new SnakeBody(parameters);
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