namespace Turtle.GameManagement
{
    using System;
    using System.Collections.Generic;
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
        private GameState gameState = GameState.Running;

        private enum GameState
        {
            Running,
            FoundExit,
            HitMine,
            TurtleDroppedOut,
        }

        public GameManager(GameBoardDTO gameBoardDto)
        {
            this.gameBoard = new GameBoard(gameBoardDto.Size);
            this.turtle = new Turtle(gameBoardDto.TurtleTransform);

            try
            {
                this.gameBoard.ValidatePosition(this.turtle.Transform.Location);
            }
            catch (OutOfBoardException exception)
            {
                Console.WriteLine(
                    $"{exception.Message} | Location: [{exception.Location.X},{exception.Location.Y}] , Object: [{exception.GameObject}]");

                throw;
            }

            PopulateBoard(this.gameBoard, gameBoardDto.MinesLocations, gameBoardDto.ExitsLocations);
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

                    this.UpdateGameState();

                    if (this.gameState != GameState.Running)
                    {
                        break;
                    }
                }
            }
            catch (UnexpectedInputException exception)
            {
                Console.WriteLine($"{exception.Message} | Input: '{exception.Input}'");
            }

            OutputFinalGameState(this.gameState, this.turtle.Transform.Location);
        }

        private static void OutputFinalGameState(GameState gameState, IVector2 turtleLocation)
        {
            switch (gameState)
            {
                case GameState.Running:
                    Console.WriteLine("Turtle did not manage to escape, still in danger!");
                    break;
                case GameState.FoundExit:
                    Console.WriteLine("Turtle escaped successfully!");
                    break;
                case GameState.HitMine:
                    Console.WriteLine("Mine hit!");
                    break;
                case GameState.TurtleDroppedOut:
                    Console.WriteLine(
                        $"Turtle dropped into the void! | Location: [{turtleLocation.X},{turtleLocation.Y}]");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void PopulateBoard(
            IGameBoard gameBoard,
            IEnumerable<IVector2> minesLocations,
            IEnumerable<IVector2> exitsLocations)
        {
            foreach (var mineLocation in minesLocations)
            {
                gameBoard.AddGameObject(new Mine(), mineLocation);
            }

            foreach (var exitLocation in exitsLocations)
            {
                gameBoard.AddGameObject(new Exit(), exitLocation);
            }
        }

        private void UpdateGameState()
        {
            try
            {
                var turtleLocationGameObject = this.gameBoard.GetGameObject(this.turtle.Transform.Location);

                if (turtleLocationGameObject is Mine)
                {
                    this.gameState = GameState.HitMine;
                }
                else if (turtleLocationGameObject is Exit)
                {
                    this.gameState = GameState.FoundExit;
                }
            }
            catch (OutOfBoardException)
            {
                this.gameState = GameState.TurtleDroppedOut;
            }
        }
    }
}