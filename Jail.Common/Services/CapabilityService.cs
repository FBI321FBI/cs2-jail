using CounterStrikeSharp.API.Core.Capabilities;

namespace Jail.Common.Services;

/// <summary>
/// Представляет сервис вохможностей CounterStrikeSharp.
/// </summary>
public class CapabilityService
{
	private readonly Dictionary<Type, object> _pluginCapabilities = new();

	public void RegisterPluginCapability<T>(string name)
	{
		var capability = new PluginCapability<T>(name);
		_pluginCapabilities[typeof(T)] = capability;
	}

	public PluginCapability<T> GetPluginCapability<T>()
	{
		if (_pluginCapabilities.TryGetValue(typeof(T), out var capability))
		{
			if (capability is PluginCapability<T> typedCapability)
			{
				return typedCapability;
			}
		}

		throw new InvalidOperationException($"Capability {typeof(T).Name} не зарегистрирована.");
	}
}
