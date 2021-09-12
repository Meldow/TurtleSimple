namespace Turtle.Core
{
    public class Transform : ITransform
    {
        public static readonly IVector2 North = new Vector2(0, -1);
        public static readonly IVector2 East = new Vector2(1, 0);
        public static readonly IVector2 South = new Vector2(0, 1);
        public static readonly IVector2 West = new Vector2(-1, 0);

        public IVector2 Location { get; set; }

        public IVector2 Direction { get; set; }

        public Transform(IVector2 location)
            : this(location, North)
        {
        }

        public Transform(IVector2 location, IVector2 direction)
        {
            this.Location = location;
            this.Direction = direction;
        }

        public void Move()
        {
            this.Location = this.Location + this.Direction;
        }

        public void Rotate()
        {
            if (this.Direction == North)
            {
                this.Direction = East;
            }
            else if (this.Direction == East)
            {
                this.Direction = South;
            }
            else if (this.Direction == South)
            {
                this.Direction = West;
            }
            else if (this.Direction == West)
            {
                this.Direction = North;
            }
        }
    }
}