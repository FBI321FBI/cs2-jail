using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Timers;
using Jail.Common.Extensions;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace Jail.Common.Timers;

/// <summary>
/// Таймер для игроков по центру.
/// </summary>
public class CenterHtmlTimer : CenterHtmlMenu
{
	public bool IsRunning => _timer != null;
	private Timer? _timer;
	private int _totalSeconds;
	private int _remainingSeconds;
	private BasePlugin _plugin;
	private Action? _onComplete;

	/// <summary>
	/// Инициализирует экземпляр <see cref="CenterHtmlTimer"/>.
	/// </summary>
	/// <param name="totalSeconds">Кол-во секунд до окончания.</param>
	/// <param name="plugin"><see cref="BasePlugin"/>.</param>
	/// <param name="onComplete">Действие после окончания.</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CenterHtmlTimer(
		int totalSeconds,
		BasePlugin plugin,
		Action? onComplete = null) : base(totalSeconds.ToString(), plugin)
	{
		_plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
		_totalSeconds = totalSeconds;
		_onComplete = onComplete;

		ExitButton = false;
	}

	public override void Open(CCSPlayerController player)
	{
		StartTimerIfNotRunning();
		base.Open(player);
	}

	public void Open(IEnumerable<CCSPlayerController> players)
	{
		StartTimerIfNotRunning();
		foreach (var player in players)
		{
			Open(player);
		}
	}

	public void Stop()
	{
		if (_timer != null)
		{
			_timer.Kill();
			_timer = null;
		}

		CloseMenuForAll();
	}

	private void StartTimerIfNotRunning()
	{
		if (_timer != null) return;

		_remainingSeconds = _totalSeconds;
		Title = _remainingSeconds.ToString();

		_timer = _plugin.AddTimer(1, () =>
		{
			_remainingSeconds--;
			if (_remainingSeconds <= 0)
			{
				Stop();
				_onComplete?.Invoke();
				return;
			}

			Title = _remainingSeconds.ToString();
		}, TimerFlags.REPEAT);
	}

	private void CloseMenuForAll()
	{
		foreach (var player in Utilities.GetPlayers())
		{
			if (player.IsActiveMenu<CenterHtmlTimer>())
			{
				MenuManager.CloseActiveMenu(player);
			}
		}
	}
}
