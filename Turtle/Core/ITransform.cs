namespace Turtle.Core
{
    public interface ITransform
    {
        IVector2 Location { get; set; }

        public IVector2 Direction { get; set; }

        void Move();

        void Rotate();
    }
}