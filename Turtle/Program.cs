namespace Turtle
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Turtle.Core;
    using Turtle.Exceptions;
    using Turtle.GameManagement;
    using Turtle.GameObjects.InputDTO;
    using Turtle.InputManagement;

    class Program
    {
        public static async Task Main(string[] args)
        {
            var gameManager = new GameManager(await CreateGameBoardDto(args[0]));

            var movesStreamReader = new StreamReader(args[1]);
            ExecuteMoves(movesStreamReader, gameManager);
            movesStreamReader.Close();
        }

        private static async Task<GameBoardDTO> CreateGameBoardDto(string gameSettingsFileName)
        {
            using var fileParser = new GameSettingsFileParser(gameSettingsFileName);

            var boardSize = await fileParser.ParseBoardSize();
            var turtleTransform = await fileParser.ParseTurtleLocationAndDirection();
            var (minesLocations, exitsLocations) = await fileParser.ParseStaticGameObjectsLocations();

            return new GameBoardDTO(boardSize, turtleTransform, minesLocations, exitsLocations);
        }

        private static void ExecuteMoves(StreamReader movesStreamReader, IGameManager gameManager)
        {
            const int mInt = 'm';
            const int rInt = 'r';
            const int newLineInt = '\n';
            var sequence = 1;

            // Checks next char not new line or empty
            while (movesStreamReader.Peek() >= 0)
            {
                try
                {
                    var move = movesStreamReader.Read();

                    switch (move)
                    {
                        case mInt:
                            gameManager.MoveTurtle();
                            break;
                        case rInt:
                            gameManager.RotateTurtle();
                            break;
                        case newLineInt:
                            gameManager.ForfeitRun();
                            break;
                        default:
                            sequence += 1;
                            throw new UnexpectedInputException(
                                $"Sequence {sequence - 1} Unexpected move input. Only 'm' and 'r' are acceptable.", move);
                    }

                    if (gameManager.IsGameRunning())
                    {
                        continue;
                    }

                    Console.WriteLine($"Sequence {sequence}: {gameManager.GetEndGameMessage()}");
                    sequence += 1;
                    gameManager.ResetBoard();

                    while (move != newLineInt && movesStreamReader.Read() != newLineInt && movesStreamReader.Peek() >= 0)
                    {
                    }
                }
                catch (UnexpectedInputException exception)
                {
                    Console.WriteLine($"{exception.Message} Skipping line. | Input: '{exception.Input}'");
                }
            }
        }
    }
}