namespace Turtle.GameManagement
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Turtle.Core;
    using Turtle.Exceptions;
    using Turtle.GameObjects;
    using Turtle.GameObjects.Movable;
    using Turtle.GameObjects.Static;

    public class GameManager : IGameManager
    {
        private IGameBoard gameBoard;
        private ITurtle turtle;
        private State gameState = State.Running;

        private enum State
        {
            Running,
            FoundExit,
            HitMine,
            TurtleDroppedOut,
        }

        public async Task Setup(StreamReader inputGameSettings)
        {
            this.gameBoard = await CreateGameBoard(inputGameSettings);
            this.turtle = await CreateTurtle(inputGameSettings, this.gameBoard);
            await PopulateBoard(inputGameSettings, this.gameBoard);
        }

        public async Task GameLoop(StreamReader inputMoves)
        {
            try
            {
                string readLine;
                while ((readLine = await inputMoves.ReadLineAsync()) != null)
                {
                    if (readLine == "m")
                    {
                        this.turtle.Move();
                    }
                    else if (readLine == "r")
                    {
                        this.turtle.Rotate();
                    }
                    else
                    {
                        throw new UnexpectedInputException(
                            "Unexpected move input, only 'm' and 'r' are acceptable.",
                            readLine);
                    }

                    this.UpdateGameState(this.gameBoard, this.turtle);

                    if (this.gameState != State.Running)
                    {
                        break;
                    }
                }
            }
            catch (UnexpectedInputException exception)
            {
                Console.WriteLine($"{exception.Message} | Input: '{exception.Input}'");
            }
            finally
            {
                inputMoves.Close();
            }
        }

        public void OutputFinalGameState()
        {
            switch (this.gameState)
            {
                case State.Running:
                    Console.WriteLine("Turtle did not manage to escape, still in danger!");
                    break;
                case State.FoundExit:
                    Console.WriteLine("Turtle escaped successfully!");
                    break;
                case State.HitMine:
                    Console.WriteLine("Mine hit!");
                    break;
                case State.TurtleDroppedOut:
                    Console.WriteLine(
                        $"Turtle dropped into the void! | Location: [{this.turtle.Transform.Location.X},{this.turtle.Transform.Location.Y}]");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateGameState(IGameBoard gameBoard, ITurtle turtle)
        {
            try
            {
                var turtleLocationGameObject = gameBoard.GetGameObject(turtle.Transform.Location);

                if (turtleLocationGameObject is Mine)
                {
                    this.gameState = State.HitMine;
                }
                else if (turtleLocationGameObject is Exit)
                {
                    this.gameState = State.FoundExit;
                }
            }
            catch (OutOfBoardException)
            {
                this.gameState = State.TurtleDroppedOut;
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

        private static async Task<GameBoard> CreateGameBoard(StreamReader inputGameSettings)
        {
            var boardSize = (await inputGameSettings.ReadLineAsync())?.Split(',');
            return new GameBoard(
                int.Parse(boardSize[0]),
                int.Parse(boardSize[1]));
        }

        private static async Task<Turtle> CreateTurtle(StreamReader inputGameSettings, IGameBoard gameBoard)
        {
            try
            {
                var turtleLocation = (await inputGameSettings.ReadLineAsync())?.Split(',');
                var transform = new Transform(
                    new Vector2(
                        int.Parse(turtleLocation[0]),
                        int.Parse(turtleLocation[1])));
                gameBoard.ValidatePosition(transform.Location);

                return new Turtle(transform);
            }
            catch (OutOfBoardException exception)
            {
                Console.WriteLine(
                    $"{exception.Message} | Location: [{exception.Location.X},{exception.Location.Y}] , Object: [{exception.GameObject}]");

                throw;
            }
        }

        private static async Task PopulateBoard(StreamReader inputGameSettings, IGameBoard gameBoard)
        {
            string readLine;
            while ((readLine = await inputGameSettings.ReadLineAsync()) != null)
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
                            gameBoard.AddGameObject(new Mine(), transform);
                            break;
                        case "e":
                            gameBoard.AddGameObject(new Exit(), transform);
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
        }
    }
}