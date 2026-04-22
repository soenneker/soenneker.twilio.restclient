using Soenneker.Twilio.RestClient.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Twilio.RestClient.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class TwilioRestClientUtilTests : HostedUnitTest
{
    private readonly ITwilioRestClientUtil _util;

    public TwilioRestClientUtilTests(Host host) : base(host)
    {
        _util = Resolve<ITwilioRestClientUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
