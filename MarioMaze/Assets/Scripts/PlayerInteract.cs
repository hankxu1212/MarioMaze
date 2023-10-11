    using UnityEngine;

    [RequireComponent(typeof(Collider))]
    public class PlayerInteract : MonoBehaviour
    {
        private int coins = 0;
        
        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Collided");
            if (other.transform.tag == "Coin")
            {
                coins++;
                Debug.Log(coins);
                Destroy(other.gameObject);
            }
        }
    }