namespace Turtle.InputDTO
{
    using System.Collections.Generic;
    using Dawn;
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
            this.Size = Guard.Argument(size, nameof(size)).NotNull().Value;
            this.TurtleTransform = Guard.Argument(turtleTransform, nameof(turtleTransform)).NotNull().Value;
            this.MinesLocations = Guard.Argument(minesLocations, nameof(minesLocations)).NotNull().Value;
            this.ExitsLocations = Guard.Argument(exitsLocations, nameof(exitsLocations)).NotNull().Value;
        }
    }
}