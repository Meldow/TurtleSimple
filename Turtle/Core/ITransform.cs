namespace Turtle.Core
{
    public interface ITransform
    {
        IVector2 Location { get; set; }

        void Move();

        void Rotate();
    }
}