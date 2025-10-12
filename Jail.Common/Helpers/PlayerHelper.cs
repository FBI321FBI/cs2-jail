using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

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
}
