using System;
using UnityEngine;

[Serializable]
public class Validation 
{
    public bool isCompleted;
    public enum TypeOfValidation {Collect, Talk, Kill, WalkToAPlace}
    public TypeOfValidation typeOfValidation;

    public int itensCollected;
    public int mobsKilled;
    public string npcTalked;
    public Collider placeToWalk;
}
