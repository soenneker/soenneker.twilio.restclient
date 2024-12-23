using Soenneker.Twilio.RestClient.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Twilio.RestClient.Tests;

[Collection("Collection")]
public class TwilioRestClientUtilTests : FixturedUnitTest
{
    private readonly ITwilioRestClientUtil _util;

    public TwilioRestClientUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<ITwilioRestClientUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
