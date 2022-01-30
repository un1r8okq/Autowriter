using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace Autowriter.RazorPages.Identity
{
    public class UserStore : IUserStore<AutowriterUser>, IUserPasswordStore<AutowriterUser>
    {
        private readonly IDbConnection _conn;

        public UserStore(IDbConnection dbConnection)
        {
            _conn = dbConnection;
        }

        public async Task<IdentityResult> CreateAsync(AutowriterUser user, CancellationToken cancellationToken)
        {
            var command = $"INSERT INTO {Data.UsersTableName} " +
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
            var command = $"DELETE FROM {Data.UsersTableName} " +
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
                $"FROM {Data.UsersTableName} " +
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
                $"FROM {Data.UsersTableName} " +
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
                $"FROM {Data.UsersTableName} " +
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
            var command = $"UPDATE {Data.UsersTableName} " +
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
        }

        public async Task<AutowriterUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var query = "SELECT * " +
                $"FROM {Data.UsersTableName} " +
                "WHERE normalizedUserName = @normalizedUserName";
            var parameters = new
            {
                normalizedUserName = normalizedUserName,
            };
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
    }
}
