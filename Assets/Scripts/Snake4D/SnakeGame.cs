using System.Collections.Generic;
using UnityEngine;

namespace Snake4D
{
    public class SnakeGame
    {
        public bool GameOver => _gameState.GameOver;

        SnakeGameParameters _parameters;
        Dimension _dimension => _parameters.Dimension;
        Vector4Int _size => _parameters.Size;

        SnakeGameState _gameState;

        public SnakeGame(SnakeGameParameters parameters)
        {
            _parameters = parameters;
            _gameState = new SnakeGameState(parameters);
        }

        public List<Vector3Int> GetPreviousSnakeBodyWorldSpacePositions()
        {
            List<Vector4Int> positions = _gameState.PreviousSnakeBodyPositions;
            return Utilities.Convert4DVectorsToWorldSpace(positions);
        }

        public List<Vector3Int> GetCurrentSnakeBodyWorldSpacePositions()
        {
            List<Vector4Int> positions = _gameState.CurrentSnakeBodyPositions;
            return Utilities.Convert4DVectorsToWorldSpace(positions);
        }

        public List<Vector3Int> GetPreviousSnakeBodyWorldSpaceDirections()
        {
            List<Vector4Int> positions = _gameState.PreviousSnakeBodyDirections;
            return Utilities.Convert4DVectorsToWorldSpace(positions);
        }

        public List<Vector3Int> GetCurrentSnakeHeadWorldSpaceDirections()
        {
            List<Vector4Int> positions = _gameState.CurrentSnakeBodyDirections;
            return Utilities.Convert4DVectorsToWorldSpace(positions);
        }

        public Vector3Int GetCurrentSnakeFoodWorldSpacePosition()
        {
            Vector3Int gameSpacePosition = _gameState.SnakeFoodPosition.ToVector3Int();
            return Utilities.GameSpaceToWorldSpace(gameSpacePosition);
        }

        public Vector3Int GetPreviousSnakeFoodWorldSpacePosition()
        {
            Vector3Int gameSpacePosition = _gameState.PreviousSnakeFoodPosition.ToVector3Int();
            return Utilities.GameSpaceToWorldSpace(gameSpacePosition);
        }


        public void UpdateState()
        {
            _gameState.UpdateState();
        }

        public void OnUserInput(UserInputType inputType)
        {
            if (inputType == UserInputType.NONE) return;
            _gameState.OnUserInput(inputType);
        }

        private void SpawnFood()
        {

        }
    }
}