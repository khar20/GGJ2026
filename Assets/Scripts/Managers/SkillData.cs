using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Runner/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public float cooldown = 2f;
    public float punishmentWeight = 15f; // How much it fills the punishment bar
    public Sprite icon;
}