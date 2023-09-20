namespace Domain.Interfaces;

public interface IConsoleWrapper
{
    void Write(string value);
    void WriteLine(string value);
    string? ReadLine();
    ConsoleKeyInfo ReadKey();
}