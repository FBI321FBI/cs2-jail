namespace Jail.Common.Converters;

/// <summary>
/// Представляет класс конвертации css команд.
/// </summary>
public static class CssCommandsConverter
{
	#region Public
	/// <summary>
	/// Преобразует css команду в формат написания через say в консоле.
	/// </summary>
	/// <remarks>
	/// css_test -> say "!test"
	/// </remarks>
	/// <param name="consoleCommand">Css команда.</param>
	/// <returns></returns>
	public static string ConvertToSayFormat(string consoleCommand)
	{
		if (!IsCssCommand(consoleCommand))
		{
			throw new InvalidOperationException($"Команда {consoleCommand} не является css командой.");
		}

		var commandWithoutCss = consoleCommand.Replace("css_", "");
		return $"say \"!{commandWithoutCss}\"";
	}
	#endregion

	#region Private
	private static bool IsCssCommand(string cssCommand) =>
		cssCommand.ToLower().StartsWith("css_");
	#endregion
}
