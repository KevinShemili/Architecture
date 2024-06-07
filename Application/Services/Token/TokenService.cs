using Application.Contracts.Token;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Tuple<string, DateTime>> GenerateJWT()
        {
            /*var tokenClaims = new List<Claim> {
                new (ClaimTypes.Name, user.UserName!),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Count != 0)
            {
                var permissionsList = new List<string>();

                foreach (var roleName in userRoles)
                {
                    tokenClaims.Add(new(ClaimTypes.Role, roleName));

                    var role = await _roleManager.FindByNameAsync(roleName);

                    if (role is not null)
                    {
                        var roleClaims = await _roleManager.GetClaimsAsync(role);
                        
                        foreach (var claim in roleClaims)
                            if (claim.Type == "Permission")
                                permissionsList.Add(claim.Value);
                    }
                }

                tokenClaims.Add(new("permissions", 
                    string.Join(",", permissionsList)));
            }

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

            return new Tuple<string, DateTime>(
                new JwtSecurityTokenHandler().WriteToken(token), 
                tokenExpiry);*/

            return new Tuple<string, DateTime>(string.Empty, DateTime.Now);
        }
    }
}
