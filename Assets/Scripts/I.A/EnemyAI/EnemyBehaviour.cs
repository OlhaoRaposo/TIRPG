using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{ IState state;
  public enum  EnemyState { Patrol, Attack,Chase,Dead,None }
 [SerializeField] private float life;
 [SerializeField] public EnemyState currentState;
 public GameObject target;
 public Transform startePoint;
 public NavMeshAgent agent ;
 public Animator enemyAnimator;
 public UnityEvent OnStart;
 public UnityAction OnTargeted;
 private Collider[] targetsDetected;
 [Header("Data")]
 public Data data;
 public GameObject arena;
 [Header("Drops&Quests")]
 public ItemDropInfo[] dropInfo;
 public QuestType.EnemyTypes questType;
 [Header("Canvas")]
 public GameObject EnemyCanvas;
 public string bossName;
 public TextMeshProUGUI enemyName;
 public GameObject damageCanvas;
 [Header("Triggers")]
 public List<Attacks> attacks;
 public List<string> attacksAvailable = new List<string>();
 public string triggerCode;
 public GameObject weapon;
 private Vector2 smoothVelocity;
 private Vector2 velocity;
 [HideInInspector]public bool isAtacking;
 public bool isDummy;
 public bool isDefeted;
 [Header("Beastiary")]
 public Bestiary mySpawner;
 private void Awake() {
   if(isDummy)
     return;
   
   enemyAnimator.applyRootMotion = true;
   agent.updatePosition = false;
   agent.updateRotation = true;
 }
 private void Start() {
   if(isDefeted)
     gameObject.SetActive(false);
   
   startePoint = transform;
   if(!TryGetComponent(out NavMeshAgent ag)){
     Debug.LogWarning("You Forgot to set the NavMeshAgent to the enemy THIS ENEMY NO LONGER WILL WORK."); }
   if (!TryGetComponent(out Animator an))
     Debug.LogWarning("You Forgot to set the Animator THIS ENEMY NO LONGER WILL WORK.");
   
   life = data.maxLife >= 0 ? data.maxLife : 300;
   if(isDummy) 
     return;
   enemyName.text = bossName;
   ChangeState(new PatrolState(this));
   OnStart.Invoke();
 }
 private void OnAnimatorMove() {
   if(isDummy) 
     return;
   Vector3 rootPosition = enemyAnimator.rootPosition;
   rootPosition.y = agent.nextPosition.y;
   transform.position = rootPosition;
   agent.nextPosition = rootPosition;
   enemyAnimator.rootPosition = rootPosition;
   transform.position = agent.nextPosition;
   enemyAnimator.rootPosition = agent.nextPosition;
 }
 private void SyncronizeMovement()
 {
   Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
   worldDeltaPosition.y = 0;
   float dx = Vector3.Dot( transform.right, worldDeltaPosition);
   float dy = Vector3.Dot( transform.forward, worldDeltaPosition);
   Vector2 deltaPosition = new Vector2(dx, dy);

   float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
   smoothVelocity = Vector2.Lerp(smoothVelocity, deltaPosition, smooth);
   velocity = smoothVelocity / Time.deltaTime;
   if (agent.remainingDistance <= agent.stoppingDistance)
   {
     velocity = Vector2.Lerp(Vector2.zero, velocity,
       agent.remainingDistance / agent.stoppingDistance);
   }
   bool canMove = velocity.magnitude > .2f
                  && agent.remainingDistance > agent.stoppingDistance;

   enemyAnimator.SetBool("isWalking", canMove);
   enemyAnimator.SetFloat("velx", velocity.x);
   enemyAnimator.SetFloat("vely", velocity.y);
   float deltaMagnetude = worldDeltaPosition.magnitude;
   if(deltaMagnetude > agent.radius / 2f) {
    transform.position = Vector3.Lerp(enemyAnimator.rootPosition, agent.nextPosition, smooth);
   }
 } 
 private void FixedUpdate() {
   if(isDummy) 
     return;
   
   state?.Update();
 }
 private void Update() {
   if(isDummy) 
     return;
   SyncronizeMovement();
   DetectTarget();
 }

 private void OnTriggerEnter(Collider col) {
   if (col.gameObject.CompareTag("Player")) {
     PlayerHPController.instance.ChangeHP(10,true);
     Debug.Log("HIT");
   }
 }

 private void DetectTarget() {
   targetsDetected = Physics.OverlapSphere(transform.position, 10);
   foreach (var detections in targetsDetected) {
     if (detections.gameObject.CompareTag("Player"))
       target = detections.gameObject;
   }
   EnemyCanvas.gameObject.SetActive(target != null);

   if (target != null)
   {
     if (TargetDistance() >= 20) {
       target = null;
       ChangeState(new PatrolState(this));
       arena.SetActive(false);     
     }
   }
   if (target != null){
     if (currentState != EnemyState.Chase) {
       ChangeState(new ChaseState(this));
       arena.SetActive(true);       
     }
   }else {
     if(currentState != EnemyState.Patrol)
       ChangeState(new PatrolState(this));
   }
 }
 public void Die()
 {
  /* if(TryGetComponent(out KillDetection kd)) {
     kd.onKill.Invoke();
     if(mySpawner.spawner.TryGetComponent(out Spawner sp)) {
       sp.hasQuestOnCourse = true;
       sp.onKill = this.GetComponent<KillDetection>().onKill;
     }
   
   }*/
    WorldController.worldController.bossesDefeated.Add(gameObject.name);
   Debug.Log("KilledEn" + gameObject.name);
   Destroy(this.gameObject);
   ItemDropManager.instance.DropItem(dropInfo, transform.position);
   PlayerStats.instance.GainXp(Random.Range(30,100));
 }
 public void TakeDamage(float damage, DamageElementManager.DamageElement damageElement)
 {
   float actualDamage;
   actualDamage = 1;
   if (target == null) {
     target = GameObject.FindGameObjectWithTag("Player");
   }
   Image lifeBar = EnemyCanvas.transform.Find("LIFE").transform.Find("LifeBarFill").GetComponent<Image>();
   life -= Mathf.Round((actualDamage * damage));
   lifeBar.fillAmount = life / data.maxLife;
   DamageElementManager.instance.ApplyDamageEffect(this, damageElement, Mathf.Round((actualDamage * damage)));
   if (life <= 0) {
     if (enemyAnimator.GetBool("isAlive")) {
       enemyAnimator.SetBool("isAlive",false);
       enemyAnimator.SetTrigger("Die");
      
     }
   }
 }
 public float TargetDistance() {
   Vector3 distance = target.transform.position - transform.position;
   return distance.magnitude;
 }
 public void InstantiateText(float totalDamage, Color textColor) {
   GameObject damageObj = Instantiate(damageCanvas, transform.position + new Vector3(0, 2.8f,0) , transform.rotation, transform);
   Text damageTxt = damageObj.transform.GetChild(0).GetComponent<Text>();
   damageTxt.text = Mathf.Round((totalDamage)).ToString();
   damageTxt.color = textColor;
 }
 public void InflictDirectDamage(float damage) {
   float actualDamage;
   actualDamage = 1;
   Image lifeBar = EnemyCanvas.transform.Find("LIFE").transform.Find("LifeBarFill").GetComponent<Image>();
   life -= Mathf.Round((actualDamage * damage));
   lifeBar.fillAmount = life / data.maxLife;
   
   if (life <= 0) {
       if (enemyAnimator.GetBool("isAlive")) {
         enemyAnimator.SetBool("isAlive",false);
         enemyAnimator.SetTrigger("Die");
       }
   }
 }
 public void  ChangeState(IState state) {
    this.state = state;
    this.state.Enter();
 }
}
[Serializable]
  public class Data {
    public float maxLife;
}
[Serializable]
public class Triggers {
  public List<string> meleeAttacks;
  public List<string> rangedAttacks;
  public List<string> specialAttacks;
  public List<string> jumpAttacks;
}
[Serializable]
public class Attacks
{
  [Header("Code")]
  public string attackCode;
  [Header("Range")]
  public float minRange;
  public float maxRange;
  [Header("Damage")]
  public float damage;
  public DamageElementManager.DamageElement damageElement;

}