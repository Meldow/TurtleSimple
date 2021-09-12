namespace Turtle.GameObjects.InputDTO
{
    using System.Collections.Generic;
    using Turtle.Core;

    public class GameBoardDTO
    {
        public readonly IVector2 Size;
        public readonly IVector2 TurtleLocation;
        public readonly IVector2 TurtleDirection;
        public readonly IEnumerable<IVector2> MinesLocations;
        public readonly IEnumerable<IVector2> ExitsLocations;

        public GameBoardDTO(IVector2 size, IVector2 turtleLocation, IVector2 turtleDirection, IEnumerable<IVector2> minesLocations, IEnumerable<IVector2> exitsLocations)
        {
            this.Size = size;
            this.TurtleLocation = turtleLocation;
            this.TurtleDirection = turtleDirection;
            this.MinesLocations = minesLocations;
            this.ExitsLocations = exitsLocations;
        }
    }
}