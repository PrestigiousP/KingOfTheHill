using UnityEngine;

namespace Characters.scripts.Interfaces
{
    public interface IDamageable
    {
        public void GetPunched(Vector3 direction, float damage);
    }
}