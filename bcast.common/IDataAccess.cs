using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    public interface IDataAccess
    {
        string[] getAccountList(bool includeLocked);
        Account getAcccount(string name);
        Account getAccountByEmail(string email);
        void saveAccount(Account account);
        string[] getEndpointList(string accountName);
        Endpoint getEndpoint(string fullName);
        Endpoint getEndpoint(string accountName, string name);
        void saveEndpoint(Endpoint endpoint);
        void deleteEndpoint(string fullname);
    }
}
