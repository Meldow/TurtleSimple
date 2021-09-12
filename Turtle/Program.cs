namespace Turtle
{
    using System.IO;
    using System.Threading.Tasks;
    using Turtle.GameManagement;

    class Program
    {
        public static async Task Main(string[] args)
        {
            var gameManager = new GameManager();

            var gameSettingsStreamReader = new StreamReader(args[0]);
            await gameManager.Setup(gameSettingsStreamReader);
            gameSettingsStreamReader.Close();

            var movesStreamReader = new StreamReader(args[1]);
            await gameManager.GameLoop(movesStreamReader);
            movesStreamReader.Close();

            gameManager.OutputFinalGameState();
        }
    }
}