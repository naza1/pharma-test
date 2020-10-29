using System;
using System.Collections.Generic;
using IPharmaAuth0.Helpers;
using Pharma.Common.Model;
using Pharma.Database;
using System.Linq;

namespace Pharma.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly PharmaContext _context;

        public AccountService(PharmaContext context)
        {
            _context = context;
        }

        public Account Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var account = _context.Accounts.FirstOrDefault(x => x.Username == username);

            // check if username exists
            if (account == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, account.PasswordHash, account.PasswordSalt))
                return null;

            // authentication successful
            return account;
        }

        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts;
        }

        public Account GetById(int id)
        {
            return _context.Accounts.Find(id);
        }

        public Account Create(Account account, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Accounts.Any(x => x.Username == account.Username))
                throw new AppException("Username \"" + account.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            account.PasswordHash = passwordHash;
            account.PasswordSalt = passwordSalt;

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account;
        }

        public void Update(Account accountParam, string password = null)
        {
            var account = _context.Accounts.Find(accountParam.Id);

            if (account == null)
                throw new AppException("Account not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(accountParam.Username) && accountParam.Username != account.Username)
            {
                // throw error if the new username is already taken
                if (_context.Accounts.Any(x => x.Username == accountParam.Username))
                    throw new AppException("Username " + accountParam.Username + " is already taken");

                account.Username = accountParam.Username;
            }

            // update account properties if provided
            if (!string.IsNullOrWhiteSpace(accountParam.FirstName))
                account.FirstName = accountParam.FirstName;

            if (!string.IsNullOrWhiteSpace(accountParam.LastName))
                account.LastName = accountParam.LastName;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                account.PasswordHash = passwordHash;
                account.PasswordSalt = passwordSalt;
            }

            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                _context.SaveChanges();
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", $"passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", $"passwordHash");

            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }

            return true;
        }
    }
}
