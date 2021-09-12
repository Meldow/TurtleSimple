namespace Turtle.GameObjects
{
    using System;
    using Turtle.Core;
    using Turtle.Exceptions;
    using Turtle.GameObjects.Static;

    public class GameBoard : IGameBoard
    {
        private readonly int xSize;
        private readonly int ySize;
        private IGameObject[,] tiles { get; }

        public GameBoard(int xSize, int ySize)
        {
            this.xSize = xSize;
            this.ySize = ySize;
            this.tiles = new IGameObject[xSize + 1, ySize + 1]; // Add +1 to size to take into account 0 based arrays
        }

        public void AddGameObject(IGameObject gameObject, IVector2 location)
        {
            this.ValidatePosition(location);

            try
            {
                this.tiles[location.X, location.Y] = gameObject;
            }
            catch (OutOfBoardException exception)
            {
                Console.WriteLine(
                    $"{exception.Message} Skipping this one. | Location: [{exception.Location.X},{exception.Location.Y}] , Object: [{exception.GameObject}]");
            }
        }

        public IGameObject GetGameObject(IVector2 location)
        {
            this.ValidatePosition(location);
            return this.tiles[location.X, location.Y];
        }

        public void ValidatePosition(IVector2 location)
        {
            if (location.X < 0
                || location.X > this.xSize
                || location.Y < 0
                || location.Y > this.ySize)
            {
                throw new OutOfBoardException(
                    "GameObject out of the board.",
                    location);
            }
        }
    }
}