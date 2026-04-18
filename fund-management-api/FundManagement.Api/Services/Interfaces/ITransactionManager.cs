namespace FundManagement.Api.Services.Interfaces
{
    public interface ITransactionManager
    {
        Task ExecuteAsync(Func<Task> action);
    }
}
