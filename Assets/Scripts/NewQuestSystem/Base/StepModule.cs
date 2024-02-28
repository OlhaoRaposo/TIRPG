using System;
using UnityEngine;

[Serializable]
public abstract class StepModule : Module
{    
    [HideInInspector] public QuestStepModule inspect;

    public virtual void SetActive(bool async){}
    
    public virtual void SetInactive(){}
    
    public virtual void OnValidate(){}
}

[Serializable]
public class QuestStepModule : ModularNoBehaviour<StepModule>
{
    public void SetActive(bool async)
    {
        foreach (StepModule module in this)
        {
            module.SetActive(async);
        }
    }
    
    public void SetInactive()
    {
        foreach (StepModule module in this)
        {
            module.SetInactive();
        }
    }
    
    public void OnValidate()
    {
        foreach (StepModule module in this)
        {
            module.OnValidate();
        }
    }
    
    public override void OnAddModule(StepModule module)
    {
        module.inspect = this;
    }

    public int GetProceduresCount => ModuleCount;
}