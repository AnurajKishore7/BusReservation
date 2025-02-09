using BusApp.DTOs.Auth;
using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository<User, string> _userRepository;
        private readonly IClientRepo _clientRepository;
        private readonly IConfiguration _configuration;


        public UserService(IRepository<User, string> userRepository,
                           IClientRepo clientRepository,
                           ITransportOperatorRepo transportOperatorRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _configuration = configuration;
        }

        public async Task<ClientRegisterResponseDto?> RegisterClient(ClientRegisterDto clientDto)
        {
            try
            {
                if (await _userRepository.GetByKeyAsync(clientDto.Email) != null)
                    throw new InvalidOperationException("Email is already registered.");

                using var hmac = new HMACSHA512();
                var user = new User
                {
                    Email = clientDto.Email,
                    Name = clientDto.Name,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(clientDto.Password)),
                    PasswordSalt = hmac.Key,
                    Role = "Client",
                    IsApproved = true,
                    CreatedAt = DateTime.Now
                };
                Console.WriteLine($"Stored Salt: {Convert.ToBase64String(user.PasswordSalt)}");
                await _userRepository.AddAsync(user);

                var client = new Client
                {
                    Name = clientDto.Name,
                    Email = clientDto.Email,
                    DOB = clientDto.DOB,
                    Gender = clientDto.Gender,
                    Contact = clientDto.Contact,
                    IsDiabled = clientDto.IsDiabled
                };
                await _clientRepository.AddAsync(client);

                return new ClientRegisterResponseDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during client registration: {ex.Message}");
            }
        }

        public async Task<TransportOperatorRegisterResponseDto?> RegisterTransportOperator(TransportOperatorRegisterDto operatorDto)
        {
            try
            {
                if (await _userRepository.GetByKeyAsync(operatorDto.Email) != null)
                    throw new InvalidOperationException("Email is already registered.");

                using var hmac = new HMACSHA512();
                var user = new User
                {
                    Name = operatorDto.Name,
                    Email = operatorDto.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(operatorDto.Password)),
                    PasswordSalt = hmac.Key,
                    Role = "TransportOperator",
                    IsApproved = false, // Requires admin approval
                    CreatedAt = DateTime.Now
                };

                await _userRepository.AddAsync(user);

                return new TransportOperatorRegisterResponseDto
                {
                    Name = operatorDto.Name,
                    Email = operatorDto.Email,
                    IsApproved = user.IsApproved
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during transport operator registration: {ex.Message}");
            }
        }

        public async Task<LoginResponseDto?> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userRepository.GetByKeyAsync(loginDto.Email.ToLower());

                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return new LoginResponseDto { Message = "User not found" };
                }

                using var hmac = new HMACSHA512(user.PasswordSalt);
                Console.WriteLine($"Stored Salt: {Convert.ToBase64String(user.PasswordSalt)}");

                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password.Trim()));

                Console.WriteLine($"Stored Hash: {Convert.ToBase64String(user.PasswordHash)}");
                Console.WriteLine($"Computed Hash: {Convert.ToBase64String(computedHash)}");

                if (!computedHash.SequenceEqual(user.PasswordHash))
                {
                    Console.WriteLine("Password mismatch.");
                    return new LoginResponseDto { Message = "Invalid credentials" };
                }

                if (!user.IsApproved)
                {
                    Console.WriteLine("User not approved.");
                    return new LoginResponseDto { Message = "Pending Approval" };
                }

                // Generate JWT Token
                var secretKey = _configuration["Jwt:Secret"];
                if (string.IsNullOrEmpty(secretKey))
                    throw new Exception("JWT Secret Key is missing in configuration.");

                var key = Encoding.UTF8.GetBytes(secretKey);

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                string jwtToken = tokenHandler.WriteToken(token);

                Console.WriteLine($"Generated JWT Token: {jwtToken}");

                return new LoginResponseDto
                {
                    Message = "Login successful",
                    Token = jwtToken
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                throw;
            }
        }

    }
}
