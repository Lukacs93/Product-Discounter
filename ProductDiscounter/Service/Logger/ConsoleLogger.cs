namespace ProductDiscounter.Service.Logger;

public class ConsoleLogger : LoggerBase
{
    protected override void LogMessage(string message, string type)
    {
        Console.WriteLine($"[{DateTime.Now}] {type}: {message}");
    }
}