using System;
using System.Threading;
using System.Threading.Tasks;
using Twilio.Clients;

namespace Soenneker.Twilio.RestClient.Abstract;

/// <summary>
/// An async thread-safe singleton for a Twilio RestClient
/// </summary>
public interface ITwilioRestClientUtil : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<TwilioRestClient> Get(CancellationToken cancellationToken = default);
}
