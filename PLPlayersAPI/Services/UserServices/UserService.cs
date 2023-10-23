using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using PLPlayersAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace PLPlayersAPI.Services.UserServices
{
    public class UserService : IUserService
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<string?> LoginUser(UserDTO userDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.Equals(userDTO.Username));

            if (user != null && BCrypt.Net.BCrypt.Verify(userDTO.Password, user.PasswordHash))
                return CreateToken(user);

            return null;
        }

        public string CreateToken(User user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public async Task<UserDTO> RegisterUser(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);

             _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return userDTO;
        }
    }
}
