using Infrastructure.Services.Factory;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.Pool
{
    public class PoolObjects<TPoolable> where TPoolable : class, IPoolable
    {
        protected readonly IFactoryService<TPoolable> FactoryService;

        private readonly List<TPoolable> _poolables = new();

        public PoolObjects(IFactoryService<TPoolable> factoryService)
        {
            FactoryService = factoryService;
        }

        public TPoolable CreateObject(bool active)
        {
            TPoolable poolable = FactoryService.CreateObject();
            _poolables.Add(poolable);

            if (active == true)
                poolable.Activate();
            else
                poolable.Deactivate();

            return poolable;
        }

        public bool HasFreeObject(out TPoolable poolable)
        {
            foreach (TPoolable element in _poolables)
            {
                GameObject goPoolable = element.GameObject;

                if (goPoolable.activeInHierarchy == false)
                {
                    element.Activate();

                    poolable = element;
                    return true;
                }
            }

            poolable = null;
            return false;
        }

        public TPoolable GetFreeObject()
        {
            if (HasFreeObject(out TPoolable poolable))
                return poolable;

            return CreateObject(active: true);
        }
    }
}
