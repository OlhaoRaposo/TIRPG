using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public static SkillTree instance;

    [SerializeField] List<Skill> skills;

    private void Awake()
    {
        instance = this;
    }
    public Skill GetSkillByData(SkillData data)
    {
        foreach (Skill s in skills)
        {
            if (s.GetData() == data)
            {
                return s;
            }
        }

        return null;
    }

    //retorna as skills anteriores
    public List<Skill> GetPreviousSkillsByData(SkillData data)
    {
        List<Skill> skills = new List<Skill>();

        foreach (SkillData skillReq in data.skillsRequired)
        {
            Skill s = GetSkillByData(skillReq);

            if (s != null) skills.Add(s);
        }

        return skills;
    }
}
