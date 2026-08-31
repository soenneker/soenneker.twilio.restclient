using Soenneker.Twilio.RestClient.Abstract;
using System.Threading.Tasks;
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

public sealed class TwilioRestClientUtil : ITwilioRestClientUtil
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TwilioRestClientUtil> _logger;
    private readonly bool _removeHttpClientOnDispose;

    private readonly AsyncSingleton<TwilioRestClient> _restClient;

    public TwilioRestClientUtil(IConfiguration configuration, IHttpClientCache httpClientCache, ILogger<TwilioRestClientUtil> logger)
        : this(configuration, httpClientCache, logger, removeHttpClientOnDispose: true)
    {
    }

    internal TwilioRestClientUtil(IConfiguration configuration, IHttpClientCache httpClientCache, ILogger<TwilioRestClientUtil> logger,
        bool removeHttpClientOnDispose)
    {
        _configuration = configuration;
        _httpClientCache = httpClientCache;
        _logger = logger;
        _removeHttpClientOnDispose = removeHttpClientOnDispose;

        _restClient = new AsyncSingleton<TwilioRestClient>(CreateClient);
    }

    private async ValueTask<TwilioRestClient> CreateClient(CancellationToken token)
    {
        _logger.LogDebug("Initializing Twilio REST client...");

        var accountSid = _configuration.GetValueStrict<string>("Twilio:AccountSid");
        var authToken = _configuration.GetValueStrict<string>("Twilio:AuthToken");

        System.Net.Http.HttpClient httpClient = await _httpClientCache.Get(nameof(TwilioRestClientUtil), cancellationToken: token)
                                                                     .NoSync();

        return new TwilioRestClient(accountSid, authToken, httpClient: new SystemNetHttpClient(httpClient));
    }

    public ValueTask<TwilioRestClient> Get(CancellationToken cancellationToken = default)
    {
        return _restClient.Get(cancellationToken);
    }

    public void Dispose()
    {
        if (_removeHttpClientOnDispose)
            _httpClientCache.RemoveSync(nameof(TwilioRestClientUtil));

        _restClient.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_removeHttpClientOnDispose)
        {
            await _httpClientCache.Remove(nameof(TwilioRestClientUtil))
                                  .NoSync();
        }

        await _restClient.DisposeAsync()
                         .NoSync();
    }
}
