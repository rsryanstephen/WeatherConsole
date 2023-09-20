using Domain.Interfaces;

namespace Domain.ConsoleLogic;

public class ConsoleWrapper: IConsoleWrapper
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public void Write(string value)
    {
        Console.Write(value);
    }

    public void WriteLine(string value)
    {
        Console.WriteLine(value);
    }

    public ConsoleKeyInfo ReadKey()
    {
        return Console.ReadKey();
    }
}