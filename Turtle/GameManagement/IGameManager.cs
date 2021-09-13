namespace Turtle.GameManagement
{
    using Turtle.InputDTO;

    public interface IGameManager
    {
        void GameLoop(ActionsDto actionsDto);

        void MoveTurtle();

        void RotateTurtle();

        bool IsGameRunning();

        void ForfeitRun();

        string GetEndGameMessage();

        void ResetBoard();
    }
}