using BusApp.DTOs;
using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Services.Interfaces;

namespace BusApp.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<User, string> _userRepository;
        private readonly ITransportOperatorRepo _transportOperatorRepository;

        public RoleService(IRepository<User, string> userRepository, ITransportOperatorRepo transportOperatorRepository)
        {
            _userRepository = userRepository;
            _transportOperatorRepository = transportOperatorRepository;
        }
        public async Task<bool> ApproveTransportOperator(ApproveRoleDto dto)
        {
            try
            {
                var user = await _userRepository.GetByKeyAsync(dto.Email);
                if (user == null || user.Role != "TransportOperator") return false;

                user.IsApproved = dto.IsApproved;
                await _userRepository.UpdateAsync(user);

                if (dto.IsApproved)
                {
                    var transportOperator = new TransportOperator
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Contact = ""
                    };
                    await _transportOperatorRepository.AddAsync(transportOperator);

                    Console.WriteLine("Adding Transport Operator: " + transportOperator.Email); // Debugging
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while approving the Transport Operator.", ex);
            }
        }

    }
}
