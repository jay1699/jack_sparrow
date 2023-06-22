namespace DeskBook.Infrastructure.Contracts.ITokenRepository
{
    public interface ITokenRepository
    {
        Task<string> GenerateTokenAsync();
    }
}