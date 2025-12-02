using Xunit;

namespace ApiTests.Tests;

[CollectionDefinition("Api collection")]
public class ApiCollection : ICollectionFixture<ApiTests.Infrastructure.ApiTestFixture>
{
}
