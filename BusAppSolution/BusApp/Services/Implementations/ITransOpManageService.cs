using BusApp.DTOs.BusManagement;

namespace BusApp.Services.Implementations
{
    public interface ITransOpManageService
    {
        Task<bool> AddBusAsync(BusDto busDto);
        Task<IEnumerable<BusResponseDto>> GetBusesByOperatorIdAsync(int operatorId);
        Task<BusResponseDto?> GetBusByIdAsync(int busId);
        Task<bool> UpdateBusAsync(int busId, BusDto busDto);
        Task<bool> DeleteBusAsync(int busId);
    }

}
