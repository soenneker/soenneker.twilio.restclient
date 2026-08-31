[![](https://img.shields.io/nuget/v/soenneker.twilio.restclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.twilio.restclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.twilio.restclient/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.twilio.restclient/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.twilio.restclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.twilio.restclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.twilio.restclient/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.twilio.restclient/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Twilio.RestClient

Provides a cached, authenticated `TwilioRestClient` backed by a shared `HttpClient`.

## Installation

```bash
dotnet add package Soenneker.Twilio.RestClient
```

## Configuration

```json
{
  "Twilio": {
    "AccountSid": "AC...",
    "AuthToken": "..."
  }
}
```

## Registration

```csharp
using Soenneker.Twilio.RestClient.Registrars;

services.AddTwilioRestClientUtilAsScoped();
```

Scoped registration creates a `TwilioRestClient` wrapper per scope while retaining the shared cached HTTP client when a scope ends. Use `AddTwilioRestClientUtilAsSingleton()` to share the wrapper too; disposing that singleton removes its cached HTTP client.

## Send a message

```csharp
using Soenneker.Twilio.RestClient.Abstract;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public sealed class MessageSender
{
    private readonly ITwilioRestClientUtil _clients;

    public MessageSender(ITwilioRestClientUtil clients)
    {
        _clients = clients;
    }

    public async ValueTask<string?> Send(string destination, string sender, string body)
    {
        TwilioRestClient client = await _clients.Get();

        MessageResource message = await MessageResource.CreateAsync(
            to: new PhoneNumber(destination),
            from: new PhoneNumber(sender),
            body: body,
            client: client);

        return message.Sid;
    }
}
```

Pass the returned client explicitly to Twilio SDK resource methods. This avoids relying on the SDK's process-wide static `TwilioClient` state. Twilio API failures propagate as Twilio SDK exceptions.
