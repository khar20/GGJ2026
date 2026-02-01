using UnityEngine;

public class ObstacleLogic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the thing hitting us is the Player
        if (other.CompareTag("Player"))
        {
            // Find the HealthManager on the player and tell it to take damage
            if (other.TryGetComponent<HealthManager>(out var hm))
            {
                hm.TakeObstacleDamage();
            }

            // Destroy the obstacle so we don't hit it twice
            Destroy(gameObject);
        }
    }
}