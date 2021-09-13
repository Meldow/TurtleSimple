namespace Turtle.InputManagement
{
    using System.IO;
    using Dawn;

    public abstract class FileParser : IFileParser
    {
        protected readonly StreamReader StreamReader;

        protected FileParser(string fileName)
        {
            Guard.Argument(fileName, nameof(fileName)).NotNull().NotEmpty();
            this.StreamReader = new StreamReader(fileName);
        }

        public void Dispose()
        {
            this.StreamReader?.Dispose();
        }
    }
}