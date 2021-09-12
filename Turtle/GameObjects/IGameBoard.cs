namespace Turtle.GameObjects
{
    using Turtle.Core;
    using Turtle.GameObjects.Static;

    public interface IGameBoard
    {
        void AddGameObject(IGameObject gameObject, IVector2 location);

        IGameObject GetGameObject(IVector2 location);

        void ValidatePosition(IVector2 transform);
    }
}