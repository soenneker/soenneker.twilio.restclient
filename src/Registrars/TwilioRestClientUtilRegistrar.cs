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
    public static void AddTwilioRestClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCache();
        services.TryAddSingleton<ITwilioRestClientUtil, TwilioRestClientUtil>();
    }

    /// <summary>
    /// Adds <see cref="ITwilioRestClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static void AddTwilioRestClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCache();
        services.TryAddScoped<ITwilioRestClientUtil, TwilioRestClientUtil>();
    }
}
