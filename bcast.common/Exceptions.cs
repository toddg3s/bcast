using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    public class AccountNotFoundException : Exception
    {
        public string Name { get; private set; }
        public AccountNotFoundException() : base("Account was not found") { }
        public AccountNotFoundException(string name) : base("Account was not found") { Name = name; }
        public AccountNotFoundException(string name, Exception InnerException) : base("Account was not found", InnerException) { Name = name; }
    }

    public class AccountAlreadyExistsException : Exception
    {
        public string Name { get; private set; }
        public AccountAlreadyExistsException() : base("Account with that name already exists") { }
        public AccountAlreadyExistsException(string name) : base("Account with that name already exists") { Name = name; }
        public AccountAlreadyExistsException(string name, Exception InnerException) : base("Account with that name already exists", InnerException) { Name = name; }
    }
}
