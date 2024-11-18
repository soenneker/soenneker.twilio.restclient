using Soenneker.Twilio.RestClient.Abstract;
using System.Threading.Tasks;
using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Extensions.Configuration;
using Soenneker.Utils.AsyncSingleton;
using Soenneker.Utils.HttpClientCache.Abstract;
using Twilio.Clients;
using Twilio.Http;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Twilio.RestClient;

/// <inheritdoc cref="ITwilioRestClientUtil"/>
public class TwilioRestClientUtil : ITwilioRestClientUtil
{
    private readonly IHttpClientCache _httpClientCache;

    private readonly AsyncSingleton<TwilioRestClient> _restClient;

    public TwilioRestClientUtil(IConfiguration configuration, IHttpClientCache httpClientCache, ILogger<TwilioRestClientUtil> logger)
    {
        _httpClientCache = httpClientCache;

        _restClient = new AsyncSingleton<TwilioRestClient>(async (token, _) =>
        {
            logger.LogDebug("Initializing Twilio REST client...");

            var accountSid = configuration.GetValueStrict<string>("Twilio:AccountSid");
            var authToken = configuration.GetValueStrict<string>("Twilio:AuthToken");

            System.Net.Http.HttpClient httpClient = await httpClientCache.Get(nameof(TwilioRestClientUtil), cancellationToken: token).NoSync();

            return new TwilioRestClient(accountSid, authToken, httpClient: new SystemNetHttpClient(httpClient));
        });
    }

    public ValueTask<TwilioRestClient> Get(CancellationToken cancellationToken = default)
    {
        return _restClient.Get(cancellationToken);
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

        await _httpClientCache.Remove(nameof(TwilioRestClientUtil)).NoSync();

        await _restClient.DisposeAsync().NoSync();
    }
}