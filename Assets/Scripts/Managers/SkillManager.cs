using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SkillManager : MonoBehaviour
{
    public SkillData[] availableSkills; // Ensure you have 3 ScriptableObjects assigned here
    public int currentSkillIndex = 0;

    [Header("Status")]
    public float punishmentLevel = 0;
    public bool skillsLocked = false;
    public float punishmentDrainRate = 5.0f; // Drops per second

    // References
    private PlayerController playerMove;
    private HealthManager playerHealth;

    void Start()
    {
        playerMove = GetComponent<PlayerController>();
        playerHealth = GetComponent<HealthManager>();
    }

    void Update()
    {
        // Drain Punishment bar over time
        if (punishmentLevel > 0)
        {
            punishmentLevel -= punishmentDrainRate * Time.deltaTime;
        }

        // Unlock if drained
        if (skillsLocked && punishmentLevel <= 0)
        {
            skillsLocked = false;
            Debug.Log("Skills Unlocked!");
        }

        // Clamp values
        punishmentLevel = Mathf.Clamp(punishmentLevel, 0, 100);
    }

    public void OnCycleSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentSkillIndex = (currentSkillIndex + 1) % availableSkills.Length;
            Debug.Log("Selected: " + availableSkills[currentSkillIndex].skillName);
        }
    }

    public void OnUseSkill(InputAction.CallbackContext context)
    {
        if (context.performed && !skillsLocked)
        {
            StartCoroutine(ExecuteSkill());
        }
    }

    IEnumerator ExecuteSkill()
    {
        SkillData skill = availableSkills[currentSkillIndex];

        // 1. Add Punishment
        punishmentLevel += skill.punishmentWeight;
        if (punishmentLevel >= 100)
        {
            skillsLocked = true;
            punishmentLevel = 100; // Cap it
        }

        // 2. Do the Skill
        switch (skill.skillName)
        {
            case "Lightning":
                // Dash: Invincible for 1 second
                Debug.Log("Skill: Lightning Dash!");
                playerHealth.isInvincible = true;
                yield return new WaitForSeconds(1.0f);
                playerHealth.isInvincible = false;
                break;

            case "Jump":
                // Physics Jump
                Debug.Log("Skill: Jump!");
                playerMove.TriggerJump();
                break;

            case "Recover":
                // Heal Fatigue
                Debug.Log("Skill: Recover!");
                playerHealth.fatigue -= 20;
                // Stop movement briefly?
                float originalSpeed = playerMove.forwardSpeed;
                playerMove.forwardSpeed = 0;
                yield return new WaitForSeconds(0.5f);
                playerMove.forwardSpeed = originalSpeed;
                break;
        }
    }
}