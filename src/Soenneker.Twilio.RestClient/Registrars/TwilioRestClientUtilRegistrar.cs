using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Soenneker.Twilio.RestClient.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.Twilio.RestClient.Registrars;

/// <summary>
/// Registers authenticated Twilio REST clients.
/// </summary>
public static class TwilioRestClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="ITwilioRestClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddTwilioRestClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton().TryAddSingleton<ITwilioRestClientUtil, TwilioRestClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="ITwilioRestClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddTwilioRestClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddScoped<ITwilioRestClientUtil>(provider =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    var httpClientCache = provider.GetRequiredService<IHttpClientCache>();
                    var logger = provider.GetRequiredService<ILogger<TwilioRestClientUtil>>();

                    return new TwilioRestClientUtil(configuration, httpClientCache, logger, removeHttpClientOnDispose: false);
                });

        return services;
    }
}
