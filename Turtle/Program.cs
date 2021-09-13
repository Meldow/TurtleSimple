namespace Turtle
{
    using System;
    using System.Threading.Tasks;
    using Turtle.Exceptions;
    using Turtle.GameManagement;
    using Turtle.InputDTO;
    using Turtle.InputManagement;
    using Action = Turtle.InputManagement.Action;

    class Program
    {
        public static async Task Main(string[] args)
        {
            var gameManager = new GameManager(await CreateGameBoardDto(args[0]));
            ExecuteMoves(args[1], gameManager);
        }

        private static async Task<GameBoardDto> CreateGameBoardDto(string gameSettingsFileName)
        {
            using var fileParser = new GameSettingsFileParser(gameSettingsFileName);

            var boardSize = await fileParser.ParseBoardSize();
            var turtleTransform = await fileParser.ParseTurtleLocationAndDirection();
            var (minesLocations, exitsLocations) = await fileParser.ParseStaticGameObjectsLocations();

            return new GameBoardDto(boardSize, turtleTransform, minesLocations, exitsLocations);
        }

        private static void ExecuteMoves(string movesFileName, IGameManager gameManager)
        {
            using var fileParser = new ActionFileParser(movesFileName);

            var sequence = 1;

            while (fileParser.HasNext())
            {
                try
                {
                    var move = fileParser.GetNextMove();

                    switch (move)
                    {
                        case Action.Move:
                            gameManager.MoveTurtle();
                            break;
                        case Action.Rotate:
                            gameManager.RotateTurtle();
                            break;
                        case Action.ForfeitRun:
                            gameManager.ForfeitRun();
                            break;
                    }

                    if (gameManager.IsGameRunning())
                    {
                        continue;
                    }

                    Console.WriteLine($"Sequence {sequence}: {gameManager.GetEndGameMessage()}");
                    sequence += 1;
                    gameManager.ResetBoard();

                    fileParser.SkipRestLine();
                }
                catch (UnexpectedInputException exception)
                {
                    Console.WriteLine($"Parsing sequence {sequence}: {exception.Message} Ignoring character | Input: '{exception.InputChar}'");
                }
            }
        }
    }
}