namespace Turtle.GameObjects.Movable
{
    using Dawn;
    using global::Turtle.Core;

    public class Turtle : ITurtle
    {
        public ITransform Transform { get; set; }

        public Turtle(ITransform transform)
        {
            this.Transform = Guard.Argument(transform, nameof(transform)).NotNull().Value;
        }

        public void Move()
        {
            this.Transform.Move();
        }

        public void Rotate()
        {
            this.Transform.Rotate();
        }
    }
}