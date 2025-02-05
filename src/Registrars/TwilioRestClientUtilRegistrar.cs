using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Twilio.RestClient.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.Twilio.RestClient.Registrars;

/// <summary>
/// An async thread-safe singleton for a Twilio RestClient
/// </summary>
public static class TwilioRestClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="ITwilioRestClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddTwilioRestClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddSingleton<ITwilioRestClientUtil, TwilioRestClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="ITwilioRestClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddTwilioRestClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddScoped<ITwilioRestClientUtil, TwilioRestClientUtil>();

        return services;
    }
}