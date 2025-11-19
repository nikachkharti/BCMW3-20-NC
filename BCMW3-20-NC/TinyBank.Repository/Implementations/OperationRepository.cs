using TinyBank.Repository.Interfaces;
using TinyBank.Repository.Models;

namespace TinyBank.Repository.Implementations
{
    public class OperationRepository : IOperationRepository
    {
        public Task<int> AddOperationAsync(Operation operation)
        {
            throw new NotImplementedException();
        }

        public List<Operation> GetOperationsOfAccount(int accountId)
        {
            throw new NotImplementedException();
        }

        public Operation GetSingleOperation()
        {
            throw new NotImplementedException();
        }
    }
}
