using System;
using Characters.scripts.Interfaces;
using UnityEngine;
using Utils;

namespace Characters.Npcs.scripts
{
    // Note: Npc implémente IPlayer car il imite un joueur dans le jeu
    public class Npc : MonoBehaviour, IKnockBack
    {
        public float moveSpeed = 0.5f;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player)) return;
            
            var playerPosition = other.gameObject.transform.position;
            var direction = (playerPosition - transform.position).normalized;
                
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        // TODO: revoir si on veut ça 
        public void GetPunched(Vector3 direction, float damage)
        {
            _rb.AddForce(direction, ForceMode.Impulse);
        }

        public void KnockBack(Vector3 direction, float force)
        {
            _rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        }
    }
}
