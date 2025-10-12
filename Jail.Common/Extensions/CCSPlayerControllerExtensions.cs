using CounterStrikeSharp.API.Core;
using Jail.Common.Models;

namespace Jail.Common.Extensions;

/// <summary>
/// Представляет расширения для <see cref="CCSPlayerController"/>
/// </summary>
public static class CCSPlayerControllerExtensions
{
	/// <summary>
	/// Выполняет <see cref="CssCommandName"/> в формате say.
	/// </summary>
	/// <param name="controller"><see cref="CCSPlayerController"/>.</param>
	/// <param name="command"><see cref="CssCommandName"/>.</param>
	public static void ExecuteClientCommandFromServer(this CCSPlayerController player, CssCommandName command)
	{
		player.ExecuteClientCommandFromServer(command.SayCommand);
	}
}
