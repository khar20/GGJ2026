using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Player References")]
    public HealthManager playerHealth;
    public SkillManager playerSkills;

    [Header("Compound Health Bar")]
    public Image healthBar;   // Green
    public Image fatigueBar;  // Yellow
    public Image damageBar;   // Red

    [Header("Skill UI")]
    public Image punishmentBar; // Purple
    public TextMeshProUGUI skillNameText;
    public Image skillIconBorder; // Optional: change color if locked

    void Update()
    {
        UpdateHealthBar();
        UpdateSkillUI();
    }

    void UpdateHealthBar()
    {
        // 1. Calculate values (0.0 to 1.0)
        float damagePercent = playerHealth.damage / 100f;
        float fatiguePercent = playerHealth.fatigue / 100f;
        float healthPercent = playerHealth.health / 100f;

        // 2. Stack the bars visually

        // RED: Only shows the damage percentage
        // Example: 10% damage = 0.1 fill
        damageBar.fillAmount = damagePercent;

        // YELLOW: Shows Damage + Fatigue
        // Example: 10% Dmg + 10% Fat = 0.2 fill. 
        // Since Red is on top, you only see the second 10% as Yellow.
        fatigueBar.fillAmount = damagePercent + fatiguePercent;

        // GREEN: Shows Damage + Fatigue + Health
        // Example: 10% D + 10% F + 80% H = 1.0 fill (Full bar).
        // Since Yellow and Red are on top, you see the remaining 80% as Green.
        healthBar.fillAmount = damagePercent + fatiguePercent + healthPercent;
    }

    void UpdateSkillUI()
    {
        if (playerSkills == null) return;

        // Update Punishment Bar Width
        punishmentBar.fillAmount = playerSkills.punishmentLevel / 100f;

        // Update Text
        SkillData current = playerSkills.availableSkills[playerSkills.currentSkillIndex];
        skillNameText.text = current.skillName;

        // Visual Feedback for Locked State
        if (playerSkills.skillsLocked)
        {
            punishmentBar.color = Color.red; // Bar turns red when full/locked
            skillNameText.color = Color.grey;
        }
        else
        {
            punishmentBar.color = new Color(0.6f, 0f, 0.8f); // Purple
            skillNameText.color = Color.white;
        }
    }
}