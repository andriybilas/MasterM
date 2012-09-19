using System;


namespace Litium.Test.Common.Xunit
{
	public abstract class TestBase : IDisposable
	{
		protected TestBase()
		{
			// Initialize the profiler
			

			// You can also use the profiler in an offline manner.
			// This will generate a file with a snapshot of all the NHibernate activity in the application,
			// which you can use for later analysis by loading the file into the profiler.
			// var filename = @"c:\profiler-log";
			// NHibernateProfiler.InitializeOfflineProfiling(filename);		

			((Action)Init)();
		}

		protected virtual void Init()
		{
		}

		public virtual void Dispose()
		{
		}
	}
}
