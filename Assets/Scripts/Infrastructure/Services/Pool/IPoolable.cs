using Infrastructure.Services.Factory;

namespace Infrastructure.Services.Pool
{
    public interface IPoolable : ICreatable
    {
        void Activate();
        void Deactivate();
    }
}
