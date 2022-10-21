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
        }

        public Vector3Int GetSnakeHeadWorldSpacePosition()
        {
            Vector3Int gameSpacePosition = _gameState.SnakeHeadPosition.ToVector3Int();
            return Utilities.GameSpaceToWorldSpace(gameSpacePosition);
        }

        public void UpdateState()
        {

        }

        private void SpawnFood()
        {

        }
    }
}