using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using Litium.Common.Extensions;

namespace Litium.Test.Common.Xunit.StateEnginePrototype
{
	public class StateEngine
	{
		private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, dynamic>> _states = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, dynamic>>();
		private readonly DummyUnitOfWorkContainer _unitOfWork;

		public StateEngine(DummyUnitOfWorkContainer unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public static void Add<TEntity, TState>(StateDefinition<TEntity, TState> stateDefinition)
		{
			var entityType = typeof (TEntity);
			var stateType = typeof (TState);
			_states.GetOrAdd(entityType, new ConcurrentDictionary<Type, dynamic>())
				.AddOrUpdate(stateType, stateDefinition, (type, o) => stateDefinition);
		}

		public void Execute<TEntity, TState>(TEntity entity, TState state, dynamic args = null)
		{
			var stateDefiniton = Get<TEntity, TState>();
			if (stateDefiniton == null)
			{
				throw new StateNotFoundForEntityException();
			}

			var stateTransition = stateDefiniton.Get(entity, state);
			if (stateTransition == null)
			{
				throw new StateTransitionNotAllowedException();
			}

			if (args == null)
			{
				if (stateTransition.ExpectParameterType != null)
				{
					throw new StateTransitionArgumentException(stateTransition.ExpectParameterType);
				}
			}
			else if (stateTransition.ExpectParameterType == null || !stateTransition.ExpectParameterType.IsInstanceOfType(args))
			{
				throw new StateTransitionArgumentException(stateTransition.ExpectParameterType);
			}

			if (stateTransition.Expect != null && !stateTransition.Expect(entity, args))
			{
				throw new StateTransitionNotAllowedException();
			}

			if (stateTransition.PreAction != null)
			{
				stateTransition.PreAction(_unitOfWork, entity, args);
			}

			stateDefiniton.PropertyInfo.SetValue(entity, state, BindingFlags.SetProperty | BindingFlags.NonPublic, null, null, Thread.CurrentThread.CurrentCulture);

			if (stateTransition.PostAction != null)
			{
				stateTransition.PostAction(_unitOfWork, entity, args);
			}
		}

		public ISet<TState> GetAvailableTransitions<TEntity, TState>(TEntity entity)
		{
			var stateDefiniton = Get<TEntity, TState>();
			if (stateDefiniton == null)
			{
				throw new StateNotFoundForEntityException();
			}
			return stateDefiniton.GetAvailableTransitions(entity);
		}

		private StateDefinition<TEntity, TState> Get<TEntity, TState>()
		{
			var entityType = typeof (TEntity);
			var stateType = typeof (TState);

			ConcurrentDictionary<Type, dynamic> value;
			if (_states.TryGetValue(entityType, out value))
			{
				dynamic o;
				if (value.TryGetValue(stateType, out o))
				{
					return o;
				}
			}
			return null;
		}
	}

	public class DummyUnitOfWorkContainer : IDisposable
	{
		private StateEngine _stateEngine;

		public StateEngine StateEngine
		{
			get { return _stateEngine ?? (_stateEngine = new StateEngine(this)); }
		}

		public void Dispose()
		{
		}
	}

	public class StateTransitionArgumentException : Exception
	{
		public StateTransitionArgumentException(Type type)
		{
			Type = type;
		}

		public StateTransitionArgumentException(string message)
			: base(message)
		{
		}

		public StateTransitionArgumentException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected StateTransitionArgumentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Type = info.GetValue("Type", typeof (Type)) as Type;
		}

		public Type Type { get; private set; }

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Type", Type, typeof (Type));
		}
	}

	public class StateTransitionNotAllowedException : Exception
	{
	}

	public class StateNotFoundForEntityException : Exception
	{
	}

	public class StateTransitionDefinition<TEntity, TState>
	{
		public StateTransitionDefinition(TState enterState, TState exitState)
		{
			Contract.Requires(!ReferenceEquals(null, enterState));
			Contract.Requires(!ReferenceEquals(null, exitState));

			EnterState = enterState;
			ExitState = exitState;
		}

		public TState EnterState { get; private set; }
		public TState ExitState { get; private set; }
		public Func<TEntity, dynamic, bool> Expect { get; set; }
		public Type ExpectParameterType { get; set; }
		public Action<DummyUnitOfWorkContainer, TEntity, dynamic> PostAction { get; set; }
		public Action<DummyUnitOfWorkContainer, TEntity, dynamic> PreAction { get; set; }
	}

	public class StateDefinition<TEntity, TState>
	{
		private readonly Func<TEntity, TState> _compiledProperty;
		private readonly ConcurrentDictionary<TState, ConcurrentDictionary<TState, StateTransitionDefinition<TEntity, TState>>> _states = new ConcurrentDictionary<TState, ConcurrentDictionary<TState, StateTransitionDefinition<TEntity, TState>>>();

		public StateDefinition(Expression<Func<TEntity, TState>> property, params StateTransitionDefinition<TEntity, TState>[] states)
		{
			Property = property;
			States = states;
			_compiledProperty = Property.Compile();
			PropertyInfo = Property.GetPropertyInfo();
			if (states != null)
			{
				foreach (var state in states)
				{
					StateTransitionDefinition<TEntity, TState> state1 = state;
					_states.GetOrAdd(state.EnterState, new ConcurrentDictionary<TState, StateTransitionDefinition<TEntity, TState>>())
						.AddOrUpdate(state.ExitState, state, (s, definition) => state1);
				}
			}
		}

		public Expression<Func<TEntity, TState>> Property { get; private set; }
		public PropertyInfo PropertyInfo { get; private set; }
		public StateTransitionDefinition<TEntity, TState>[] States { get; private set; }

		public StateTransitionDefinition<TEntity, TState> Get(TEntity entity, TState exitState)
		{
			var enterState = _compiledProperty.Invoke(entity);
			ConcurrentDictionary<TState, StateTransitionDefinition<TEntity, TState>> holder;
			if (_states.TryGetValue(enterState, out holder))
			{
				StateTransitionDefinition<TEntity, TState> value;
				if (holder.TryGetValue(exitState, out value))
				{
					return value;
				}
			}
			return null;
		}

		public ISet<TState> GetAvailableTransitions(TEntity entity)
		{
			var enterState = _compiledProperty.Invoke(entity);
			ConcurrentDictionary<TState, StateTransitionDefinition<TEntity, TState>> holder;
			if (_states.TryGetValue(enterState, out holder))
			{
				return holder.Keys.ToSet();
			}
			return new HashSet<TState>();
		}
	}
}
