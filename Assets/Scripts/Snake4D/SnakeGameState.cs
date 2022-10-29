using System.Collections.Generic;

namespace Snake4D
{
    public class SnakeGameState
    {
        public Vector4Int PreviousSnakeHeadPosition => _snakeHead.GetPreviousPosition();
        public Vector4Int SnakeHeadPosition => _snakeHead.Position;
        public Vector4Int PredictedSnakeHeadPosition => _snakeHead.GetPredictedPosition();

        public Vector4Int PreviousSnakeHeadDirection => _snakeHead.GetPreviousDirection();
        public Vector4Int SnakeHeadDirection => _snakeHead.Direction;

        public Vector4Int PreviousSnakeFoodPosition => _snakeFood.GetPreviousPosition();
        public Vector4Int SnakeFoodPosition => _snakeFood.Position;

        public bool GameOver { get; private set; }

        SnakeGameParameters _parameters;
        Dimension _dimension => _parameters.Dimension;
        Vector4Int _size => _parameters.Size;
        bool _passThroughWalls => _parameters.PassThroughWalls;

        Vector4Int _currentInput;

        SnakeHead _snakeHead => _snakeBody.SnakeHead;
        SnakeBody _snakeBody;

        SnakeFood _snakeFood;

        public SnakeGameState(SnakeGameParameters parameters)
        {
            _parameters = parameters;
            _snakeBody = new SnakeBody(parameters);
            _snakeFood = new SnakeFood(parameters, _snakeBody); // Must initiate snake body before initiating snake food
        }

        public void UpdateState()
        {
            _snakeBody.UpdateSnakeBody();
            _snakeFood.UpdateSnakeFood(); // Must update snake body before updating snake food

            if (!_snakeFood.HasSpawned)
            {
                GameOver = true;
            }
        }

        public void OnUserInput(UserInputType inputType)
        {
            Vector4Int newDirection = Utilities.ConvertUserInputToDirectionVector(inputType);
            _snakeHead.ChangeDirection(newDirection);
        }
    }
}