using System;
using UnityEngine;

public class PlayerUseSkill : MonoBehaviour
{
    [SerializeField]
    private Skills[] skills;
    [SerializeField]
    private int currentSkill;
    [SerializeField]
    private float timeNextSkill;

    void Start()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].IsChosenPlayer == true)
            {
                currentSkill = i;
            }
        }
    }

    void Update()
    {
        if (timeNextSkill <= skills[currentSkill].coolDownTime)
        {
            timeNextSkill += Time.deltaTime;
        }
    }

    public void UseSkill()
    {
        if (timeNextSkill >= skills[currentSkill].coolDownTime)
        {
            timeNextSkill = 0;
            GameObject CurrentSkillObject = Instantiate(skills[currentSkill].SkillObject, skills[currentSkill].PointSpawnSkill.position, skills[currentSkill].PointSpawnSkill.rotation);
            Destroy(CurrentSkillObject, skills[currentSkill].liveTime);
        }
    }
}

[Serializable]
public class Skills
{
    public Transform PointSpawnSkill;
    public GameObject SkillObject;
    public float liveTime;
    public float coolDownTime;
    public bool IsChosenPlayer;
}
