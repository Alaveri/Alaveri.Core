namespace Alaveri.Core;

public class UnsupportedFileVersionException : Exception
{
    public byte MajorVersion { get; set; }
    public byte MinorVersion { get; set; }

    public UnsupportedFileVersionException()
    {
    }

    public UnsupportedFileVersionException(string message) : base(message)
    {
    }

    public UnsupportedFileVersionException(string message, byte majorVersion, byte minorVersion) : base(message)
    {
        MajorVersion = majorVersion;
        MinorVersion = minorVersion;
    }
}
