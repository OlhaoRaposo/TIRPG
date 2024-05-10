using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{ IState state;
  public enum  EnemyState { Patrol, Chase, Attack, Dead,None }
 [SerializeField] private float life;
 [SerializeField] private float energy;
 [SerializeField] public EnemyState currentState;
 public GameObject target;
 public NavMeshAgent agent ;
 public Animator enemyAnimator;
 
 
 private Collider[] targetsDetected;
 [Header("EnemyType")]
 public EnemyType myType;
 public enum EnemyType { ranged, melee, rangedAndMelee }
 [Header("Data")]
 public Data data;
 
 private Vector2 smoothVelocity;
 private Vector2 velocity;

 private void Awake() {
   enemyAnimator.applyRootMotion = true;
   agent.updatePosition = false;
   agent.updateRotation = true;
 }
 private void Start() {
   if(!TryGetComponent(out NavMeshAgent ag)){
     Debug.LogWarning("You Forgot to add a NavMeshAgent to the enemy, adding one now."); }

   if (!TryGetComponent(out Animator an))
     Debug.LogWarning("You Forgot to set the Animator THIS ENEMY NO LONGER WILL WORK");
   
   life = data.maxLife >= 0 ? data.maxLife : 300;
   energy = data.maxEnergy;
   ChangeState(new PatrolState(this));
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