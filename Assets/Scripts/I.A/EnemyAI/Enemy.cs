using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{ IState state;
  public enum  EnemyState { Patrol, Attack,Chase,Dead,None }
 [SerializeField] private float life;
 [SerializeField] private float energy;
 [SerializeField] public EnemyState currentState;
 public GameObject target;
 public NavMeshAgent agent ;
 public Animator enemyAnimator;
 public UnityEvent OnStart;
 private Collider[] targetsDetected;
 [Header("EnemyType")]
 public EnemyType myType;
 public enum EnemyType { ranged, melee, rangedAndMelee }
 [Header("Data")]
 public Data data;
 [Header("Triggers")]
 public Triggers triggers;
 public Weapon weapon;
 private Vector2 smoothVelocity;
 private Vector2 velocity;
 [HideInInspector]public bool isAtacking;
 private void Awake() {
   enemyAnimator.applyRootMotion = true;
   agent.updatePosition = false;
   agent.updateRotation = true;
 }
 private void Start() {
   if(!TryGetComponent(out NavMeshAgent ag)){
     Debug.LogWarning("You Forgot to add a NavMeshAgent to the enemy, add one now."); }

   if (!TryGetComponent(out Animator an))
     Debug.LogWarning("You Forgot to set the Animator THIS ENEMY NO LONGER WILL WORK");
   
   life = data.maxLife >= 0 ? data.maxLife : 300;
   energy = data.maxEnergy;
   weapon.SetUser(this);
   ChangeState(new PatrolState(this));
   OnStart.Invoke();
 }
 private void OnAnimatorMove() {
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
   state?.Update();
 }
 private void Update() {
   SyncronizeMovement();
   DetectTarget();
 }
 private void DetectTarget() {
   targetsDetected = Physics.OverlapSphere(transform.position, 10);
   foreach (var detections in targetsDetected) {
     if (detections.gameObject.CompareTag("Player"))
       target = detections.gameObject;
   }
   if (target != null){
     if(currentState != EnemyState.Chase)
        ChangeState(new ChaseState(this));
   }else {
     if(currentState != EnemyState.Patrol)
       ChangeState(new PatrolState(this));
   }
 }
 public float TargetDistance() {
   Vector3 distance = target.transform.position - transform.position;
   return distance.magnitude;
 }
 public void  ChangeState(IState state) {
    this.state = state;
    this.state.Enter();
  }
}
[Serializable]
  public class Data {
    public float maxLife;
    public float maxEnergy;
    [Header("Bestiary")]
    public Bestiary mySpawner;
}

[Serializable]
public class Triggers {
  public List<string> meleeAttacks;
  public List<string> rangedAttacks;
  public List<string> specialAttacks;
  public List<string> jumpAttacks;
}