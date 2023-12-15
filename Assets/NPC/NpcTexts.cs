[System.Serializable]
public class NpcTexts
{
  public string npcCode;
  //public string[] text;
  public Texts[] text;
  public string questAlreadyGiven;
  public string questRewards;
}
[System.Serializable]
public class Texts
{
  public string text;
  public string audioCode;
}
