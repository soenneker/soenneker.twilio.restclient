using System;
using System.Threading;
using System.Threading.Tasks;
using Twilio.Clients;

namespace Soenneker.Twilio.RestClient.Abstract;

/// <summary>
/// Provides a cached, authenticated Twilio REST client.
/// </summary>
public interface ITwilioRestClientUtil : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the authenticated client, creating it on first use.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel client creation.</param>
    /// <returns>The cached client.</returns>
    ValueTask<TwilioRestClient> Get(CancellationToken cancellationToken = default);
}
