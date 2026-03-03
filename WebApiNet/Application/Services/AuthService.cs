using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiNet.Core.Entities;
using WebApiNet.Core.Interfaces;
using WebApiNet.Infrastructure.Repositories.UnitOfWork;
using WebApiNet.Shared.DTOs.Auth;

namespace WebApiNet.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;



        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _config = config;

        }

        public Task<bool> DeleteUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserResponse>> GetAllUserAync()
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> GetUserAsync()
        {
            throw new NotImplementedException();
        }

        /*    public Task<UserResponse> GetUserAsync()
            {
                var dni = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   throw new KeyNotFoundException("No se ha encontrado DNI.");


                var cliente = _context.Clientes.FirstOrDefault(c => c.Dni == dni) ??
                    throw new KeyNotFoundException("Usuario no encontrado");

                return Task.FromResult(new UserResponse
                {
                    Dni = cliente.Dni,
                    Nombre = cliente.Nombre,
                    Email = cliente.Email
                });
            }*/

        public async Task<AuthResponse> Login(LoginRequest loginDto)
        {
            var cliente = await _unitOfWork.Auth.GetByIdAsync(loginDto.Email) ??
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, cliente.PasswordHash))
            {
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");
            }

            dynamic tokenData = GenerateJwtToken(cliente);
            return new AuthResponse
            {
                Token = tokenData.Token,
                ExpirationAt = tokenData.Expiration,
                User = new UserResponse
                {
                    Dni = cliente.Dni,
                    Nombre = cliente.Nombre,
                    Email = cliente.Email
                }
            };
        }

        public async Task<UserResponse> Register(RegisterRequest registerRequest)
        {
            var clienteExistente = await _unitOfWork.Auth.GetByIdAsync(registerRequest.Dni);

            if (clienteExistente != null)
            {
                throw new InvalidOperationException("El usuario ya existe");
            }

            var cliente = _mapper.Map<Cliente>(registerRequest);
            cliente.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

            await _unitOfWork.Auth.AddAsync(cliente);

            return _mapper.Map<UserResponse>(cliente);
        }

        public Task<UserResponse> UpdateUserAsync(string id, UpdateUserRequest updateUserDto)
        {
            throw new NotImplementedException();
        }

        private object GenerateJwtToken(Cliente cliente)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, cliente.Dni),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, cliente.Dni),
                new Claim(ClaimTypes.Role, cliente.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"])),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new
            {
                Token = tokenString,
                Expiration = expiration
            };
        }
    }
}
