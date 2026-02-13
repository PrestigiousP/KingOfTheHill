using UnityEngine;

namespace Characters.scripts.Interfaces
{
    public interface IKnockBack
    {
        public void KnockBack(Vector3 direction, float force);
    }
}