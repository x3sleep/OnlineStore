namespace Lab2.Interfaces.Logging;

public class FileLogger(string path) : ILogger
{
    public string Path { get; } = path;

    public void Log(string message) => File.AppendAllText(Path, $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
}
