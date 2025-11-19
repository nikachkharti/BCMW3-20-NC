using TinyBank.Repository.Models;

namespace TinyBank.Repository.Interfaces
{
    public interface IOperationRepository
    {
        Task<int> AddOperationAsync(Operation operation);
        Operation GetSingleOperation();
        List<Operation> GetOperationsOfAccount(int accountId);
    }
}
