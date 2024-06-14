using Application.Contracts.Persistence;
using Application.Contracts.Token;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ICoreDbContext _coreDbContext;

        public TokenService(IConfiguration configuration,
                            ICoreDbContext coreDbContext)
        {
            _configuration = configuration;
            _coreDbContext = coreDbContext;
        }

        public async Task<JWTTokenModel> GenerateJWTAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _coreDbContext.TableNoTracking<User>()
                                           .Include(x => x.Roles)
                                           .ThenInclude(x => x.Permissions)
                                           .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            if (user is null)
                ArgumentNullException.ThrowIfNull(user);

            var tokenClaims = new List<Claim> {
                            new (ClaimTypes.Email, user.Email),
                            new (ClaimTypes.Name, user.UserName),
                            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new (JwtRegisteredClaimNames.Sub, user.Id.ToString())};

            if (user.Roles.Count != 0)
                foreach (var role in user.Roles)
                    if (role.Permissions.Count != 0)
                        foreach (var permission in role.Permissions)
                            tokenClaims.Add(new Claim("Permissions", permission.Key));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWTSettings:Secret"]!));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenExpiry = DateTime.UtcNow.AddHours(
                    Convert.ToDouble(_configuration["JWTSettings:ExpiryHour"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTSettings:Issuer"],
                audience: _configuration["JWTSettings:Audience"],
                claims: tokenClaims,
                expires: tokenExpiry,
                signingCredentials: creds
            );

            return new JWTTokenModel
            {
                JWTToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiry = tokenExpiry
            };
        }

        public RefreshTokenModel GenerateRefreshToken()
        {
            return new RefreshTokenModel {
                RefreshToken = Guid.NewGuid().ToString(),
                Expiry = DateTime.UtcNow.AddHours(
                    Convert.ToDouble(_configuration["JWTSettings:Issuer"]))
            };
        }

        public string GenerateEmailVerificationToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
