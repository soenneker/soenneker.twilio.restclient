using Soenneker.Twilio.RestClient.Abstract;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Extensions.Configuration;
using Soenneker.Utils.AsyncSingleton;
using Soenneker.Utils.HttpClientCache.Abstract;
using Twilio.Clients;
using Twilio.Http;
using HttpClient = System.Net.Http.HttpClient;

namespace Soenneker.Twilio.RestClient;

/// <inheritdoc cref="ITwilioRestClientUtil"/>
///<inheritdoc cref="ITwilioRestClientUtil"/>
public class TwilioRestClientUtil : ITwilioRestClientUtil
{
    private readonly IHttpClientCache _httpClientCache;

    private readonly AsyncSingleton<TwilioRestClient> _restClient;

    public TwilioRestClientUtil(IConfiguration configuration, IHttpClientCache httpClientCache, ILogger<TwilioRestClientUtil> logger)
    {
        _httpClientCache = httpClientCache;

        _restClient = new AsyncSingleton<TwilioRestClient>(async () =>
        {
            logger.LogDebug("Initializing Twilio REST client...");

            var accountSid = configuration.GetValueStrict<string>("Twilio:AccountSid");
            var authToken = configuration.GetValueStrict<string>("Twilio:AuthToken");

            HttpClient httpClient = await httpClientCache.Get(nameof(TwilioRestClientUtil));

            return new TwilioRestClient(accountSid, authToken, httpClient: new SystemNetHttpClient(httpClient));
        });
    }

    public ValueTask<TwilioRestClient> Get()
    {
        return _restClient.Get();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _httpClientCache.RemoveSync(nameof(TwilioRestClientUtil));

        _restClient.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        await _httpClientCache.Remove(nameof(TwilioRestClientUtil));

        await _restClient.DisposeAsync();
    }
}