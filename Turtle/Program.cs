namespace Turtle
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Turtle.Core;
    using Turtle.Exceptions;
    using Turtle.GameManagement;
    using Turtle.GameObjects;

    class Program
    {
        public static async Task Main(string[] args)
        {
            var gameBoardDto = await CreateGameBoardDto(args);
            var gameManager = new GameManager(gameBoardDto);

            var movesStreamReader = new StreamReader(args[1]);
            await gameManager.GameLoop(movesStreamReader);
            movesStreamReader.Close();
        }

        private static async Task<GameBoardDTO> CreateGameBoardDto(string[] args)
        {
            var gameSettingsStreamReader = new StreamReader(args[0]);

            var boardSizeTokens = (await gameSettingsStreamReader.ReadLineAsync())?.Split(',');
            var boardSize = new Vector2(int.Parse(boardSizeTokens[0]), int.Parse(boardSizeTokens[1]));

            var inputTurtleTransformTokens = (await gameSettingsStreamReader.ReadLineAsync())?.Split(',');
            var turtleTransform = new Transform(
                new Vector2(
                    int.Parse(inputTurtleTransformTokens[0]),
                    int.Parse(inputTurtleTransformTokens[1])),
                ParseDirection(inputTurtleTransformTokens[2]));

            var (minesLocations, exitsLocations) = await ParseGameObjectsAsync(gameSettingsStreamReader);

            var gameBoardDto = new GameBoardDTO(boardSize, turtleTransform, minesLocations, exitsLocations);

            gameSettingsStreamReader.Close();
            return gameBoardDto;
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