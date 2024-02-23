using System;
using UnityEngine;

[Serializable]
public class Dialogue 
{
        [Header("Status")]
        public bool alreadySaid;
        [Header("Keys")]
        public string key;
        [Header("Dialogues")]
        public string dialogue;
        public AudioSource audio;
}
