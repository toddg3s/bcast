using bcast.common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.data.sqlserver
{
    public class DataAccessImpl : IDataAccess
    {
        private bcastdata _data;
        public bcastdata Data { get { return _data ?? (_data = new bcastdata());}}
        public string[] getAccountList(bool includeLocked)
        {
            return (from acct in Data.Account where includeLocked || !acct.Locked select acct.Name).ToArray();
        }

        public Account getAccount(string name)
        {
            return Data.Account.Find(name);
        }

        public Account getAccountByEmail(string email)
        {
            return (from acct in Data.Account where acct.Email.Equals(email.Trim(), StringComparison.InvariantCultureIgnoreCase) select acct).FirstOrDefault();
        }

        public void saveAccount(Account account)
        {
            var acct = Data.Account.Find(account.Name);
            if(acct==null)
            {
                Data.Account.Add(account);
            }
            else
            {
                Data.Account.Attach(account);
                Data.Entry(account).State = EntityState.Modified;
            }
            Data.SaveChanges();
        }

        public string[] getEndpointList(string accountName)
        {
            return (from endp in Data.Endpoint where endp.Name.StartsWith(accountName + ".", StringComparison.InvariantCultureIgnoreCase) select endp.Name).ToArray();
        }

        public Endpoint getEndpoint(string fullName)
        {
            return Data.Endpoint.Find(fullName);
        }

        public Endpoint getEndpoint(string accountName, string name)
        {
            return getEndpoint(accountName + "." + name);
        }

        public void saveEndpoint(Endpoint endpoint)
        {
            var endp = Data.Endpoint.Find(endpoint.Name);
            if(endp==null)
            {
                Data.Endpoint.Add(endpoint);
            }
            else
            {
                Data.Endpoint.Attach(endpoint);
                Data.Entry(endpoint).State = EntityState.Modified;
            }
            Data.SaveChanges();
        }

        public void deleteEndpoint(string fullName)
        {
            var endp = Data.Endpoint.Find(fullName);
            Data.Entry(endp).State = EntityState.Deleted;
            Data.SaveChanges();
        }
    }
}
