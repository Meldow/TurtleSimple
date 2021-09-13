namespace Turtle.InputManagement
{
    using Turtle.Exceptions;

    public class ActionFileParser : FileParser
    {
        const int mInt = 'm';
        const int rInt = 'r';
        const int newLineInt = '\n';
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
                case mInt:
                    return Action.Move;
                case rInt:
                    return Action.Rotate;
                case newLineInt:
                    return Action.ForfeitRun;
                default:
                    throw new UnexpectedInputException(
                        $"Unexpected move input. Only 'm' and 'r' are acceptable.", move);
            }
        }

        public void SkipRestLine()
        {
            while (this.lastReadInput != newLineInt && this.StreamReader.Peek() >= 0)
            {
                this.lastReadInput = this.StreamReader.Read();
            }
        }
    }
}