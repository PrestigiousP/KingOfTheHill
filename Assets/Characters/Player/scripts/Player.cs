using UnityEngine;

namespace Characters.Player.scripts
{
    public class Player : MonoBehaviour
    {
        private Transform _directionTransform;
        
        // Start is called before the first frame update
        void Start()
        {    
            _directionTransform = transform.Find("direction");
        
            if (_directionTransform == null)
            {
                Debug.LogError("Direction object not found!");
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
