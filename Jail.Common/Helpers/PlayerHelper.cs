using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace Jail.Common.Helpers;

/// <summary>
/// Помощник по игрокам.
/// </summary>
public static class PlayerHelper
{
	/// <summary>
	/// Находит игрока по слоту типа string.
	/// </summary>
	/// <param name="slot">Слот.</param>
	/// <returns><see cref="CCSPlayerController"/>.</returns>
	public static CCSPlayerController? FindPlayerBySlotTypeString(string slot)
	{
		if (!int.TryParse(slot, out int playerSlot))
		{
			return null;
		}

		var player = Utilities.GetPlayerFromSlot(playerSlot);

		return player;
	}

	/// <summary>
	/// Возвращает всех живых игроков.
	/// </summary>
	public static List<CCSPlayerController> GetAlivePlayers()
		=> Utilities.GetPlayers().Where(x => x.PawnIsAlive).ToList();

	/// <summary>
	/// Возвращает всех живых Т игроков.
	/// </summary>
	public static List<CCSPlayerController> GetAliveTPlayers()
		=> GetAlivePlayers().Where(x => x.Team == CsTeam.Terrorist).ToList();

	/// <summary>
	/// Возвращает всех живых КТ игроков.
	/// </summary>
	public static List<CCSPlayerController> GetAliveCTPlayers()
		=> GetAlivePlayers().Where(x => x.Team == CsTeam.CounterTerrorist).ToList();

	/// <summary>
	/// Возвращает всех мёртвых игроков.
	/// </summary>
	public static List<CCSPlayerController> GetDeadPlayers()
		=> Utilities.GetPlayers().Where(x => !x.PawnIsAlive).ToList();

	/// <summary>
	/// Возвращает всех мёртвых Т игроков.
	/// </summary>
	public static List<CCSPlayerController> GetDeadTPlayers()
		=> GetDeadPlayers().Where(x => x.Team == CsTeam.Terrorist).ToList();

	/// <summary>
	/// Возвращает всех мёртвых КТ игроков.
	/// </summary>
	public static List<CCSPlayerController> GetDeadCTPlayers()
		=> GetDeadPlayers().Where(x => x.Team == CsTeam.CounterTerrorist).ToList();
}
