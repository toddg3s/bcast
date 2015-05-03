using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    public interface IAuthData
    {
        SecureAccount getAccount(string name);
        void createAccount(string name, string password);
        void changePassword(string name, string password);
        void deleteAccount(string name);
    }
}
