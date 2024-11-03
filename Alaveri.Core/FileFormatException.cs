namespace Alaveri.Core;

public class FileFormatException : Exception
{
    public FileFormatException()
    {
    }

    public FileFormatException(string message) : base(message)
    {
    }
}
