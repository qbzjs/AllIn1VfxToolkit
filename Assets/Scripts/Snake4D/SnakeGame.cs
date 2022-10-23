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

        public Vector3Int GetPreviousSnakeHeadWorldSpacePosition()
        {
            Vector3Int gameSpacePosition = _gameState.PreviousSnakeHeadPosition.ToVector3Int();
            return Utilities.GameSpaceToWorldSpace(gameSpacePosition);
        }

        public Vector3Int GetCurrentSnakeHeadWorldSpacePosition()
        {
            Vector3Int gameSpacePosition = _gameState.SnakeHeadPosition.ToVector3Int();
            return Utilities.GameSpaceToWorldSpace(gameSpacePosition);
        }

        public Vector3Int GetPredictedSnakeHeadWorldSpacePosition()
        {
            Vector3Int predictedPosition = _gameState.PredictedSnakeHeadPosition.ToVector3Int();
            return Utilities.GameSpaceToWorldSpace(predictedPosition);
        }

        public Vector3Int GetPreviousSnakeHeadWorldSpaceDirection()
        {
            Vector3Int gameSpaceDirection = _gameState.PreviousSnakeHeadDirection.ToVector3Int();
            return Utilities.GameSpaceToWorldSpace(gameSpaceDirection);
        }

        public Vector3Int GetCurrentSnakeHeadWorldSpaceDirection()
        {
            Vector3Int gameSpaceDirection = _gameState.SnakeHeadDirection.ToVector3Int();
            return Utilities.GameSpaceToWorldSpace(gameSpaceDirection);
        }

        public void UpdateState()
        {
            _gameState.UpdateState();
        }

        public void OnUserInput(UserInputType inputType)
        {
            _gameState.OnUserInput(inputType);
        }

        private void SpawnFood()
        {

        }
    }
}