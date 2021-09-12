namespace Turtle.GameObjects.Movable
{
    using global::Turtle.Core;

    public class Turtle : ITurtle
    {
        public ITransform Transform { get; set; }

        public Turtle(ITransform transform)
        {
            this.Transform = transform;
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