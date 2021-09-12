namespace Turtle.GameManagement
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IGameManager
    {
        Task GameLoop(StreamReader inputMoves);
    }
}