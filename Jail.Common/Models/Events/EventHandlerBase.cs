using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Events;

namespace Jail.Common.Models.Events;

/// <summary>
/// Представляет базовый класс для обработчика события.
/// </summary>
public abstract class EventHandlerBase<T> where T : GameEvent
{
	#region Data
	#region Context
	/// <summary>
	/// Представляет контекст дополнительного действия.
	/// </summary>
	/// <param name="Action">Действие.</param>
	/// <param name="HookMode"><see cref="HookMode"/>.</param>
	private record AdditionalActionContext(Action<T, GameEventInfo> Action, HookMode HookMode);
	#endregion

	private Dictionary<Guid, AdditionalActionContext> _actions;
	#endregion

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
		var actionsPre = _actions.Values.Where(x => x.HookMode == HookMode.Pre);
		var actionsPost = _actions.Values.Where(x => x.HookMode == HookMode.Post);

		foreach(var actionPre in actionsPre)
		{
			actionPre.Action(@event, info);
		}

		Handle(@event, info);

		foreach (var actionPost in actionsPost)
		{
			actionPost.Action(@event, info);
		}
	}

	/// <summary>
	/// Добавляет дополнительное действие для события.
	/// </summary>
	/// <param name="action">Действие.</param>
	/// <param name="conditionDelete">Условие для удаления из доп. действий.</param>
	/// <param name="actionMode">Когда будет вызвано действие. По стандарту после основного обработчика.</param>
	/// <param name="conditionDeleteMode">Когда будет вызвана проверка на удаления действия.
	/// По стандарту после основного обработчика.</param>
	public void AddAdditionalEventAction(
		Action<T, GameEventInfo> action, 
		Func<bool> conditionDelete, 
		HookMode actionMode = HookMode.Post,
		HookMode conditionDeleteMode = HookMode.Post)
	{
		var id = Guid.NewGuid();
		Action<T, GameEventInfo> actionWrap = (@event, info) =>
		{
			if (conditionDeleteMode == HookMode.Pre)
			{
				if (conditionDelete())
				{
					_actions.Remove(id);
				}
				else
				{
					action(@event, info);
				}
			}
			else
			{
				action(@event, info);
				if (conditionDelete())
				{
					_actions.Remove(id);
				}
			}
		};

		var actionContext = new AdditionalActionContext(actionWrap, actionMode);
		_actions.Add(id, actionContext);
	}

	/// <summary>
	/// Удаление дополнительного действия для события.
	/// </summary>
	/// <param name="action">Действие.</param>
	public void RemoveAdditionalEventAction(Action<T, GameEventInfo> action)
	{
		var actionIdForRemove = _actions.SingleOrDefault(x => x.Value.Action == action).Key;
		_actions.Remove(actionIdForRemove);
	}

	/// <summary>
	/// Обработка события.
	/// </summary>
	/// <param name="event">Событие.</param>
	/// <param name="info"><see cref="GameEventInfo"/>.</param>
	protected abstract void Handle(T @event, GameEventInfo info);
}
