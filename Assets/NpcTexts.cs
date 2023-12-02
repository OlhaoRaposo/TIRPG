using System.Collections.Generic;

[System.Serializable]
public class NpcTexts
{
  public string npcCode;
  //public string[] text;
  public Texts[] text;

}
[System.Serializable]
public class Texts
{
  public string text;
  public string audioCode;
}
