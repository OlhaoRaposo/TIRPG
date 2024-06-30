using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public static SkillTree instance;

    SkillTreeDragMove dragClass;

    [SerializeField] List<Skill> skills;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        dragClass = GetComponent<SkillTreeDragMove>();
    }
    public void ResetAllSkillVisuals()
    {
        foreach (Skill s in skills)
        {
            s.SetupSkillVisuals();
        }
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
    public void ForceAcquireSkill(SkillData data)
    {
        GetSkillByData(data).ForceAcquireSkill();
    }
    public SkillTreeDragMove GetDragClass()
    {
        return dragClass;
    }
}
