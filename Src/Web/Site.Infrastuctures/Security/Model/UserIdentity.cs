using System.Security.Principal;

namespace Site.Infrastuctures.Security.Model
{
    public class UserIdentity : IIdentity
    {
        private readonly bool _isAuthenticated;
        private readonly string _name;

        public UserIdentity(string name, bool authenticated)
        {
            _name = name;
            _isAuthenticated = authenticated;
        }

        public string Name
        {
            get { return _name; }
        }

        public string AuthenticationType { get { return "InForm"; } }
        
        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
        }
    }
}
