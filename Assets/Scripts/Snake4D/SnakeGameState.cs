using System.Collections.Generic;
using UnityEngine;

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

        public int SnakeTailLength => _snakeBody.Count - 1;

        public bool GameOver { get; private set; }

        SnakeHead _snakeHead => _snakeBody.SnakeHead;
        SnakeBody _snakeBody;

        SnakeFood _snakeFood;

        public SnakeGameState(SnakeGameParameters parameters)
        {
            _snakeBody = new SnakeBody(parameters);
            _snakeFood = new SnakeFood(parameters, _snakeBody); // Must initiate snake body before initiating snake food
        }

        public void UpdateState()
        {
            _snakeBody.UpdateSnakeBody();

            // Check if snake head has crashed into tail
            if (_snakeBody.HasBitenItself())
            {
                Debug.Log("Snake has biten itself!");
                GameOver = true;
                return;
            }

            bool hasSnakeAteFood = _snakeFood.HasSnakeAteFood(); // Must update snake body before updating snake food
            if (hasSnakeAteFood)
            {
                _snakeBody.GrowTail();

                bool hasFoodSpawned = _snakeFood.AttemptToSpawn();
                if (!hasFoodSpawned)
                {
                    Debug.Log("Snake food has nowhere to spawn!");
                    GameOver = true;
                }
            }
        }

        public void OnUserInput(UserInputType inputType)
        {
            Vector4Int newDirection = Utilities.ConvertUserInputToDirectionVector(inputType);
            _snakeHead.ChangeUserInputDirection(newDirection);
        }
    }
}