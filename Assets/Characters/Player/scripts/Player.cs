using UnityEngine;

namespace Characters.Player.scripts
{
    public class Player : MonoBehaviour
    {
        public Vector3 Forward { get; private set; }
        
        // Start is called before the first frame update
        private void Start()
        {
            var frontTransform = transform.Find("Front");
            Forward = (frontTransform.position - transform.position).normalized;
        }
    }
}
