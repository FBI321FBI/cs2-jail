using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Events;

namespace Jail.Common.Models.Events;

/// <summary>
/// Представляет базовый класс для обработчика события.
/// </summary>
public abstract class EventHandlerBase<T> where T : GameEvent
{
	private Dictionary<Guid, Action<T, GameEventInfo>> _actions;

	protected EventHandlerBase()
	{
		_actions = new();
	}

	/// <summary>
	/// Метод выполения обработчика.
	/// </summary>
	/// <param name="event">Событие.</param>
	/// <param name="info"><see cref="GameEventInfo"/>.</param>
	public void Execute(T @event, GameEventInfo info)
	{
		Handle(@event, info);

		foreach (var action in _actions.Values.ToList())
		{
			action(@event, info);
		}
	}

	/// <summary>
	/// Добавляет дополнительное действие для события.
	/// </summary>
	/// <param name="action">Действие.</param>
	/// <param name="conditionDelete">Условие для удаления из доп. действий.</param>
	public void AddAdditionalEventAction(Action<T, GameEventInfo> action, Func<bool> conditionDelete)
	{
		var id = Guid.NewGuid();
		Action<T, GameEventInfo> actionWrap = (@event, info) =>
		{
			action(@event, info);

			if (conditionDelete())
			{
				_actions.Remove(id);
			}
		};
		_actions.Add(id, actionWrap);
	}

	/// <summary>
	/// Удаление дополнительного действия для события.
	/// </summary>
	/// <param name="action">Действие.</param>
	public void RemoveAdditionalEventAction(Action<T, GameEventInfo> action)
	{
		var actionIdForRemove = _actions.SingleOrDefault(x => x.Value == action).Key;
		_actions.Remove(actionIdForRemove);
	}

	/// <summary>
	/// Обработка события.
	/// </summary>
	/// <param name="event">Событие.</param>
	/// <param name="info"><see cref="GameEventInfo"/>.</param>
	protected abstract void Handle(T @event, GameEventInfo info);
}
