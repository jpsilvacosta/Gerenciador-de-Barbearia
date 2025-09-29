namespace WebApi.Test
{
    [CollectionDefinition("Integration Tests", DisableParallelization = true)]
    public class IntegrationTestCollection : ICollectionFixture<CustomWebApplicationFactory>
    {
    }
}
