namespace Turtle.InputDTO
{
    using System.Collections.Generic;
    using Turtle.Core;

    public class GameBoardDto
    {
        public readonly IVector2 Size;
        public readonly ITransform TurtleTransform;
        public readonly IEnumerable<IVector2> MinesLocations;
        public readonly IEnumerable<IVector2> ExitsLocations;

        public GameBoardDto(
            IVector2 size,
            ITransform turtleTransform,
            IEnumerable<IVector2> minesLocations,
            IEnumerable<IVector2> exitsLocations)
        {
            this.Size = size;
            this.TurtleTransform = turtleTransform;
            this.MinesLocations = minesLocations;
            this.ExitsLocations = exitsLocations;
        }
    }
}