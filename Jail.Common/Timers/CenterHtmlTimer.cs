using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Timers;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace Jail.Common.Timers;

public class CenterHtmlTimer
{
	private Timer _timer;
	private int _totalSeconds;
	private CenterHtmlMenu _menuTimer;

	public CenterHtmlTimer(
		int totalSeconds,
		BasePlugin plugin)
	{
		_totalSeconds = totalSeconds;
		_menuTimer = new ("", plugin);
	}

	public void StartCountdown(IEnumerable<CCSPlayerController> players)
	{
		PrepareTimer();
		foreach (var player in players)
		{
			_menuTimer.Open(player);
		}
	}

	public void Stop()
	{
		if (_timer == null)
		{
			throw new ArgumentNullException("Таймер ещё не начался.");
		}

		_timer.Kill();
	}

	private void PrepareTimer()
	{
		_totalSeconds++;
		_timer = new(1, () =>
		{
			if (_totalSeconds == 0)
			{
				Stop();
			}

			_menuTimer.Title = (_totalSeconds--).ToString();
		}, TimerFlags.REPEAT);
	}
}
