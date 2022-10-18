using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure
{
    // First in script execution order
    public class Bootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            RegisterServices();
        }

        private void RegisterServices()
        {
            // Register HeroFactory, UnitFactory and UnitPool
            AllServices.RegisterSingle<PlayerInput>(new PlayerInput());
            // Get input by: AllServices.Single<PlayerInput>().
        }
    }
}
