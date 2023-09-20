namespace Domain.Interfaces;

public interface IFilePathService
{
    Task<string> Get();
}