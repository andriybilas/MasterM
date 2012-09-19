using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Litium.Common;
using Litium.Common.Events;
using Litium.Common.WorkUnit;
using Litium.Domain.Utilities;
using File = Litium.Domain.Entities.Media.File;

namespace Litium.Domain.EventListeners.Media
{
	internal sealed class FileStorageHandler : IEventListener<File, InsertEvent>,
	                                           IEventListener<File, UpdateEvent>,
	                                           IEventListener<File, DeleteEvent>,
	                                           IEventListener<File, LoadedEvent>
	{
		void IEventListener<File, InsertEvent>.HandleEvent(EntityEventArgs<File> eventArgs)
		{
		    ClearIfExists(eventArgs.Entity);
			Store(eventArgs.Entity, false);
		}

	    private void ClearIfExists(File entity)
	    {
	        IEnumerable<File> files = Repository.Data.Get<File>().Where(x => x.EntityId == entity.EntityId).All();
	        foreach (var file in files)
	        {
                  Repository.Data.Delete(file);
	        }
	    }

	    private bool HasAnotherEntity(File file)
	    {
	        IEnumerable<File> files = Repository.Data.Get<File>().Where(x => x.Name.Equals(file.Name, StringComparison.InvariantCultureIgnoreCase) && x.Id != file.Id).All();
            if (files == null)
                return false;

	        return files.Any();
	    }

	    void IEventListener<File, UpdateEvent>.HandleEvent(EntityEventArgs<File> eventArgs)
		{
			Store(eventArgs.Entity, true);
		}

		void IEventListener<File, DeleteEvent>.HandleEvent(EntityEventArgs<File> eventArgs)
		{
			Delete(eventArgs.Entity);
		}

		void IEventListener<File, LoadedEvent>.HandleEvent(EntityEventArgs<File> eventArgs)
		{
			//InitializeLazyLoad(eventArgs.Entity);
		}

		private void Store(File file, bool updateMode)
		{
			if (file == null)
				throw new ArgumentNullException("file");

			if (file.FileStream == null || file.FileStream.Value == null)
				return;

			FileStorageUtilities.StoreFile(file);
		}

		private void Delete(File file)
		{
			if (file == null)
				throw new ArgumentNullException("file");

			if (string.IsNullOrEmpty(file.StoragePath))
				return;

            if (!HasAnotherEntity(file))
			    FileStorageUtilities.DeleteFile(file);
		}

		private void InitializeLazyLoad(File file)
		{
			if (file == null)
				throw new ArgumentNullException("file");

			file.FileStream = new Lazy<Stream>(() => FileStorageUtilities.LoadFile(file));
		
		}
	}
}