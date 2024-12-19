using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BillCollector.Core;
using BillCollector.Core.Entities;
using BillCollector.Infrastructure.DbContexts;

namespace BillCollector.Infrastructure.Auth
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }

    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }

    public interface IJwtService
    {
        Task<TokenResponse> GenerateTokenAsync(User user);
        Task<TokenResponse> RefreshTokenAsync(string refreshToken);
        ClaimsPrincipal ValidateToken(string token);
    }

    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly BillCollectorDbContext _context;
        //private readonly UserManager<User> _userManager;

        public JwtService(
            IOptions<JwtSettings> jwtSettings,
            BillCollectorDbContext context
            /*UserManager<User> userManager*/)
        {
            _jwtSettings = jwtSettings.Value;
            _context = context;
            //_userManager = userManager;
        }

        public async Task<TokenResponse> GenerateTokenAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim())
            };

            // Add roles
            var userRoles = await _context.UserRoles
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == user.Id)
                .ToListAsync();

            foreach (var role in userRoles.Select(ur => ur.Role.Name))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Add permissions
            var roleIds = userRoles.Select(r => r.RoleId);
            var permissions = await _context.Permissions
                .Where(p => roleIds.Contains(p.RoleId))
                .ToListAsync();

            foreach (var permission in permissions)
            {
                claims.Add(new Claim(BillCollectorConstants.PERMISSION_CLAIM_TYPE, permission.PermissionName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            var refreshToken = await GenerateRefreshTokenAsync(user.Id);

            return new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                ExpiresAt = expires,
            };
        }

        private async Task<string> GenerateRefreshTokenAsync(long userId)
        {
            //var randomNumber = new byte[32];
            //using var rng = RandomNumberGenerator.Create();
            //rng.GetBytes(randomNumber);
            //var refreshToken = Convert.ToBase64String(randomNumber);

            //// Save refresh token
            //var userSession = new UserSession
            //{
            //    UserId = userId,
            //    Token = refreshToken,
            //    ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            //    CreatedAt = DateTime.UtcNow,
            //    LastActivityAt = DateTime.UtcNow
            //};

            //await _context.UserSessions.AddAsync(userSession);
            //await _context.SaveChangesAsync();

            return "";
        }

        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            //var session = await _context.UserSessions
            //    .Include(us => us.User)
            //    .FirstOrDefaultAsync(us => us.Token == refreshToken && us.ExpiresAt > DateTime.UtcNow);

            //if (session == null)
            //    throw new SecurityTokenException("Invalid refresh token");

            //var user = session.User;
            //_context.UserSessions.Remove(session);
            //await _context.SaveChangesAsync();

            //return await GenerateTokenAsync(user);
            return new TokenResponse();
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return principal;
            }
            catch
            {
                throw new SecurityTokenException("Invalid token");
            }
        }
    }
}
