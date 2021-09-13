namespace Turtle.GameManagement
{
    using System;
    using System.Collections.Generic;
    using Turtle.Actions;
    using Turtle.Core;
    using Turtle.Exceptions;
    using Turtle.GameObjects;
    using Turtle.GameObjects.Movable;
    using Turtle.GameObjects.Static;
    using Turtle.InputDTO;

    public class GameManager : IGameManager
    {
        private IGameBoard gameBoard;
        private ITurtle turtle;
        private GameState gameState = GameState.Running;

        private ITransform turtleStartingTransform;

        private enum GameState
        {
            Running,
            FoundExit,
            HitMine,
            TurtleDroppedOut,
            ForfeitRun,
        }

        public GameManager(GameBoardDto gameBoardDto)
        {
            this.gameBoard = new GameBoard(gameBoardDto.Size);

            this.turtle = new Turtle(new Transform(
                new Vector2(
                    gameBoardDto.TurtleTransform.Location.X,
                    gameBoardDto.TurtleTransform.Location.Y),
                new Vector2(
                    gameBoardDto.TurtleTransform.Direction.X,
                    gameBoardDto.TurtleTransform.Direction.Y)));

            this.turtleStartingTransform = new Transform(
                new Vector2(
                    gameBoardDto.TurtleTransform.Location.X,
                    gameBoardDto.TurtleTransform.Location.Y),
                new Vector2(
                    gameBoardDto.TurtleTransform.Direction.X,
                    gameBoardDto.TurtleTransform.Direction.Y));

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

        public void GameLoop(ActionsDto actionsDto)
        {
            foreach (var move in actionsDto.Actions)
            {
                if (move is Move)
                {
                    this.turtle.Move();
                }
                else if (move is Rotate)
                {
                    this.turtle.Rotate();
                }

                this.UpdateGameState();

                if (this.gameState != GameState.Running)
                {
                    break;
                }
            }

            OutputFinalGameState(this.gameState, this.turtle.Transform.Location);
            this.ResetBoard();
        }

        public void MoveTurtle()
        {
            this.turtle.Move();
            this.UpdateGameState();
        }

        public void RotateTurtle()
        {
            this.turtle.Rotate();
            this.UpdateGameState();
        }

        public bool IsGameRunning() => this.gameState == GameState.Running;

        public void ForfeitRun()
        {
            this.gameState = GameState.ForfeitRun;
        }

        public string GetEndGameMessage() => this.gameState switch
        {
            GameState.ForfeitRun => "Turtle did not manage to escape, still in danger!",
            GameState.FoundExit => "Turtle escaped successfully!",
            GameState.HitMine => "Mine hit!",
            GameState.TurtleDroppedOut =>
                $"Turtle dropped into the void! | Location: [{this.turtle.Transform.Location.X},{this.turtle.Transform.Location.Y}]",
            _ => throw new ArgumentOutOfRangeException()
        };

        public void ResetBoard()
        {
            this.turtle.Transform.Location.X = this.turtleStartingTransform.Location.X;
            this.turtle.Transform.Location.Y = this.turtleStartingTransform.Location.Y;
            this.turtle.Transform.Direction.X = this.turtleStartingTransform.Direction.X;
            this.turtle.Transform.Direction.Y = this.turtleStartingTransform.Direction.Y;
            this.gameState = GameState.Running;
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