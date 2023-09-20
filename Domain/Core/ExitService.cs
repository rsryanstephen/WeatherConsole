using Domain.Interfaces;

namespace Domain.Core;

public class ExitService: IExitService
{
    private readonly IConsoleWrapper _consoleWrapper;
    
    public ExitService(IConsoleWrapper consoleWrapper)
    {
        _consoleWrapper = consoleWrapper;
    }
    
    public async Task<T> VerifyContinue<T>(Func<Task<T>> continueAction, string message)
    {
        _consoleWrapper.WriteLine(message);
        
        var key = _consoleWrapper.ReadKey();
    
        switch (key.Key)
        {
            case ConsoleKey.Y or ConsoleKey.Enter:
                return await continueAction();
            case ConsoleKey.N or ConsoleKey.Escape:
                Exit();
                return await continueAction();
            default: 
                return await VerifyContinue(continueAction, "Invalid input, please try again.");
        }
    }

    public async Task<T> VerifyContinue<T>(Func<T, Task<T>> continueAction, T actionParam, string message)
    {
        _consoleWrapper.WriteLine(message);
        
        var key = _consoleWrapper.ReadKey();

        switch (key.Key)
        {
            case ConsoleKey.Y or ConsoleKey.Enter:
                return await continueAction(actionParam);
            case ConsoleKey.N or ConsoleKey.Escape:
                Exit();
                return await continueAction(actionParam);
            default: 
                return await VerifyContinue(continueAction, actionParam, "Invalid input, please try again.");
        }
    }
    
    private void Exit()
    {
        _consoleWrapper.WriteLine("\nOk, bye!");
        Environment.Exit(0);
    }
}