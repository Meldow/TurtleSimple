namespace Turtle.GameObjects
{
    using System;
    using Dawn;
    using Turtle.Core;
    using Turtle.Exceptions;
    using Turtle.GameObjects.Static;

    public class GameBoard : IGameBoard
    {
        private readonly int xSize;
        private readonly int ySize;

        public GameBoard(IVector2 size)
        {
            Guard.Argument(size, nameof(size)).NotNull();

            this.xSize = size.X;
            this.ySize = size.Y;
            this.Tiles = new IGameObject[size.X + 1, size.Y + 1]; // Add +1 to size to take into account 0 based arrays
        }

        private IGameObject[,] Tiles { get; }

        public void AddGameObject(IGameObject gameObject, IVector2 location)
        {
            Guard.Argument(gameObject, nameof(gameObject)).NotNull();
            Guard.Argument(location, nameof(location)).NotNull();

            try
            {
                this.ValidatePosition(location);
                this.Tiles[location.X, location.Y] = gameObject;
            }
            catch (OutOfBoardException exception)
            {
                Console.WriteLine(
                    $"{exception.Message} Skipping this one. | Location: [{exception.Location.X},{exception.Location.Y}] , Object: [{gameObject}]");
            }
        }

        public IGameObject GetGameObject(IVector2 location)
        {
            Guard.Argument(location, nameof(location)).NotNull();

            this.ValidatePosition(location);
            return this.Tiles[location.X, location.Y];
        }

        public void ValidatePosition(IVector2 location)
        {
            Guard.Argument(location, nameof(location)).NotNull();

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