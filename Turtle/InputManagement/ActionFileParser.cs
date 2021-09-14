namespace Turtle.InputManagement
{
    using Turtle.Exceptions;

    public class ActionFileParser : FileParser
    {
        private const int MInt = 'm';
        private const int RInt = 'r';
        private const int NewLineInt = '\n';
        private int lastReadInput;

        public ActionFileParser(string fileName)
            : base(fileName)
        {
        }

        public bool HasNext() => this.StreamReader.Peek() > 0;

        public Action GetNextMove()
        {
            var move = this.StreamReader.Read();
            this.lastReadInput = move;
            switch (move)
            {
                case MInt:
                    return Action.Move;
                case RInt:
                    return Action.Rotate;
                case NewLineInt:
                    return Action.ForfeitRun;
                default:
                    throw new UnexpectedInputException(
                        $"Unexpected move input. Only 'm' and 'r' are acceptable.", move);
            }
        }

        public void SkipRestLine()
        {
            while (this.lastReadInput != NewLineInt && this.StreamReader.Peek() >= 0)
            {
                this.lastReadInput = this.StreamReader.Read();
            }
        }
    }
}