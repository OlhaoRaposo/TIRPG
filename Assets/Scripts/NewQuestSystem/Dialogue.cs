using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue 
{
        [Header("Status")]
        public int sentenceIndex;
        [Header("Dialogues")] 
        public List<string> sentences;
}
