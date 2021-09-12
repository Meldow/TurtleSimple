namespace Turtle.GameManagement
{
    using Turtle.GameObjects.InputDTO;

    public interface IGameManager
    {
        void GameLoop(ActionsDTO actionsDto);

        void MoveTurtle();

        void RotateTurtle();

        bool IsGameRunning();

        string GetEndGameMessage();
    }
}