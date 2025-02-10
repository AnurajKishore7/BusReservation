using BusApp.Models;

namespace BusApp.Repositories.Interfaces.TransOp
{
    public interface ITransOpRepo
    {
        Task<TransportOperator?> GetOperatorByIdAsync(int operatorId);
        Task<bool> UpdateOperatorAsync(TransportOperator operatorEntity);
        Task<bool> DeleteOperatorAsync(int operatorId);
    }
}
