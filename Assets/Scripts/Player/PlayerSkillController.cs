using System;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [SerializeField]
    private Skills[] Skills;
    [SerializeField]
    private int CurrentSkill;

    void Start()
    {
        for (int i = 0; i < Skills.Length; i++)
        {
            if (Skills[i].IsChosenPlayer == true)
            {
                CurrentSkill = i;
            }
        }
    }

    void Update()
    {
        UseSkill();
    }

    void UseSkill()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject CurrentSkillObject = Instantiate(Skills[CurrentSkill].SkillObject, Skills[CurrentSkill].PointSpawnSkill.position, Skills[CurrentSkill].PointSpawnSkill.rotation);
        }
    }
}

[Serializable]
public class Skills
{
    public Transform PointSpawnSkill;
    public GameObject SkillObject;
    public bool IsChosenPlayer;
}
