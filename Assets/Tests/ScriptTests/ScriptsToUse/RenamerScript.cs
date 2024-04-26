using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RenamerScript : MonoBehaviour
{
    public string baseName = "Prefab";
    public GameObject[] prefabs;

    void Rename()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            GameObject obj = prefabs[i];
            string path = "Assets/" + baseName + " (" + (i + 1) + " )" + ".prefab";
            string newName = baseName + "(" + (i + 1) + " )";

            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(obj), newName);
            PrefabUtility.SaveAsPrefabAsset(obj, path);
        }
    }

    void Start()
    {
        Rename();
    }
}
