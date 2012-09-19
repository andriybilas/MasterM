using System;
using System.Collections.Generic;
using System.Linq;
using Litium.Common;
using Litium.Common.Lifecycle;
using NHibernate;

namespace Litium.Infrastructure.DataAccess.Lifecycle
{
	/// <summary>
	/// 	Class using NHibernate for managing conversation with repository
	/// </summary>
	public class LazeSessionRequestConversation : IConversation
	{
		/// <summary>
		/// 	Closes session
		/// </summary>
		public void Close()
		{
			foreach (ISessionFactory sessionfactory in GetSessionFactories())
			{
				ISession session = LazySessionContext.UnBind(sessionfactory);
				if (session == null) continue;
				EndSession(session);
			}
		}

		/// <summary>
		/// 	Opens session
		/// </summary>
		public void Open()
		{
			foreach (ISessionFactory sessionFactory in GetSessionFactories())
			{
				ISessionFactory localFactory = sessionFactory;
				LazySessionContext.Bind(new Lazy<ISession>(() => BeginSession(localFactory)), localFactory);
			}
		}

		private static ISession BeginSession(ISessionFactory sessionFactory)
		{
			var session = sessionFactory.OpenSession();
			//session.BeginTransaction();
			return session;
		}

		private static void EndSession(ISession session)
		{
			//if (session.Transaction != null && session.Transaction.IsActive)
			//{
			//    session.Transaction.Commit();
			//}

			session.Dispose();
		}

		private IEnumerable<ISessionFactory> GetSessionFactories()
		{
			var sessionFactories = IoC.ResolveAll<ISessionFactory>();

			if (sessionFactories == null || sessionFactories.Count() == 0)
				throw new TypeLoadException("At least one ISessionFactory need to be registrated with IoC container.");

			return sessionFactories;
		}
	}
}