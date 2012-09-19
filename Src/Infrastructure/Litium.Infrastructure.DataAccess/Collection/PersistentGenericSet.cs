﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NHibernate.Collection;
using NHibernate.DebugHelpers;
using NHibernate.Engine;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using NHibernate.Util;

namespace Litium.Infrastructure.DataAccess.Collection
{
	/// <summary>
	/// 	A persistent wrapper for an <see cref = "ISet{T}" />
	/// </summary>
	[Serializable]
	[DebuggerTypeProxy(typeof (CollectionProxy<>))]
	public class PersistentGenericSet<T> : AbstractPersistentCollection, ISet<T>
	{
		/// <summary>
		/// 	The <see cref = "ISet{T}" /> that NHibernate is wrapping.
		/// </summary>
		protected ISet<T> set;

		/// <summary>
		/// 	A temporary list that holds the objects while the PersistentSet is being
		/// 	populated from the database.
		/// </summary>
		/// <remarks>
		/// 	This is necessary to ensure that the object being added to the PersistentSet doesn't
		/// 	have its' <c>GetHashCode()</c> and <c>Equals()</c> methods called during the load
		/// 	process.
		/// </remarks>
		[NonSerialized] private IList<T> tempList;

		public PersistentGenericSet()
		{
		}

		// needed for serialization

		/// <summary>
		/// 	Constructor matching super.
		/// 	Instantiates a lazy set (the underlying set is un-initialized).
		/// </summary>
		/// <param name = "session">The session to which this set will belong. </param>
		public PersistentGenericSet(ISessionImplementor session)
			: base(session)
		{
		}

		/// <summary>
		/// 	Instantiates a non-lazy set (the underlying set is constructed
		/// 	from the incoming set reference).
		/// </summary>
		/// <param name = "session">The session to which this set will belong. </param>
		/// <param name = "original">The underlying set data. </param>
		public PersistentGenericSet(ISessionImplementor session, ISet<T> original)
			: base(session)
		{
			// Sets can be just a view of a part of another collection.
			// do we need to copy it to be sure it won't be changing
			// underneath us?
			// ie. this.set.addAll(set);
			set = original;
			SetInitialized();
			IsDirectlyAccessible = true;
		}

		public override bool Empty
		{
			get { return set.Count == 0; }
		}

		public bool IsEmpty
		{
			get { return ReadSize() ? CachedSize == 0 : (set.Count == 0); }
		}

		public bool IsSynchronized
		{
			get { return false; }
		}

		public override bool RowUpdatePossible
		{
			get { return false; }
		}

		public object SyncRoot
		{
			get { return this; }
		}

		#region ISet<T> Members

		public int Count
		{
			get { return ReadSize() ? CachedSize : set.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Add(T o)
		{
			var exists = IsOperationQueueEnabled ? ReadElementExistence(o) : null;
			if (!exists.HasValue)
			{
				Initialize(true);
				if (set.Add(o))
				{
					Dirty();
					return true;
				}
				return false;
			}
			if (exists.Value)
			{
				return false;
			}
			QueueOperation(new SimpleAddDelayedOperation(this, o));
			return true;
		}

		void ICollection<T>.Add(T item)
		{
			Add(item);
		}

		public void Clear()
		{
			if (ClearQueueEnabled)
			{
				QueueOperation(new ClearDelayedOperation(this));
			}
			else
			{
				Initialize(true);
				if (set.Count != 0)
				{
					set.Clear();
					Dirty();
				}
			}
		}

		public bool Contains(T o)
		{
			var exists = ReadElementExistence(o);
			return exists == null ? set.Contains(o) : exists.Value;
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			Read();
			Array.Copy(set.ToArray(), 0, array, arrayIndex, Count);
		}

		//public bool ContainsAll(ICollection c)
		//{
		//    Read();
		//    return set.ContainsAll(c);
		//}

		public void ExceptWith(IEnumerable<T> other)
		{
			Read();
			set.ExceptWith(other);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			Read();
			return set.GetEnumerator();
		}

		public IEnumerator GetEnumerator()
		{
			Read();
			return set.GetEnumerator();
		}

		public void IntersectWith(IEnumerable<T> other)
		{
			Read();
			set.IntersectWith(other);
		}

		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			Read();
			return set.IsProperSubsetOf(other);
		}

		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			Read();
			return set.IsProperSupersetOf(other);
		}

		public bool IsSubsetOf(IEnumerable<T> other)
		{
			Read();
			return set.IsProperSupersetOf(other);
		}

		public bool IsSupersetOf(IEnumerable<T> other)
		{
			Read();
			return set.IsSupersetOf(other);
		}

		public bool Overlaps(IEnumerable<T> other)
		{
			Read();
			return set.Overlaps(other);
		}

		public bool Remove(T o)
		{
			var exists = PutQueueEnabled ? ReadElementExistence(o) : null;
			if (!exists.HasValue)
			{
				Initialize(true);
				if (set.Remove(o))
				{
					Dirty();
					return true;
				}
				return false;
			}
			if (exists.Value)
			{
				QueueOperation(new SimpleRemoveDelayedOperation(this, o));
				return true;
			}
			return false;
		}

		public bool SetEquals(IEnumerable<T> other)
		{
			Read();
			return set.SetEquals(other);
		}

		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			Read();
			set.SymmetricExceptWith(other);
		}

		public void UnionWith(IEnumerable<T> other)
		{
			Read();
			set.UnionWith(other);
		}

		#endregion

		#region DelayedOperations

		#region Nested type: ClearDelayedOperation

		protected sealed class ClearDelayedOperation : IDelayedOperation
		{
			private readonly PersistentGenericSet<T> enclosingInstance;

			public ClearDelayedOperation(PersistentGenericSet<T> enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}

			#region IDelayedOperation Members

			public object AddedInstance
			{
				get { return null; }
			}

			public object Orphan
			{
				get { throw new NotSupportedException("queued clear cannot be used with orphan delete"); }
			}

			public void Operate()
			{
				enclosingInstance.set.Clear();
			}

			#endregion
		}

		#endregion

		#region Nested type: SimpleAddDelayedOperation

		protected sealed class SimpleAddDelayedOperation : IDelayedOperation
		{
			private readonly PersistentGenericSet<T> enclosingInstance;
			private readonly T value;

			public SimpleAddDelayedOperation(PersistentGenericSet<T> enclosingInstance, T value)
			{
				this.enclosingInstance = enclosingInstance;
				this.value = value;
			}

			#region IDelayedOperation Members

			public object AddedInstance
			{
				get { return value; }
			}

			public object Orphan
			{
				get { return null; }
			}

			public void Operate()
			{
				enclosingInstance.set.Add(value);
			}

			#endregion
		}

		#endregion

		#region Nested type: SimpleRemoveDelayedOperation

		protected sealed class SimpleRemoveDelayedOperation : IDelayedOperation
		{
			private readonly PersistentGenericSet<T> enclosingInstance;
			private readonly T value;

			public SimpleRemoveDelayedOperation(PersistentGenericSet<T> enclosingInstance, T value)
			{
				this.enclosingInstance = enclosingInstance;
				this.value = value;
			}

			#region IDelayedOperation Members

			public object AddedInstance
			{
				get { return null; }
			}

			public object Orphan
			{
				get { return value; }
			}

			public void Operate()
			{
				enclosingInstance.set.Remove(value);
			}

			#endregion
		}

		#endregion

		#endregion

		public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
		{
			set = (ISet<T>) persister.CollectionType.Instantiate(anticipatedSize);
		}

		/// <summary>
		/// 	Set up the temporary List that will be used in the EndRead() 
		/// 	to fully create the set.
		/// </summary>
		public override void BeginRead()
		{
			base.BeginRead();
			tempList = new List<T>();
		}

		public void CopyTo(Array array, int index)
		{
			// NH : we really need to initialize the set ?
			Read();
			Array.Copy(set.ToArray(), 0, array, index, Count);
		}

		public override object Disassemble(ICollectionPersister persister)
		{
			var result = new object[set.Count];
			int i = 0;

			foreach (object obj in set)
			{
				result[i++] = persister.ElementType.Disassemble(obj, Session, null);
			}
			return result;
		}

		/// <summary>
		/// 	Takes the contents stored in the temporary list created during <c>BeginRead()</c>
		/// 	that was populated during <c>ReadFrom()</c> and write it to the underlying 
		/// 	PersistentSet.
		/// </summary>
		public override bool EndRead(ICollectionPersister persister)
		{
			foreach (T item in tempList)
			{
				set.Add(item);
			}
			tempList = null;
			SetInitialized();
			return true;
		}

		public override IEnumerable Entries(ICollectionPersister persister)
		{
			return set;
		}

		public override bool EntryExists(object entry, int i)
		{
			return true;
		}

		public override bool Equals(object other)
		{
			var that = other as ISet<T>;
			if (that == null)
			{
				return false;
			}
			Read();
			return set.SequenceEqual(that);
		}

		public override bool EqualsSnapshot(ICollectionPersister persister)
		{
			var elementType = persister.ElementType;
			var snapshot = (ISetSnapshot<T>) GetSnapshot();
			if (((ICollection) snapshot).Count != set.Count)
			{
				return false;
			}

			return !(from object obj in set
			         let oldValue = snapshot[(T) obj]
			         where oldValue == null || elementType.IsDirty(oldValue, obj, Session)
			         select obj).Any();
		}

		public override IEnumerable GetDeletes(ICollectionPersister persister, bool indexIsFormula)
		{
			IType elementType = persister.ElementType;
			var sn = (ISetSnapshot<T>) GetSnapshot();
			var deletes = new List<T>(((ICollection<T>) sn).Count);

			deletes.AddRange(sn.Where(obj => !set.Contains(obj)));

			deletes.AddRange(from obj in set
			                 let oldValue = sn[obj]
			                 where oldValue != null && elementType.IsDirty(obj, oldValue, Session)
			                 select oldValue);

			return deletes;
		}

		public override object GetElement(object entry)
		{
			return entry;
		}

		public override int GetHashCode()
		{
			Read();
			return set.GetHashCode();
		}

		public override object GetIndex(object entry, int i, ICollectionPersister persister)
		{
			throw new NotSupportedException("Sets don't have indexes");
		}

		public override ICollection GetOrphans(object snapshot, string entityName)
		{
			var sn = new SetSnapShot<object>((IEnumerable<object>) snapshot);
			if (set.Count == 0) return sn;
			if (((ICollection) sn).Count == 0) return sn;
			return GetOrphans(sn, set.ToArray(), entityName, Session);
		}

		public override ICollection GetSnapshot(ICollectionPersister persister)
		{
			var entityMode = Session.EntityMode;
			var clonedSet = new SetSnapShot<T>(set.Count);
			var enumerable = from object current in set
			                 select persister.ElementType.DeepCopy(current, entityMode, persister.Factory);
			foreach (var copied in enumerable)
			{
				clonedSet.Add((T) copied);
			}
			return clonedSet;
		}

		public override object GetSnapshotElement(object entry, int i)
		{
			throw new NotSupportedException("Sets don't support updating by element");
		}

		/// <summary>
		/// 	Initializes this PersistentSet from the cached values.
		/// </summary>
		/// <param name = "persister">The CollectionPersister to use to reassemble the PersistentSet.</param>
		/// <param name = "disassembled">The disassembled PersistentSet.</param>
		/// <param name = "owner">The owner object.</param>
		public override void InitializeFromCache(ICollectionPersister persister, object disassembled, object owner)
		{
			var array = (object[]) disassembled;
			int size = array.Length;
			BeforeInitialize(persister, size);
			for (int i = 0; i < size; i++)
			{
				var element = (T) persister.ElementType.Assemble(array[i], Session, owner);
				if (element != null)
				{
					set.Add(element);
				}
			}
			SetInitialized();
		}

		public override bool IsSnapshotEmpty(object snapshot)
		{
			return ((ICollection) snapshot).Count == 0;
		}

		public override bool IsWrapper(object collection)
		{
			return set == collection;
		}

		public override bool NeedsInserting(object entry, int i, IType elemType)
		{
			var sn = (ISetSnapshot<T>) GetSnapshot();
			object oldKey = sn[(T) entry];
			// note that it might be better to iterate the snapshot but this is safe,
			// assuming the user implements equals() properly, as required by the PersistentSet
			// contract!
			return oldKey == null || elemType.IsDirty(oldKey, entry, Session);
		}

		public override bool NeedsUpdating(object entry, int i, IType elemType)
		{
			return false;
		}

		public new void Read()
		{
			base.Read();
		}

		public override object ReadFrom(IDataReader rs, ICollectionPersister role, ICollectionAliases descriptor, object owner)
		{
			var element = (T) role.ReadElement(rs, owner, descriptor.SuffixedElementAliases, Session);
			if (element != null)
			{
				tempList.Add(element);
			}
			return element;
		}

		public override string ToString()
		{
			Read();
			return StringHelper.CollectionToString(set.ToArray());
		}

		#region Nested type: ISetSnapshot

		private interface ISetSnapshot<S> : ICollection<S>, ICollection
		{
			S this[S element] { get; }
		}

		#endregion

		#region Nested type: SetSnapShot

		[Serializable]
		private class SetSnapShot<S> : ISetSnapshot<S>
		{
			private readonly List<S> elements;

			private SetSnapShot()
			{
				elements = new List<S>();
			}

			public SetSnapShot(int capacity)
			{
				elements = new List<S>(capacity);
			}

			public SetSnapShot(IEnumerable<S> collection)
			{
				elements = new List<S>(collection);
			}

			#region ISetSnapshot<S> Members

			int ICollection.Count
			{
				get { return elements.Count; }
			}

			int ICollection<S>.Count
			{
				get { return elements.Count; }
			}

			public bool IsReadOnly
			{
				get { return ((ICollection<T>) elements).IsReadOnly; }
			}

			public bool IsSynchronized
			{
				get { return ((ICollection) elements).IsSynchronized; }
			}

			public S this[S element]
			{
				get
				{
					int idx = elements.IndexOf(element);
					if (idx >= 0)
					{
						return elements[idx];
					}
					return default(S);
				}
			}

			public object SyncRoot
			{
				get { return ((ICollection) elements).SyncRoot; }
			}

			public void Add(S item)
			{
				elements.Add(item);
			}

			public void Clear()
			{
				throw new InvalidOperationException();
			}

			public bool Contains(S item)
			{
				return elements.Contains(item);
			}

			public void CopyTo(S[] array, int arrayIndex)
			{
				elements.CopyTo(array, arrayIndex);
			}

			public void CopyTo(Array array, int index)
			{
				((ICollection) elements).CopyTo(array, index);
			}

			public IEnumerator<S> GetEnumerator()
			{
				return elements.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public bool Remove(S item)
			{
				throw new InvalidOperationException();
			}

			#endregion
		}

		#endregion
	}
}