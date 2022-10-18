namespace Infrastructure.Services
{
    // Service-locator instead of plugins DI.
    public class AllServices
    {
        public static AllServices Container => _instance ??= new AllServices(); 

        private static AllServices _instance;

        public static void RegisterSingle<TService>(TService implementation) where TService : IService
            => Implementation<TService>.Instance = implementation;

        public static TService Single<TService>() where TService : IService
            => Implementation<TService>.Instance;

        public class Implementation<TService> where TService : IService
        {
            public static TService Instance;
        }
    }
}
