namespace ODSphereRouter.DbContexts
{
    public interface ISphereRouterDbContextFactory
    {
        SphereRouterDbContext CreateDbContext();
    }
}
