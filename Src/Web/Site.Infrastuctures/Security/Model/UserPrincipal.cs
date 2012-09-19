using System;
using System.Security.Principal;
using Litium.Domain.Entities.Customers;

namespace Site.Infrastuctures.Security.Model
{
    public class UserPrincipal : IPrincipal
    {
        private readonly UserRole _role;
        private readonly IIdentity _identity;
        
        public UserPrincipal(UserRole role, IIdentity identity)
        {
            _role = role;
            _identity = identity;
        }

        public bool IsInRole(string role)
        {
            return _role.ToString().Equals(role, StringComparison.InvariantCultureIgnoreCase);
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }
    }
}
