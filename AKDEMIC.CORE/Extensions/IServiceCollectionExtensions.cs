using Microsoft.Extensions.DependencyInjection;

namespace AKDEMIC.CORE.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void RemoveCollection<TServiceType, TImplementationType>(this IServiceCollection services)
        {
            foreach (var service in services)
            {
                if (service.ServiceType == typeof(TServiceType) && service.ImplementationType == typeof(TImplementationType))
                {
                    services.Remove(service);
                    break;
                }
            }
        }
    }
}
