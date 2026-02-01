using UnityEngine;
using UnityEngine.UI; // For HUD interaction

public class HealthManager : MonoBehaviour
{
    public PlayerStats stats;
    public SceneLoader sceneManager;

    [Header("Current States (%)")]
    public float health = 100f;
    public float fatigue = 0f;
    public float damage = 0f;

    [Header("Wwise")]
    public AK.Wwise.Event hurtSound;

    void Update()
    {
        // 1. Calculations over time
        fatigue += stats.fatigueIncreaseRate * Time.deltaTime;
        health -= stats.healthDecreaseRate * Time.deltaTime;

        // 2. Clamp values so they don't go below 0 or above 100
        fatigue = Mathf.Clamp(fatigue, 0, 100);
        health = Mathf.Clamp(health, 0, 100);

        // 3. Logic: Total must be 100%. 
        // If Fatigue + Damage + Health > 100, Health must shrink.
        float totalBadStuff = fatigue + damage;
        if (health + totalBadStuff > 100f)
        {
            health = 100f - totalBadStuff;
        }

        // 4. Check for Death
        if (health <= 0)
        {
            Die();
        }
    }

    public bool isInvincible = false; // NEW
    public void TakeObstacleDamage()
    {
        if (isInvincible) return; // NEW: If dashing, ignore damage

        damage += stats.obstacleDamage;
        if (hurtSound.IsValid()) hurtSound.Post(gameObject);
        Debug.Log("Ouch! Damage is now: " + damage);
    }

    void Die()
    {
        if (health > 0) return; // Prevent double death

        Debug.Log("Player is Dead!");

        // Play Wwise Death Sound (Create 'Play_Death' event in Wwise first)
        // AK.Wwise.Event.Post("Play_Death", gameObject); 

        // Trigger UI
        if (sceneManager != null)
        {
            sceneManager.TriggerGameOver();
        }
        else
        {
            // Fallback if you forgot to link it
            Debug.LogError("SceneLoader not assigned in HealthManager!");
            Time.timeScale = 0;
        }
    }
}