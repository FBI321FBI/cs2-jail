using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using Jail.Common.Models;

namespace Jail.Common.Extensions;

/// <summary>
/// Представляет расширения для <see cref="CCSPlayerController"/>
/// </summary>
public static class CCSPlayerControllerExtensions
{
	/// <summary>
	/// Выполняет <see cref="CssCommand"/> в формате say.
	/// </summary>
	/// <param name="controller"><see cref="CCSPlayerController"/>.</param>
	/// <param name="command"><see cref="CssCommand"/>.</param>
	public static void ExecuteClientCommandFromServer(this CCSPlayerController player, CssCommand command)
	{
		player.ExecuteClientCommandFromServer(command.SayCommand);
	}

	/// <summary>
	/// Проверка на активное меню у игрока.
	/// </summary>
	/// <typeparam name="T">Меню.</typeparam>
	/// <returns>true, если меню активно и false, если нет.</returns>
	public static bool IsActiveMenu<T>(this CCSPlayerController player)
	{
		var activeMenu = MenuManager.GetActiveMenu(player);
		var baseMenuInstance = activeMenu as BaseMenuInstance;
		if (baseMenuInstance?.Menu is T)
		{
			return true;
		}

		return false;
	}
}
