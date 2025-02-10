using BusApp.DTOs.TransOP;

namespace BusApp.Services.Interfaces.TransOp
{
    public interface ITransOpService
    {
        Task<TransOpResponseDto?> GetOperatorByIdAsync(int operatorId);
        Task<bool> UpdateOperatorAsync(int operatorId, TransOpUpdateDto updateDto);
        Task<bool> DeleteOperatorAsync(int operatorId);
    }
}
