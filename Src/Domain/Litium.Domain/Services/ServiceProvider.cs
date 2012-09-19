using Litium.Common;

namespace Litium.Domain.Services
{
    /// <summary>
    /// The Litium studio service provider. Provide module service interfaces.
    /// </summary>
    public static class ServiceProvider
    {
       /// <summary>
        /// Return service interface.
        /// </summary>
        /// <typeparam name="T">Type of service interface.</typeparam>
        /// <returns>Interface.</returns>
        public static T GetService<T>()
        {
            return IoC.Resolve<T>();
        }
    }
}