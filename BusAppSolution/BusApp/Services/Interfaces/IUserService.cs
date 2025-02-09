using BusApp.DTOs.Auth;

namespace BusApp.Services.Interfaces
{
    public interface IUserService
    {
       
        Task<ClientRegisterResponseDto?> RegisterClient(ClientRegisterDto clientDto);
        Task<TransportOperatorRegisterResponseDto?> RegisterTransportOperator(TransportOperatorRegisterDto operatorDto);
        Task<LoginResponseDto?> Login(LoginDto loginDto);
        
    }
}
