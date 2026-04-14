using UnityEngine;

namespace Classes.Interfaces
{
    public interface IDamageable
    {
        public void GetPunched(Vector3 direction, float damage);
    }
}