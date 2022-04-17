using System.Globalization;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace Autowriter.RazorPages.Identity
{
    public class UserStore : IUserStore<AutowriterUser>, IUserPasswordStore<AutowriterUser>
    {
        private readonly UserDbConnection _conn;

        public UserStore(UserDbConnection dbConnection)
        {
            _conn = dbConnection;
        }

        public async Task<IdentityResult> CreateAsync(AutowriterUser user, CancellationToken cancellationToken)
        {
            if (ValidationErrors(user).Any())
            {
                return IdentityResult.Failed(ValidationErrors(user));
            }

            var command = $"INSERT INTO {UserDbConnection.UsersTableName} " +
                "(concurrencyStamp, normalizedUserName, passwordHash, securityStamp, userName) " +
                "VALUES (@concurrencyStamp, @normalizedUserName, @passwordHash, @securityStamp, @userName)";
            var parameters = new
            {
                concurrencyStamp = user.ConcurrencyStamp,
                normalizedUserName = user.NormalizedUserName,
                passwordHash = user.PasswordHash,
                securityStamp = user.SecurityStamp,
                userName = user.UserName,
            };
            var affectedRowCount = await _conn.ExecuteAsync(command, parameters);

            var expected = 1;
            if (affectedRowCount == expected)
            {
                return IdentityResult.Success;
            }

            throw new Exception($"{affectedRowCount} rows affected when creating user, expected {expected}");
        }

        public async Task<IdentityResult> DeleteAsync(AutowriterUser user, CancellationToken cancellationToken)
        {
            var command = $"DELETE FROM {UserDbConnection.UsersTableName} " +
                "WHERE id = @id OR userName = @userName";
            var parameters = new
            {
                id = user.Id,
                userName = user.UserName,
            };
            var affectedRowCount = await _conn.ExecuteAsync(command, parameters);

            var expected = 1;
            if (affectedRowCount == expected)
            {
                return IdentityResult.Success;
            }

            throw new Exception($"{affectedRowCount} rows affected when deleting user, expected {expected}");
        }

        public async Task<AutowriterUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var query = "SELECT concurrencyStamp, email, id, securityStamp, userName " +
                $"FROM {UserDbConnection.UsersTableName} " +
                "WHERE id = @id";
            var parameters = new
            {
                id = userId,
            };
            var user = await _conn.QueryFirstOrDefaultAsync<AutowriterUser>(query, parameters);

            return user;
        }

        public async Task<string> GetPasswordHashAsync(AutowriterUser user, CancellationToken cancellationToken)
        {
            var query = "SELECT passwordHash " +
                $"FROM {UserDbConnection.UsersTableName} " +
                "WHERE id = @id";
            var parameters = new
            {
                id = user.Id,
            };
            var passwordHash = await _conn.QueryFirstOrDefaultAsync<string>(query, parameters);

            return passwordHash;
        }

        public async Task<bool> HasPasswordAsync(AutowriterUser user, CancellationToken cancellationToken)
        {
            var query = "SELECT passwordHash " +
                $"FROM {UserDbConnection.UsersTableName} " +
                "WHERE id = @id";
            var parameters = new
            {
                id = user.Id,
            };
            var passwordHash = await _conn.QueryFirstOrDefaultAsync<string>(query, parameters);

            return passwordHash != null && passwordHash.Length > 0;
        }

        public Task SetPasswordHashAsync(AutowriterUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(AutowriterUser user, CancellationToken cancellationToken)
        {
            if (ValidationErrors(user).Any())
            {
                return IdentityResult.Failed(ValidationErrors(user));
            }

            var command = $"UPDATE {UserDbConnection.UsersTableName} " +
                "SET concurrencyStamp = @concurrencyStamp, " +
                "normalizedUserName = @normalizedUserName, " +
                "passwordHash = @passwordHash, " +
                "securityStamp = @securityStamp, " +
                "userName = @userName " +
                "WHERE id = @id";
            var parameters = new
            {
                concurrencyStamp = user.ConcurrencyStamp,
                normalizedUserName = user.NormalizedUserName,
                passwordHash = user.PasswordHash,
                securityStamp = user.SecurityStamp,
                userName = user.UserName,
                id = user.Id,
            };
            var affectedRowCount = await _conn.ExecuteAsync(command, parameters);

            var expected = 1;
            if (affectedRowCount == expected)
            {
                return IdentityResult.Success;
            }

            throw new Exception($"{affectedRowCount} rows affected when updating user, expected {expected}");
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<AutowriterUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var query = "SELECT * " +
                $"FROM {UserDbConnection.UsersTableName} " +
                "WHERE normalizedUserName = @normalizedUserName";
            var parameters = new { normalizedUserName };
            var user = await _conn.QueryFirstOrDefaultAsync<AutowriterUser>(query, parameters);

            return user;
        }

        public Task<string> GetNormalizedUserNameAsync(AutowriterUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(AutowriterUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(AutowriterUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(AutowriterUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(AutowriterUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        private static IdentityError[] ValidationErrors(AutowriterUser user)
        {
            var errors = new List<string>();

            if (!IsValidEmail(user.Email))
            {
                errors.Add($"'{user.Email}' is not a valid email address.");
            }

            return errors
                .Select(error => new IdentityError { Description = error })
                .ToArray();
        }

        // From https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                // Normalize the domain
                email = Regex.Replace(
                    email,
                    @"(@)(.+)$",
                    DomainMapper,
                    RegexOptions.None,
                    TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                static string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase,
                    TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
