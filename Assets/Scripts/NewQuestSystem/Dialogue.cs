using UnityEngine;

public class Dialogue : MonoBehaviour
{
        [Header("Status")]
        public bool alreadySaid;
        public bool isUnlocked;
        [Header("Keys")]
        public string key;
        [Header("Dialogues")]
        public string dialogue;
        public AudioSource audio;
}
