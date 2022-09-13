using System;
using System.Net.Http;
using Xunit;

namespace miniapi.tests.common;
public class FunctionalTestFixture : IClassFixture<MiniApiApplicationFactory>, IDisposable
{
    internal readonly MiniApiApplicationFactory factory;
    public HttpMessageHandler Handler { get; set; }

    private BrowserClient browserClient;
    public BrowserClient WithBrowserClient()
    {
        if (this.browserClient == null)
        {
            this.browserClient = new BrowserClient(new BrowserHandler(Handler));
        }
        return this.browserClient;
    }

    public FunctionalTestFixture()
    {
        this.factory = new MiniApiApplicationFactory();
        Handler = factory.Server.CreateHandler();
    }

    public void Dispose()
    {
        this.factory.TestCleanup();
    }
}
