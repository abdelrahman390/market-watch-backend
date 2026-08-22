using Konscious.Security.Cryptography;
using market_watch.Data;
using market_watch.Models;
using market_watch.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace market_watch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MarketWatchTrainingContext _dbContext;
        private readonly JwtService _jwtService;
        private readonly AuditLogsService _AuditLogsService;
        List<string> userRolls = new List<string> { "Admin", "User", "Analyst"};

        public RegisterController(IConfiguration configuration, MarketWatchTrainingContext dbContext, JwtService jwtService, AuditLogsService auditLogsService)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _jwtService = jwtService;
            _AuditLogsService = auditLogsService;
        }

        public record PasswordHashResult(
            byte[] Hash,
            byte[] Salt
        );

        public static async Task<byte[]> HashPasswordWithSalt(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            var argon2 = new Argon2id(passwordBytes)
            {
                Salt = salt,
                DegreeOfParallelism = 4,
                Iterations = 3,
                MemorySize = 65536
            };

            return await argon2.GetBytesAsync(32);
        }

        public static async Task<PasswordHashResult> HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            var argon2 = new Argon2id(passwordBytes)
            {
                Salt = salt,
                DegreeOfParallelism = 4,
                Iterations = 3,
                MemorySize = 65536 // 64 MB
            };

            byte[] hash = await argon2.GetBytesAsync(32);

            return new PasswordHashResult(Hash: hash, Salt: salt );
        }

        [HttpGet("register")]
        public async Task<IActionResult> register(string UserName, string FullName, string email, string Password, string userRole, bool isActive)
        {

            try
            {
                var userIpAdress = HttpContext.Connection.RemoteIpAddress?.ToString();

                bool isValidRole = userRolls.Contains(userRole);
                bool isUniqueUsername = !_dbContext.Users.Any(u => u.UserName == UserName);
                bool CorrectEmailFormat = email.Contains("@") && email.Contains(".");

                //bool isValidPassword = Password.Length >= 8;
                bool isValidPassword = true;

                Console.WriteLine($" {isValidRole} - {isUniqueUsername} - {CorrectEmailFormat} - {isValidPassword}");

                if (!isValidRole && isUniqueUsername && CorrectEmailFormat && isValidPassword)
                {
                    return BadRequest($"The entered data is incorrect. {isValidRole} - {isUniqueUsername} - {CorrectEmailFormat} - {isValidPassword}");
                }

                PasswordHashResult passwordHashResult = await HashPassword(Password);

                User newUser = new User
                {
                    UserName = UserName,
                    FullName = FullName,
                    Email = email,
                    PasswordHash = passwordHashResult.Hash,
                    Salt = passwordHashResult.Salt,
                    IsActive = true,
                    UserRole = userRole
                };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                _AuditLogsService.Log(newUser.UserId, "register", "Users", DateTime.Now, userIpAdress);

                return Ok("Registered successfuly");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(
                    500,
                    $"Error: {ex.Message}"
                );
            }
        }

        [HttpGet("login")]
        public async Task<IActionResult> login(string UserName, string Password)
        {

            try
            {
                var userIpAdress = HttpContext.Connection.RemoteIpAddress?.ToString();
                User? user = _dbContext.Users.FirstOrDefault(u => u.UserName == UserName);

                Console.WriteLine($"Error Top: {user}");
                if (user == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                byte[] passwordHashResult = await HashPasswordWithSalt(Password, user.Salt);

                if (!CryptographicOperations.FixedTimeEquals(user.PasswordHash, passwordHashResult))
                {
                    return Unauthorized("Invalid username or password.");
                }

                Console.WriteLine($"Error Top: {passwordHashResult}");

                var token = _jwtService.GenerateToken(
                        user
                    );


                _AuditLogsService.Log(user.UserId, "login", "Users", DateTime.Now, userIpAdress);

                return Ok($"Logged in successfuly. Token: {token}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(
                    500,
                    $"Error: {ex.Message}"
                );
            }
        }

    }
}
