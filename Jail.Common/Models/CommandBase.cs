using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;

namespace Jail.Common.Models;

/// <summary>
/// Базовый класс для консольных комманд.
/// </summary>
public abstract class CommandBase
{
	protected CCSPlayerController? Player;
	protected CommandInfo Info;

	/// <summary>
	/// Инициализирует <see cref="CommandBase"/>.
	/// </summary>
	/// <param name="player"><see cref="CCSPlayerController"/>.</param>
	/// <param name="info"><see cref="CommandInfo"/>.</param>
	public CommandBase(CCSPlayerController? player, CommandInfo info)
	{
		Player = player;
		Info = info;
	}

	public abstract void Execute();
}
