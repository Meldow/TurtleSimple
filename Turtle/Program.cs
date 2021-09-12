namespace Turtle
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Turtle.Core;
    using Turtle.Exceptions;
    using Turtle.GameManagement;
    using Turtle.GameObjects.InputDTO;

    class Program
    {
        public static async Task Main(string[] args)
        {
            var gameSettingsStreamReader = new StreamReader(args[0]);
            var gameBoardDto = await CreateGameBoardDto(gameSettingsStreamReader);
            gameSettingsStreamReader.Close();
            var gameManager = new GameManager(gameBoardDto);

            var movesStreamReader = new StreamReader(args[1]);
            ExecuteMoves(movesStreamReader, gameManager);
            movesStreamReader.Close();
        }

        private static async Task<GameBoardDTO> CreateGameBoardDto(StreamReader gameSettingsStreamReader)
        {
            var boardSizeTokens = (await gameSettingsStreamReader.ReadLineAsync())?.Split(',');
            var boardSize = new Vector2(int.Parse(boardSizeTokens[0]), int.Parse(boardSizeTokens[1]));

            var inputTurtleTransformTokens = (await gameSettingsStreamReader.ReadLineAsync())?.Split(',');
            var turtleLocation = new Vector2(
                int.Parse(inputTurtleTransformTokens[0]),
                int.Parse(inputTurtleTransformTokens[1]));
            var turtleDirection = ParseDirection(inputTurtleTransformTokens[2]);

            var (minesLocations, exitsLocations) = await ParseGameObjectsAsync(gameSettingsStreamReader);
            var gameBoardDto = new GameBoardDTO(boardSize, turtleLocation, turtleDirection, minesLocations, exitsLocations);

            return gameBoardDto;
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

        private static IVector2 ParseDirection(string direction) => direction switch
        {
            "North" => Transform.North,
            "East" => Transform.East,
            "South" => Transform.South,
            "West" => Transform.West,
            _ => throw new ArgumentOutOfRangeException(
                nameof(direction),
                $"Not expected direction value: {direction}. Valid directions 'North', 'East', 'South' or 'West'"),
        };

        private static async Task<(IEnumerable<IVector2> minesLocations, IEnumerable<IVector2> exitsLocations)>
            ParseGameObjectsAsync(StreamReader inputGameObjects)
        {
            var minesLocations = new List<IVector2>();
            var exitsLocations = new List<IVector2>();
            string readLine;
            while ((readLine = await inputGameObjects.ReadLineAsync()) != null)
            {
                try
                {
                    var input = readLine.Split(',');
                    var transform = new Vector2(
                        int.Parse(input[1]),
                        int.Parse(input[2]));

                    switch (input[0])
                    {
                        case "m":
                            minesLocations.Add(transform);
                            break;
                        case "e":
                            exitsLocations.Add(transform);
                            break;
                        default:
                            throw new UnexpectedInputException(
                                "Unexpected object input, only 'm' and 'e' are acceptable.",
                                readLine);
                    }
                }
                catch (UnexpectedInputException exception)
                {
                    Console.WriteLine($"{exception.Message} Skipping this one. | Input: [{exception.Input}]");
                }
            }

            return (minesLocations, exitsLocations);
        }
    }
}