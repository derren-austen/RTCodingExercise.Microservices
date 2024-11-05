using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Catalog.AcceptanceTests.Helpers;

public class CatalogApiClassFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //base.ConfigureWebHost(builder);
    }
}
