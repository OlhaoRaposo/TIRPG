using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class JsonDatabase
{
    public List<NpcTexts> npcs = new List<NpcTexts>();
}

public class NPCSerializable : MonoBehaviour
{
    private string jsonPath;
    public NpcTexts[] textToSerialize;
    public string npcToSearch;
    private void Awake() {
        jsonPath = Application.dataPath + "/NPCJsonDatabase.json";
    }
    void Start() {
        Read();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
           SearchOnDatabase();
        }
    }
    void SearchOnDatabase()
    {
        JsonDatabase data = new JsonDatabase();
        if (File.Exists(jsonPath))
        {
            string s = File.ReadAllText(jsonPath);
            data = JsonUtility.FromJson<JsonDatabase>(s);
            foreach (var obj in data.npcs) {
                if(obj.npcCode == npcToSearch) {
                    foreach (var fala in obj.text[0].text) {
                        Debug.Log(fala);
                    }
                }
            }
        }
    }
    void Read()
    {
        JsonDatabase data = new JsonDatabase();
        if (File.Exists(jsonPath))
        {
            bool contain = false;
            string s = File.ReadAllText(jsonPath);
            data = JsonUtility.FromJson<JsonDatabase>(s);

            foreach (var obj in data.npcs) {
                foreach (var variable in textToSerialize)
                {
                    if(obj.npcCode == variable.npcCode) {
                        contain = true;
                    }
                    else {
                        contain = false;
                    }
                }
            }
            if(contain == false) {
                foreach (var variable in textToSerialize) {
                    data.npcs.Add(variable);
                }
                InsertOnDatabase(data);
            }
        }else {
            foreach (var variable in textToSerialize) {
                data.npcs.Add(variable);
            }
            InsertOnDatabase(data);
        }
    }
    void InsertOnDatabase(JsonDatabase dataAfterInsert)
    {
       string content = JsonUtility.ToJson(dataAfterInsert, true);
       File.WriteAllText(jsonPath, content);
    }
}