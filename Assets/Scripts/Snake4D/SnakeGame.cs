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
            _gameState = new SnakeGameState(parameters);
        }

        public Vector3Int GetSnakeHeadWorldSpacePosition()
        {
            Vector3Int gameSpacePosition = _gameState.SnakeHeadPosition.ToVector3Int();
            return Utilities.GameSpaceToWorldSpace(gameSpacePosition);
        }

        public Vector3Int GetPredictedSnakeHeadWorldSpacePosition()
        {
            Vector3Int predictedPosition = _gameState.PredictedSnakeHeadPosition.ToVector3Int();
            return Utilities.GameSpaceToWorldSpace(predictedPosition);
        }

        public void UpdateState()
        {
            _gameState.UpdateState();
        }

        private void SpawnFood()
        {

        }
    }
}