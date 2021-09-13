namespace Turtle.InputManagement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Turtle.Core;
    using Turtle.Exceptions;

    public class GameSettingsFileParser : FileParser
    {
        public GameSettingsFileParser(string fileName)
            : base(fileName)
        {
        }

        public async Task<IVector2> ParseBoardSize()
        {
            var boardSizeTokens = (await this.StreamReader.ReadLineAsync())?.Split(',');
            return new Vector2(int.Parse(boardSizeTokens[0]), int.Parse(boardSizeTokens[1]));
        }

        public async Task<ITransform> ParseTurtleLocationAndDirection()
        {
            var inputTurtleTransformTokens = (await this.StreamReader.ReadLineAsync())?.Split(',');

            var location = new Vector2(
                int.Parse(inputTurtleTransformTokens[0]),
                int.Parse(inputTurtleTransformTokens[1]));
            var direction = ParseDirection(inputTurtleTransformTokens[2]);

            return new Transform(location, direction);
        }

        public async Task<(IEnumerable<IVector2> minesLocations, IEnumerable<IVector2> exitsLocations)>
            ParseStaticGameObjectsLocations()
        {
            var minesLocations = new List<IVector2>();
            var exitsLocations = new List<IVector2>();
            string readLine;
            while ((readLine = await this.StreamReader.ReadLineAsync()) != null)
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
    }
}