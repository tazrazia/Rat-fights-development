namespace Infrastructure.Services.Factory
{
    public interface IFactoryService<TCreatable> where TCreatable : ICreatable
    {
        TCreatable CreateObject();
    }
}
