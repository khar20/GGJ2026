using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Runner/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float fatigueIncreaseRate = 0.5f; // Fatigue per second
    public float healthDecreaseRate = 0.2f;  // Health loss per second (hunger/thirst)
    public float obstacleDamage = 15.0f;    // Damage % from hitting a cube
}