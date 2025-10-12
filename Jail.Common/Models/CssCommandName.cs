using Jail.Common.Converters;

namespace Jail.Common.Models;

/// <summary>
/// Представляет класс наименования команды.
/// </summary>
public class CssCommandName
{
	#region Properties
	/// <summary>
	/// Команда.
	/// </summary>
	/// <remarks>
	/// Например css_test.
	/// </remarks>
	public string Value
	{
		get;
	}

	/// <summary>
	/// Представляет полную команду.
	/// </summary>
	public string FullValue
	{
		get => $"{Value} {string.Join(" ", Args)}";
	}

	/// <summary>
	/// Возвращает команду в формате Say.
	/// </summary>
	/// <remarks>
	/// css_test -> say "!test".
	/// </remarks>
	public string SayCommand
	{
		get => CssCommandsConverter.ConvertToSayFormat(FullValue);
	}

	/// <summary>
	/// Аргументы команды.
	/// </summary>
	public List<string> Args
	{
		get;
		private set;
	}
	#endregion

	#region .ctor
	/// <summary>
	/// Инициализирует экземпляр <see cref="CssCommandName"/>.
	/// </summary>
	/// <param name="value"></param>
	public CssCommandName(string value)
	{
		Value = value;
		Args = new();
	}

	public CssCommandName(string value, params string[] args)
	{
		Value = value;
		Args = new(args);
	}
	#endregion

	#region Public
	/// <summary>
	/// Добавляет аргументы.
	/// </summary>
	/// <param name="args">Аргументы.</param>
	public CssCommandName AddArgs(params string[] args)
	{
		foreach (var arg in args)
		{
			Args.Add(arg);
		}
		return this;
	}
	#endregion
}
