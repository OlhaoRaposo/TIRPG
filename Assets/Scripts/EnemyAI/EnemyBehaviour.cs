using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBehaviour : MonoBehaviour
{
  private IState state;
  public EnemyData enemyData;
  [SerializeField]
  private float energy;
  public Weapon myWeapon;
  public bool isPatroling;
  public Vector3 patrolDestination;
  public Vector3[] Test;
  [Space]
  public GameObject target;
  public bool isAttacking;
  [Space]
  [SerializeField]
  private Collider[] targetsDettectes;

  public Canvas alert;
  public NavMeshAgent enemyAgent { get; private set; }
  public virtual void Start()
  {
    enemyAgent = this.GetComponent<NavMeshAgent>();
    isPatroling = true;
    myWeapon.gun.GetComponent<RobotRangedWeapon>().enemy = this;
    BackPatrol();
  } 
  public virtual void FixedUpdate()
  {
    state?.Update();
    DettectPlayerOrAudioNearby();
  }
  public void SetState(IState state)
  {
    state?.Exit();
    this.state = state;
    state?.Enter();
  }
  public void StartAttacking()
  {
    isAttacking = true;
    isPatroling = false;
    alert.enabled = true;
    enemyAgent.speed = 8;
    StartCoroutine(LoadEnergy());
    StartCoroutine(FadeAlert());
    StartCoroutine(Attack());
    SetState(new WalkToTarget(this));
  }

  public void BackPatrol()
  {
    isAttacking = false;
    isPatroling = true;
    enemyAgent.speed = 3;
    energy = 0;
    patrolDestination = new Vector3();
    alert.enabled = false;
    StartCoroutine(Patrol());
    StopCoroutine(LoadEnergy());
  }
  //Dettection
  #region Detection
 void DettectPlayerOrAudioNearby()
 {
   targetsDettectes = Physics.OverlapSphere(transform.position, enemyData.rangeDetection,3);
   GameObject target = null;
   foreach (Collider var in targetsDettectes) {
     if (var.gameObject.CompareTag("Player")) {
       target = var.gameObject;
       break;
     }
   }
   this.target = target;
   
   if (this.target != null)
   {
     transform.LookAt(this.target.transform);
     if(!isAttacking)
      StartAttacking();
   }else {
     if(!isPatroling)
      BackPatrol();
   }
 }
 #endregion

 public IEnumerator Attack()
 {
   if (myWeapon.ammo <= 0) {
     SetState(new Reposition(this));
     yield return new WaitForSeconds(myWeapon.reloadingTime);
     myWeapon.ammo = myWeapon.maxAmmo;
   }else {
     yield return new WaitForSeconds(myWeapon.fireRate);
     if (energy < 35) {
       SetState(new RangedAttack(this));
     }else if(energy > 35 &&  energy < 85)  {
       energy -= 35;
       SetState(new RangedAreaAttack(this));
     }
   }
   myWeapon.ammo--;
   if (isAttacking)
     StartCoroutine(Attack());
 }
 public IEnumerator Patrol()
 {
   yield return new WaitForSeconds(1);
   SetState(new Patrol(this));
   if(!isAttacking)
    StartCoroutine(Patrol());
 }
  public IEnumerator LoadEnergy()
 {
   energy++;
   yield return new WaitForSeconds(1);
   if (energy < 100)
     StartCoroutine(LoadEnergy());
 }
  public IEnumerator FadeAlert()
  {
    yield return new WaitForSeconds(.5f);
    alert.enabled = false;
    yield return new WaitForSeconds(.5f);
    alert.enabled = true;
    yield return new WaitForSeconds(.5f);
    alert.enabled = false;
    yield return new WaitForSeconds(.5f);
    alert.enabled = true;
    yield return new WaitForSeconds(.5f);
    alert.enabled = false;
  }
}

[Serializable]
public class Weapon
{
  public GameObject gun;
  public int maxAmmo;
  public int ammo;
  public GameObject ammoPrefab;
  public GameObject areaAmmoPrefab;
  public GameObject imageTarget;
  public float fireRate;
  public float reloadingTime;
}
