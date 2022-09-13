using FluentAssertions;
using miniapi.domain;
using miniapi.tests.common;
using System.Threading.Tasks;
using Xunit;

namespace miniapi.tests;
public class FirstMiniApiTests : FunctionalTestFixture
{
  [Fact]
  public async Task ShouldSayHelloAsync()
  {
    var response = await this.WithBrowserClient().GetAsync("https://miniapi/");
    response.EnsureSuccessStatusCode();
    var text = await response.Content.ReadAsStringAsync();
    text.Should().Be("Hello World!");
  }

  [Fact]
  public async Task ShouldGetItems()
  {
    this.factory.SeedItemAsync(new Item { Name = "Pelle" });
    var response = await this.WithBrowserClient().GetAsync("https://miniapi/items/");
    response.EnsureSuccessStatusCode();
    var text = await response.Content.ReadAsStringAsync();
    text.Should().Contain("name\":\"Pelle\"");
  }
}