using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    public HealthManager playerHealth;
    public SkillManager playerSkills; // NEW REFERENCE

    [Header("Health Bars")]
    public Image healthBar;
    public Image fatigueBar;
    public Image damageBar;

    [Header("Skill UI")]
    public Image punishmentBar;
    public Image skillIcon;
    public TextMeshProUGUI skillNameText;
    public Color lockedColor = Color.red;
    public Color normalColor = Color.white;

    void Update()
    {
        // 1. Health Math (Existing)
        damageBar.fillAmount = playerHealth.damage / 100f;
        fatigueBar.fillAmount = (playerHealth.damage + playerHealth.fatigue) / 100f;
        healthBar.fillAmount = (playerHealth.damage + playerHealth.fatigue + playerHealth.health) / 100f;

        // 2. Punishment Bar
        punishmentBar.fillAmount = playerSkills.punishmentLevel / 100f;

        // Change color if locked
        if (playerSkills.skillsLocked) punishmentBar.color = lockedColor;
        else punishmentBar.color = normalColor;

        // 3. Current Skill Display
        SkillData current = playerSkills.availableSkills[playerSkills.currentSkillIndex];
        skillNameText.text = current.skillName;
        // If you have icons: skillIcon.sprite = current.icon;
    }
}