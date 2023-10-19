using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Weapon : MonoBehaviour
{
   public int atualAmmo;
   public int maxAmmo;
   public float fireRate;
   private float minEnergyAttack1;
   public List<AttackBehaviour> attacks = new List<AttackBehaviour>();

   public void Attack(string AttackName)
   {
      switch (AttackName)
      {
         case  "NormalAttack" :
            foreach (var attack in attacks)
            {
               if(attack.attackType == AttackBehaviour.AttacksList.normalAttack)
                  NormalAttack(attack);
            }
            break;
         case "NormalAreaAttack":
            foreach (var attack in attacks)
            {
               if(attack.attackType == AttackBehaviour.AttacksList.normalAreaAttack)
                  NormalAreaAttack(attack);
            }
            break;
         case "EspecialAttack":
            foreach (var attack in attacks)
            {
               if(attack.attackType == AttackBehaviour.AttacksList.especialAttack)
                  EspecialAttack(attack);
            }
            break;
      }
   }
   private void NormalAttack(AttackBehaviour attackBehaviour)
   {
      Instantiate(attackBehaviour.attackPrefab, transform.position, transform.rotation);
      atualAmmo--;
   }
   private void NormalAreaAttack(AttackBehaviour attackBehaviour)
   {
      for (int i = 0; i < 3; i++)
      {
         Vector3 pos = Random.insideUnitSphere * 3;
         if (Physics.Raycast(pos, Vector3.down, out RaycastHit hitInfo)) {
            Instantiate(attackBehaviour.imageTarget, hitInfo.point + new Vector3(0,.001f,0), Quaternion.Euler(90,0,0));
         }
         Instantiate(attackBehaviour.attackPrefab, pos + new Vector3(0,50,0), transform.rotation);
      }
      atualAmmo = 0;
   }
   private void EspecialAttack(AttackBehaviour attackBehaviour)
   {
      Vector3 pos = GameObject.Find("Player").transform.position;
      if (Physics.Raycast(pos, Vector3.down, out RaycastHit hitInfo)) {
         Instantiate(attackBehaviour.imageTarget, hitInfo.point + new Vector3(0,.001f,0), Quaternion.Euler(90,0,0));
      }
      Instantiate(attackBehaviour.attackPrefab, transform.position, transform.rotation);
      atualAmmo = 0;
   }
}
[Serializable]
public class AttackBehaviour 
{
   public enum AttacksList {normalAttack, normalAreaAttack, especialAttack}
   public AttacksList attackType;
   public GameObject attackPrefab;
   public GameObject imageTarget;
   public float minEnergy;
}
