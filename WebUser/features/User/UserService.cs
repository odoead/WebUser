namespace WebUser.features.User
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using WebUser.Data;
    using WebUser.Domain.entities;
    using WebUser.features.User.DTO;
    using WebUser.features.User.Interfaces;
    using WebUser.shared.Logger;

    public class UserService : IUserService
    {
        private readonly DB_Context dbContext;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private User? user;

        public UserService(DB_Context context, IMapper mapper, ILoggerManager logger, IConfiguration configuration, UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.configuration = configuration;
            this.dbContext = context;
            this.userManager = userManager;
        }

        public async Task<bool> ValidateUser(string password, string username)
        {
            user = await userManager.FindByNameAsync(username);
            var result = user != null && await userManager.CheckPasswordAsync(user, password);
            if (result == false)
                logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");
            return result;
        }

        public async Task<TokenDTO> CreateToken(bool populationExp)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var refreshToken = GenerateRefreshToken();
            if (user != null)
            {
                user.RefreshToken = refreshToken;

                if (populationExp == true)
                {
                    user.RefreshTokenExpireDate = DateTime.Now.AddDays(10);
                }
                await userManager.UpdateAsync(user);
            }

            var acessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new TokenDTO { AccessToken = acessToken, RefreshToken = refreshToken };
        }

        private static SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET") ?? string.Empty);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim> { new(ClaimTypes.Name, user.UserName) };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var jwtConfig = configuration.GetSection("JWT");
            return new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtConfig["Expires"])),
                signingCredentials: signingCredentials
            );
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtConfig = configuration.GetSection("JWT");

            var TokenValidationParameter = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
                ValidateLifetime = true,
                ValidIssuer = jwtConfig["Issuer"],
                ValidAudience = jwtConfig["Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, TokenValidationParameter, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (
                jwtSecurityToken == null
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        public async Task<TokenDTO> RefreshToken(TokenDTO tokenDTO)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDTO.AccessToken);
            var user = await userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.RefreshToken != tokenDTO.RefreshToken || user.RefreshTokenExpireDate <= DateTime.Now)
            {
                throw new Exception();
            }
            this.user = user;
            return await CreateToken(false);
        }
    }
}
