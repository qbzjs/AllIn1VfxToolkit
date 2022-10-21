using UnityEngine;

namespace Snake4D
{
    public class SnakeGame
    {
        public Dimension Dimension => _dimension;
        public Vector4Int Size => _size;

        SnakeGameParameters _parameters;
        Dimension _dimension => _parameters.Dimension;
        Vector4Int _size => _parameters.Size;

        SnakeGameState _gameState;

        public SnakeGame(SnakeGameParameters parameters)
        {
            _parameters = parameters;
            _gameState = new SnakeGameState(_dimension, _size);

            //! testing
            Vector2 position = new Vector2(2, 0);
            // position.magni
        }

        public void UpdateState()
        {

        }

        private void SpawnSnakeHead()
        {
            Vector4Int snakeHeadPosition = Utilities.GenerateRandomPosition(_dimension, _size);
        }

        private void SpawnFood()
        {
            Vector4Int foodPosition = Utilities.GenerateRandomPosition(_dimension, _size);
        }
    }
}