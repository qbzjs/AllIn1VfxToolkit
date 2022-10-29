using System.Collections.Generic;

namespace Snake4D
{
    public class SnakeGameState
    {
        public List<Vector4Int> PreviousSnakeBodyPositions => _snakeBody.GetPreviousPositions();
        public List<Vector4Int> CurrentSnakeBodyPositions => _snakeBody.GetCurrentPositions();

        public List<Vector4Int> PreviousSnakeBodyDirections => _snakeBody.GetPreviousDirections();
        public List<Vector4Int> CurrentSnakeBodyDirections => _snakeBody.GetCurrentDirections();

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
            bool foodHasSpawned = _snakeFood.UpdateSnakeFood(); // Must update snake body before updating snake food

            if (foodHasSpawned)
            {
                // TODO : Add Tail
                // _snakeBody.GrowTail();
            }

            // TODO : Game Over handling when snake food cannot spawn
        }

        public void OnUserInput(UserInputType inputType)
        {
            Vector4Int newDirection = Utilities.ConvertUserInputToDirectionVector(inputType);
            _snakeHead.ChangeDirection(newDirection);
        }
    }
}