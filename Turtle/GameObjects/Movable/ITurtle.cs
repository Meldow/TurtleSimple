namespace Turtle.GameObjects.Movable
{
    using global::Turtle.Core;

    public interface ITurtle
    {
        public ITransform Transform { get; set; }

        void Move();

        void Rotate();
    }
}