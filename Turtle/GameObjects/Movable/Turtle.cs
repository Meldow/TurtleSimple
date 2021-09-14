namespace Turtle.GameObjects.Movable
{
    using Dawn;
    using global::Turtle.Core;

    public class Turtle : ITurtle
    {
        public Turtle(ITransform transform)
        {
            this.Transform = Guard.Argument(transform, nameof(transform)).NotNull().Value;
        }

        public ITransform Transform { get; set; }

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