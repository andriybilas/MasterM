using System;

namespace Litium.Domain.Entities.Customers
{
	[Serializable]
	[Flags]
	public enum UserRole
    {
        Anonimus = 0 << 0,
        Administer = 1 << 1, 
        Customer = 2 <<2
    }
}