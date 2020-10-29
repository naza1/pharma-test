using System.Collections.Generic;
using Pharma.Common.Model;

namespace Pharma.Api.Services
{
    public interface IAccountService
    {
        Account Authenticate(string username, string password);
        IEnumerable<Account> GetAll();
        Account GetById(int id);
        Account Create(Account account, string password);
        void Update(Account account, string password = null);
        void Delete(int id);
    }
}
