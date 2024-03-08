using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Levelup Data", menuName = "Levelup Data")]
public class LevelUpData : ScriptableObject
{
    public List<int> xpPerLevel;
    public void AddLevelUpXp(int xp)
    {
        xpPerLevel.Add(xp);
    }
    public void Clear()
    {
        xpPerLevel.Clear();
    }
}
