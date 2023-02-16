using BugTrackerManagement.DAL;
using BugTrackerManagement.Models;
using BugTrackerManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using BugTrackerManagement.Exceptions;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BugTrackerManagement.Services
{
    public class AuthorizationServices:IAuthorizationServices
    {
        private readonly BugTrackerCatalogContext _context;
        private readonly AppSettings _settings;
        public AuthorizationServices(BugTrackerCatalogContext context,IOptions<AppSettings> options)
        {
            _context = context;
            _settings = options.Value;
        }
        public async Task Register(UserCreateViewModel user)
        {
            if (await UserExistsAsync(user.Username))
            {
                throw new UserRegistrationFailed();
            }

            if (!await RolesExistAsync(user.Roles))
            {
                throw new UserRegistrationFailed();
            }

            CreatePasswordHashAndSalt(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userEntity = new User
            {
                Username = user.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FullName = user.FullName
            };

            foreach (var role in user.Roles)
            {
                userEntity.UserRoles.Add(new UserRole
                {
                    User = userEntity,
                    Role = await GetRoleAsync(role)
                });
            }

            await _context.AddAsync(userEntity);

            await _context.SaveChangesAsync();
        }
        public async Task<JwtViewModel> LoginAsync(LoginViewModel loginViewModel)
        {
            var userDb = await GetUserAsync(loginViewModel.Username);

            if (!VerifyPasswordHash(loginViewModel.Password, userDb.PasswordHash, userDb.PasswordSalt))
            {
                throw new LoginFailException();
            }
            return GetToken(userDb);
        }
        private async Task<User> GetUserAsync(string name)
        {
            var userDb = await _context
                .Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == name.ToLower());

            if (userDb == null)
            {
                throw new LoginFailException();
            }

            return userDb;
        }
        private void CreatePasswordHashAndSalt(string initialpassword, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(initialpassword));
        }
        private async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }
        private JwtViewModel GetToken(User userDb)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, userDb.Username),
                new Claim("FullName", userDb.FullName),
            };

            foreach (var role in GetRoles(userDb))
            {
                claims.Add(new Claim("Roles", role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtSecret));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtViewModel { Jwt = tokenHandler.WriteToken(securityToken) };
        }
        private IEnumerable<string> GetRoles(User user)
        {
            return user.UserRoles.Select(ur => ur.Role.Name).ToList();
        }

        private bool VerifyPasswordHash(string initialpassword, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(initialpassword));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i])
                {
                    return false;
                }
            }

            return true;
        }
        private async Task<bool> RolesExistAsync(IEnumerable<string> role)
        {
            foreach(var roles in role)
            {
                if(!await _context.Roles.AnyAsync(r => r.Name.ToLower() == roles.ToLower()))
                    return false;
            }
            return true;
        }
        private async Task<Role> GetRoleAsync(string role)
        {
            if(await _context.Roles.AnyAsync(r => r.Name.ToLower() == role.ToLower()))
            {
                return await _context.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == role.ToLower());
            }
            else
            {
                throw new UserRegistrationFailed();
            }
        }
    }
}
