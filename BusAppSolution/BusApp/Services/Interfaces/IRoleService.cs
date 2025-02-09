using BusApp.DTOs;

namespace BusApp.Services.Interfaces
{
    public interface IRoleService
    {
        Task<bool> ApproveTransportOperator(ApproveRoleDto dto);
    }
}
