namespace Turtle.Core
{
    using Dawn;

    public class Transform : ITransform
    {
        public static IVector2 North = new Vector2(0, -1);
        public static IVector2 East = new Vector2(1, 0);
        public static IVector2 South = new Vector2(0, 1);
        public static IVector2 West = new Vector2(-1, 0);

        public IVector2 Location { get; set; }

        public IVector2 Direction { get; set; }

        public Transform(IVector2 location)
            : this(location, North)
        {
        }

        public Transform(IVector2 location, IVector2 direction)
        {
            this.Location = Guard.Argument(location, nameof(location)).NotNull().Value;
            this.Direction = Guard.Argument(direction, nameof(direction)).NotNull().Value;
        }

        public void Move()
        {
            this.Location = this.Location + this.Direction;
        }

        public void Rotate()
        {
            if (this.Direction.X == North.X && this.Direction.Y == North.Y)
            {
                this.Direction.X = East.X;
                this.Direction.Y = East.Y;
            }
            else if (this.Direction.X == East.X && this.Direction.Y == East.Y)
            {
                this.Direction.X = South.X;
                this.Direction.Y = South.Y;
            }
            else if (this.Direction.X == South.X && this.Direction.Y == South.Y)
            {
                this.Direction.X = West.X;
                this.Direction.Y = West.Y;
            }
            else if (this.Direction.X == West.X && this.Direction.Y == West.Y)
            {
                this.Direction.X = North.X;
                this.Direction.Y = North.Y;
            }
        }
    }
}