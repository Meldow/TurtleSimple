namespace Turtle.Core
{
    public interface IVector2
    {
        public int X { get; set; }

        public int Y { get; set; }

        // Implementation in interface... hmm
        public static IVector2 operator +(IVector2 vector2A, IVector2 vector2B)
        {
            return new Vector2(
                vector2A.X + vector2B.X,
                vector2A.Y + vector2B.Y);
        }
    }
}