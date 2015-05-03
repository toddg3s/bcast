using bcast.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.data.sqlserver
{
    public class AuthDataImpl : IAuthData
    {
        private bcastdata _data;
        public bcastdata Data { get { return _data ?? (_data = new bcastdata()); } }

        public SecureAccount getAccount(string name)
        {
            var acct = (from sa in Data.Account.OfType<SecureAccount>() where sa.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) select sa).FirstOrDefault();
            if (acct == null)
                throw new AccountNotFoundException(name);

            return acct;
        }

        public void createAccount(string name, string password)
        {
            try
            {
                getAccount(name);
                throw new AccountAlreadyExistsException(name);
            }
            catch(AccountNotFoundException)
            {
                var acct = new SecureAccount() { Name = name, Password = password };
                Data.Account.Add(acct);
                Data.SaveChanges();
            }

        }

        public void changePassword(string name, string password)
        {
            var acct = getAccount(name);
            acct.Password = password;
            Data.SaveChanges();
        }

        public void deleteAccount(string name)
        {
            var acct = getAccount(name);
            Data.Account.Remove(acct);
            Data.SaveChanges();
        }
    }
}
