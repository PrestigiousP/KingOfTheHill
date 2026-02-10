using UnityEngine;
using Utils;

namespace Characters.Npcs.scripts
{
    public class Npc : MonoBehaviour
    {
        public float moveSpeed = 0.5f;
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                var playerPosition = other.gameObject.transform.position;
                var direction = (playerPosition - transform.position).normalized;
                
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }
}
