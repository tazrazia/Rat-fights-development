using UnityEngine;

namespace Infrastructure.Services.Factory
{
    public interface ICreatable
    {
        public GameObject GameObject { get; }
    }
}
