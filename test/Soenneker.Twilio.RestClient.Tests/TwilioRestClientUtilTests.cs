using System.Threading.Tasks;
using Soenneker.Facts.Local;
using Soenneker.Twilio.RestClient.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

using FluentAssertions;
using System;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Soenneker.Twilio.RestClient.Tests;

[Collection("Collection")]
public class TwilioRestClientUtilTests : FixturedUnitTest
{
    private readonly ITwilioRestClientUtil _util;

    public TwilioRestClientUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<ITwilioRestClientUtil>(true);
    }

    [LocalFact]
    public async Task Get_should_get_client()
    {
        var client = await _util.Get();
        client.Should().NotBeNull();
    }

    [LocalFact]
    public async Task Send_should_send()
    {
        var client = await _util.Get();

        var message = await MessageResource.CreateAsync(
            new PhoneNumber("+13202666853"),
            from: new PhoneNumber("+18886350693"),
            body: "Hello World!",
            client: client
        );

        Console.WriteLine(message.Sid);
    }
}