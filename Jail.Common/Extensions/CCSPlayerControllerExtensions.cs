using CounterStrikeSharp.API.Core;
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
}
