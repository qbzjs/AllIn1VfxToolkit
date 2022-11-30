using System.Collections.Generic;
using UnityEngine;

namespace Snake4D
{
    public class SnakeGame
    {
        public Vector4Int Size => _size;
        public Dimension GameDimension => _dimension;
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

        public List<Vector3Int> GetPreviousSnakeBodyWorldSpacePositions(RenderPlane renderPlane)
        {
            List<Vector4Int> positions = _gameState.PreviousSnakeBodyPositions;
            return Utilities.Convert4DVectorsToWorldSpace(positions, renderPlane);
        }

        public List<Vector3Int> GetCurrentSnakeBodyWorldSpacePositions(RenderPlane renderPlane)
        {
            List<Vector4Int> positions = _gameState.CurrentSnakeBodyPositions;
            return Utilities.Convert4DVectorsToWorldSpace(positions, renderPlane);
        }

        public List<Vector3Int> GetPreviousSnakeBodyWorldSpaceDirections(RenderPlane renderPlane)
        {
            List<Vector4Int> positions = _gameState.PreviousSnakeBodyDirections;
            return Utilities.Convert4DVectorsToWorldSpace(positions, renderPlane);
        }

        public List<Vector3Int> GetCurrentSnakeBodyWorldSpaceDirections(RenderPlane renderPlane)
        {
            List<Vector4Int> positions = _gameState.CurrentSnakeBodyDirections;
            return Utilities.Convert4DVectorsToWorldSpace(positions, renderPlane);
        }

        public Vector3Int GetCurrentSnakeFoodWorldSpacePosition(RenderPlane renderPlane)
        {
            Vector4Int gameSpacePosition = _gameState.SnakeFoodPosition;
            return Utilities.GameSpaceToWorldSpace(gameSpacePosition, renderPlane);
        }

        public Vector3Int GetPreviousSnakeFoodWorldSpacePosition(RenderPlane renderPlane)
        {
            Vector4Int gameSpacePosition = _gameState.PreviousSnakeFoodPosition;
            return Utilities.GameSpaceToWorldSpace(gameSpacePosition, renderPlane);
        }

        public int GetSnakeTailLength()
        {
            return _gameState.SnakeTailLength;
        }

        public string DebugState()
        {
            string output = "";

            output += "Snake Positions: ";
            foreach (Vector4Int snakePosition in _gameState.CurrentSnakeBodyPositions)
            {
                output += snakePosition;
            }

            output += $" Snake Food Position: {_gameState.SnakeFoodPosition}";

            return output;
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