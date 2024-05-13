using UnityEngine;

public class ChangeLog : MonoBehaviour
{
    [SerializeField]
    private GameObject[] version;
    public void OpenLog(int aux){
        foreach (var obj in version) {
            obj.SetActive(false);
        }
        version[aux].SetActive(true);
    }
    public void CloseAll() {
        foreach (var obj in version) {
            obj.SetActive(false);
        }
    }
}
