using UnityEngine;

namespace Infrastructure.Services.Factory
{
    public class FactoryObjectService<TCreatable> : IFactoryService<TCreatable>
        where TCreatable : ICreatable
    {
        protected readonly GameObject Prefab;
        protected readonly Transform Parent;

        public FactoryObjectService(GameObject prefab, Transform transformParent)
        {
            Prefab = prefab;
            Parent = transformParent;
        }

        public TCreatable CreateObject() => GameObject.Instantiate(Prefab, Parent).GetComponent<TCreatable>();
    }
}
