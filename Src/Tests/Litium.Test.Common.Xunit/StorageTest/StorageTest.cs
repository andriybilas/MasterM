using System;
using System.IO;
using System.Transactions;
using FluentNHibernate.Mapping;
using Litium.Common;
using Litium.Common.Entities;
using Litium.Common.Events;
using Litium.Common.Lifecycle;
using Litium.Test.Common.Xunit.Base;
using Xunit;

namespace Litium.Test.Common.Xunit.StorageTest
{
	public class StorageTest : ConversationalTestBase
	{
		[Fact]
		public void SaveClassWithStorageInfo()
		{
			Guid id;
			using (var tx = new TransactionScope())
			{
				var e = new TestStorageEntity
				        	{
				        		Name = "My file", 
								StorageStream = GetType().Assembly.GetManifestResourceStream(GetType().Namespace + ".IMGP2526.jpg")
				        	};
				Repository.Data.Save(e);
				id = e.Id;
				tx.Complete();
			}
			ConversationHelper.ReOpen();

			using (var tx = new TransactionScope())
			{
				var e = Repository.Data.Get<TestStorageEntity>(id);
				e.Name += "1";
				Repository.Data.Save(e);
				tx.Complete();
			}
			ConversationHelper.ReOpen();

			using (var tx = new TransactionScope())
			{
				var e = Repository.Data.Get<TestStorageEntity>(id);
				e.Name += "2";
				e.StorageStream = GetType().Assembly.GetManifestResourceStream(GetType().Namespace + ".IMGP2526.jpg");
				Repository.Data.Save(e);
				tx.Complete();
			}
			ConversationHelper.ReOpen();

			using (new TransactionScope())
			{
				var e = Repository.Data.Get<TestStorageEntity>(id);
				e.Name += "3";
				e.StorageStream = GetType().Assembly.GetManifestResourceStream(GetType().Namespace + ".IMGP2526.jpg");
				Repository.Data.Save(e);
			}
			ConversationHelper.ReOpen();

			using (var tx = new TransactionScope())
			{
				var e = Repository.Data.Get<TestStorageEntity>(id);
				Repository.Data.Delete(e);
				tx.Complete();
			}
			ConversationHelper.ReOpen();
		}
	}

	public class StorageFileSaver : IEventListener<TestStorageEntity, InsertEvent>,
	                                IEventListener<TestStorageEntity, UpdateEvent>,
	                                IEventListener<TestStorageEntity, DeleteEvent>
	{
		void IEventListener<TestStorageEntity, DeleteEvent>.HandleEvent(EntityEventArgs<TestStorageEntity> eventArgs)
		{
			var obj = eventArgs.Entity;
			if (obj != null)
			{
				if (!string.IsNullOrEmpty(obj.StoragePath))
				{
					// add information to delete storage-file when transaction completes
					var tx = Transaction.Current;
					if (tx != null)
					{
						tx.EnlistVolatile(new TransactionDeletion(true, obj.StoragePath), EnlistmentOptions.None);
					}
				}
			}
		}

		public void HandleEvent(EntityEventArgs<TestStorageEntity> eventArgs)
		{
			var obj = eventArgs.Entity;
			if (obj != null)
			{
				if (obj.Stream != null)
				{
					if (!string.IsNullOrEmpty(obj.StoragePath))
					{
						// add information to delete old storage-file when transaction completes
						var tx = Transaction.Current;
						if (tx != null)
						{
							tx.EnlistVolatile(new TransactionDeletion(true, obj.StoragePath), EnlistmentOptions.None);
						}
					}

					if (obj.Stream.CanRead)
					{
						var filePath = string.Format(@"c:\Temp\{0:N}", Guid.NewGuid());

						// add information to delete new storage-file if transaction is rolled back.
						var tx = Transaction.Current;
						if (tx != null)
						{
							tx.EnlistVolatile(new TransactionDeletion(false, filePath), EnlistmentOptions.None);
						}

						using (var outStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
						{
							if (obj.Stream is StorageStream)
							{
								var sStream = (StorageStream)obj.Stream;
								using (var inStream = new FileStream(sStream.StoragePath, FileMode.Open, FileAccess.Read, FileShare.Read))
								{
									inStream.CopyTo(outStream);
								}
							}
							else
							{
								obj.Stream.CopyTo(outStream);
							}
						}
						obj.StoragePath = filePath;
					}
				}
			}
		}
	}

	public class TransactionDeletion : IEnlistmentNotification
	{
		private readonly bool _onCommit;
		private readonly string _storagePath;

		public TransactionDeletion(bool onCommit, string storagePath)
		{
			_onCommit = onCommit;
			_storagePath = storagePath;
		}

		public void Commit(Enlistment enlistment)
		{
			if (_onCommit)
			{
				Delete();
			}
			enlistment.Done();
		}

		public void InDoubt(Enlistment enlistment)
		{
			enlistment.Done();
		}

		public void Prepare(PreparingEnlistment preparingEnlistment)
		{
			preparingEnlistment.Prepared();
		}

		public void Rollback(Enlistment enlistment)
		{
			if (!_onCommit)
			{
				Delete();
			}
			enlistment.Done();
		}

		private void Delete()
		{
			var fi = new FileInfo(_storagePath);
			if (fi.Exists)
			{
				fi.Delete();
			}
		}
	}

	public class TestStorageEntity : Entity
	{
		public virtual string Name { get; set; }

		protected internal virtual string StoragePath { get; set; }

		public virtual Stream StorageStream
		{
			get { return Stream ?? new StorageStream(StoragePath); }
			set { Stream = value; }
		}

		protected internal virtual Stream Stream { get; set; }

		public override object ValidationCopy()
		{
			return Clone();
		}
	}

	public sealed class TestStorageEntityMap : ClassMap<TestStorageEntity>
	{
		public TestStorageEntityMap()
		{
			Table("SimpleEntityTable");
			Id(x => x.Id)
				.Column("EntityId")
				.GeneratedBy.Guid();
			Map(x => x.Name)
				.Nullable();
			Map(x => x.StoragePath)
				.Column("Column1")
				.Nullable();
		}
	}

	public class StorageStream : FileStream
	{
		public StorageStream(string storagePath)
			: base(storagePath, FileMode.Open, FileAccess.Read, FileShare.Read)
		{
			StoragePath = storagePath;
		}

		internal string StoragePath { get; private set; }
	}
}
