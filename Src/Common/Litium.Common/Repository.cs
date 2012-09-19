using Litium.Common.DataAccess;

namespace Litium.Common
{
	public static class Repository
	{
		public static GlobalRepository Data
		{
			get { return IoC.Resolve<GlobalRepository>(); }
		}
	}
}