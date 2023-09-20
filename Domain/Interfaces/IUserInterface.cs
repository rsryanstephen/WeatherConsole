namespace Domain.Interfaces;

public interface IUserInterface
{
    Task<bool> RequestUserInput();
}