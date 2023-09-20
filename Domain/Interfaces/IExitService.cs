namespace Domain.Interfaces;

public interface IExitService
{
    private const string DEFAULT_MSG = "Would you like to try again? (y/n)";
    
    Task<T> VerifyContinue<T>(Func<Task<T>> continueAction, string message = DEFAULT_MSG);
    Task<T> VerifyContinue<T>(Func<T, Task<T>> continueAction, T actionParam, string message = DEFAULT_MSG);
}