[![](https://img.shields.io/nuget/v/soenneker.twilio.restclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.twilio.restclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.twilio.restclient/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.twilio.restclient/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.twilio.restclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.twilio.restclient/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Twilio.RestClient
### An async thread-safe singleton for a Twilio RestClient

## Installation

```
dotnet add package Soenneker.Twilio.RestClient
```

## Why?

This library provides a singleton of a `TwilioRestClient`. 

Internally it implements an `HttpClient` singleton. This `HttpClient` has less overhead than new instances of `HttpClient` and `IHttpClientFactory` all while correctly handling connection pooling for DNS changes.

See [soenneker.utils.httpclientcache](https://github.com/soenneker/soenneker.utils.httpclientcache) for more information.

## Usage

1. Register `ITwilioRestClientUtil` with DI.

```csharp
public static async Task Main(string[] args)
{
    ...
    builder.Services.AddTwilioRestClientUtil();
}
```

2. Inject `ITwilioRestClientUtil` via constructor, and retrieve a `TwilioRestClient`.

```csharp
public class TestClass
{
    ITwilioRestClientUtil _twilioRestClientUtil;

    public TestClass(ITwilioRestClientUtil twilioRestClientUtil)
    {
        _twilioRestClientUtil = twilioRestClientUtil;
    }

    public async ValueTask SendMessage()
    {
        var message = await MessageResource.CreateAsync(
            new PhoneNumber("+11234567890"),
            from: new PhoneNumber("+10987654321"),
            body: "Hello World!",
            client: await _twilioRestClientUtil.Get()
        );

        Console.WriteLine(message.Sid);
    }
```