using System.Reflection;

namespace Jail.Common.Models.Listeners;

/// <summary>
/// Представляет базовый класс для обработчика слушателя.
/// </summary>
/// <typeparam name="T">Делегат слушателя.</typeparam>
public abstract class ListenerHandlerBase<TDelegate> where TDelegate : Delegate
{
	#region Data
	private Dictionary<Guid, TDelegate> _listeners;
	private ParameterInfo[] _delegateParameters;
	#endregion

	#region .ctor
	protected ListenerHandlerBase()
	{
		_listeners = new Dictionary<Guid, TDelegate>();

		var methodInfo = typeof(TDelegate).GetMethod("Invoke");
		_delegateParameters = methodInfo?.GetParameters() ?? new ParameterInfo[0];
	}
	#endregion

	#region Public
	/// <summary>
	/// Метод выполения обработчика.
	/// </summary>
	/// <param name="args">Аргументы слушателя.</param>
	public void Execute(params object[] args)
	{
		if (args.Length != _delegateParameters.Length)
		{
			throw new ArgumentException($"Ожидается {_delegateParameters.Length} аргументов, но получено {args.Length}");
		}

		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] != null && !_delegateParameters[i].ParameterType.IsInstanceOfType(args[i]))
			{
				throw new ArgumentException($"Аргумент {i} имеет неверный тип. Ожидается: {_delegateParameters[i].ParameterType}, получено: {args[i].GetType()}");
			}
		}

		Handle(args);

		foreach (var listener in _listeners.Values.ToList())
		{
			listener.DynamicInvoke(args);
		}
	}

	/// <summary>
	/// Добавляет дополнительное действие обработчику слушателя.
	/// </summary>
	/// <param name="listener">Слушатель.</param>
	/// <param name="conditionDelete">Условие удаления, после выполнения.</param>
	public void AddAdditionalListenerAction(TDelegate listener, Func<bool> conditionDelete = null)
	{
		var id = Guid.NewGuid();

		if (conditionDelete != null)
		{
			TDelegate wrappedListener = CreateWrappedListener(listener, id, conditionDelete);
			_listeners.Add(id, wrappedListener);
		}
		else
		{
			_listeners.Add(id, listener);
		}
	}

	/// <summary>
	/// Удаляет дополнительное действие обработчику слушателя.
	/// </summary>
	/// <param name="listener">Слушатель.</param>
	public void RemoveAdditionalListenerAction(TDelegate listener)
	{
		var keysToRemove = _listeners
			.Where(x => x.Value.Equals(listener))
			.Select(x => x.Key)
			.ToList();

		foreach (var key in keysToRemove)
		{
			_listeners.Remove(key);
		}
	}
	#endregion

	/// <summary>
	/// Обработчик слушателя.
	/// </summary>
	/// <param name="args">Аргументы слушателя.</param>
	protected abstract void Handle(object[] args);

	#region Private
	private TDelegate CreateWrappedListener(TDelegate originalListener, Guid id, Func<bool> conditionDelete)
	{
		var wrappedDelegate = CreateWrappedDelegateByParameterCount(originalListener, id, conditionDelete);
		return (TDelegate)wrappedDelegate;
	}

	private Delegate CreateWrappedDelegateByParameterCount(TDelegate originalListener, Guid id, Func<bool> conditionDelete)
	{
		int paramCount = _delegateParameters.Length;

		switch (paramCount)
		{
			case 0:
				return new Action(() =>
				{
					originalListener.DynamicInvoke(Array.Empty<object>());
					if (conditionDelete()) _listeners.Remove(id);
				});
			case 1:
				return CreateActionWrapper<object>(originalListener, id, conditionDelete);
			case 2:
				return CreateActionWrapper<object, object>(originalListener, id, conditionDelete);
			case 3:
				return CreateActionWrapper<object, object, object>(originalListener, id, conditionDelete);
			default:
				return new Action<object[]>(args =>
				{
					originalListener.DynamicInvoke(args);
					if (conditionDelete()) _listeners.Remove(id);
				});
		}
	}

	private Delegate CreateActionWrapper<T1>(TDelegate originalListener, Guid id, Func<bool> conditionDelete)
	{
		return new Action<T1>((arg1) =>
		{
			originalListener.DynamicInvoke(arg1);
			if (conditionDelete()) _listeners.Remove(id);
		});
	}

	private Delegate CreateActionWrapper<T1, T2>(TDelegate originalListener, Guid id, Func<bool> conditionDelete)
	{
		return new Action<T1, T2>((arg1, arg2) =>
		{
			originalListener.DynamicInvoke(arg1, arg2);
			if (conditionDelete()) _listeners.Remove(id);
		});
	}

	private Delegate CreateActionWrapper<T1, T2, T3>(TDelegate originalListener, Guid id, Func<bool> conditionDelete)
	{
		return new Action<T1, T2, T3>((arg1, arg2, arg3) =>
		{
			originalListener.DynamicInvoke(arg1, arg2, arg3);
			if (conditionDelete()) _listeners.Remove(id);
		});
	}
	#endregion
}
