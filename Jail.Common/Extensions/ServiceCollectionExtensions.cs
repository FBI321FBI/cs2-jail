using Jail.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Jail.Common.Extensions;
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Добавляет сервис <see cref="CapabilityService"/>.
	/// </summary>
	public static IServiceCollection AddCapabilityService(this IServiceCollection collection, Action<CapabilityService> actions)
	{
		collection.AddSingleton<CapabilityService>(serviceProvider =>
		{
			var capabilityService = new CapabilityService();
			actions(capabilityService);
			return capabilityService;
		});
		return collection;
	}
}
