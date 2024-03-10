using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("EnemyStats")]
    public float life;
    public float lifeMax;
    public float energy;
    public GameObject EnemyCanvas;
    public GameObject damageCanvas;
    public enum EnemyType { ranged, melee, rangedAndMelee }
    [Header("EnemyType")]
    public EnemyType myType;
    [Range(0,100)]
    public float hostility;
    [Header("Bestiary")]
    public Bestiary mySpawner;
    [Header("Drops&Quests")]
    public ItemDropInfo[] dropInfo;
    public QuestType.EnemyTypes questType;
    [Header("Movement")]
    public EnemyNavMeshAgent myNavMeshAgent;
    private Vector3 patrolDestination;
    private Vector2 velocity;
    private Vector2 smoothVelocity;
    private bool hasArrived;
    [SerializeField]
    public bool isPatroling, isChasing, isAttacking,isIddle;
    [Header("Detection System")]
    private Collider[] targetsDetected;   
    [Range(1,20)]
    public float rangeDetection;
    public GameObject enemyTarget;
    [Header("Weapon")]
    public GameObject myWeapon;
    
    private void GetAllMyReferences() {
        lifeMax = life;
        myNavMeshAgent.enemyAgent = TryGetComponent(out NavMeshAgent agent) ? agent : null;
        myNavMeshAgent.enemyAnimator = TryGetComponent(out Animator animator) ? animator : null;
        myNavMeshAgent.enemyAnimator.applyRootMotion = true;
        myNavMeshAgent.enemyAgent.updatePosition = false;
        myNavMeshAgent.enemyAgent.updateRotation = true;
    }
    void Start() {
        CreateSpawner();
        GetAllMyReferences();
        StartCoroutine(StartPatrolling());
        StartCoroutine(CheckLocomotion());
    }
    void Update()
    {
        DettectPatrolDistance();
        DettectPlayerOrAudioNearby();
        SyncronizeMovement();
    }
    public void TakeDamage(float damage)
    {
        float actualDamage;
        actualDamage = 1;
        Image lifeBar = EnemyCanvas.transform.Find("LifeBar").transform.Find("LifeBarFill").GetComponent<Image>();
        life -= Mathf.Round((actualDamage * damage));
        lifeBar.fillAmount = life / lifeMax;
        GameObject damageObj = Instantiate(damageCanvas, transform.position + new Vector3(-2, 2.8f,0) , transform.rotation, transform);
        damageObj.transform.GetChild(0).GetComponent<Text>().text = Mathf.Round((actualDamage * damage)).ToString();

        if (enemyTarget == null) {
            if (!isAttacking) {
                enemyTarget = GameObject.FindGameObjectWithTag("Player");
                isAttacking = true;
                Attack();
            }
        }

        if (life <= 0)
        {
            if (myNavMeshAgent.enemyAnimator.GetBool("isAlive"))
            {
                myNavMeshAgent.enemyAnimator.SetBool("isAlive",false);
                myNavMeshAgent.enemyAnimator.SetTrigger("Die");
            }
        }
    }
    private void SyncronizeMovement()
    {
        Vector3 worldDeltaPosition = myNavMeshAgent.enemyAgent.nextPosition - transform.position;
        worldDeltaPosition.y = 0;
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothVelocity = Vector2.Lerp(smoothVelocity, deltaPosition, smooth);
        velocity = smoothVelocity / Time.deltaTime;
        if (myNavMeshAgent.enemyAgent.remainingDistance <= myNavMeshAgent.enemyAgent.stoppingDistance)
        {
            velocity = Vector2.Lerp(Vector2.zero, velocity,
                myNavMeshAgent.enemyAgent.remainingDistance / myNavMeshAgent.enemyAgent.stoppingDistance);
        }

        bool canMove = velocity.magnitude > .5f
                       && myNavMeshAgent.enemyAgent.remainingDistance > myNavMeshAgent.enemyAgent.stoppingDistance;

        myNavMeshAgent.enemyAnimator.SetBool("isWalking", canMove);
        myNavMeshAgent.enemyAnimator.SetFloat("velx", velocity.x);
        myNavMeshAgent.enemyAnimator.SetFloat("vely", velocity.y);
        float deltaMagnetude = worldDeltaPosition.magnitude;
        if(deltaMagnetude > myNavMeshAgent.enemyAgent.radius / 2f) {
            transform.position = Vector3.Lerp(myNavMeshAgent.enemyAnimator.rootPosition, myNavMeshAgent.enemyAgent.nextPosition, smooth);
        }
    } 
    private void OnAnimatorMove()
    {
        Vector3 rootPosition = myNavMeshAgent.enemyAnimator.rootPosition;
        rootPosition.y = myNavMeshAgent.enemyAgent.nextPosition.y;
        transform.position = rootPosition;
        myNavMeshAgent.enemyAgent.nextPosition = rootPosition;
        myNavMeshAgent.enemyAnimator.rootPosition = rootPosition;
        transform.position = myNavMeshAgent.enemyAgent.nextPosition;
        myNavMeshAgent.enemyAnimator.rootPosition = myNavMeshAgent.enemyAgent.nextPosition;
    }
    private void Attack()
    {
        isAttacking = true;
        isPatroling = false;
        isIddle = false;
        StopCoroutine(StartPatrolling());
        myNavMeshAgent.enemyAgent.SetDestination(enemyTarget.transform.position);
        myNavMeshAgent.enemyAnimator.SetBool("isAttacking",true);
        myWeapon.SendMessage("Attack", energy);
    }
    private void DettectPlayerOrAudioNearby()
    {
        targetsDetected = Physics.OverlapSphere(transform.position, rangeDetection);
        foreach (var detections in targetsDetected) {
            if (detections.gameObject.CompareTag("Player"))
                enemyTarget = detections.gameObject;
        }

        if (isAttacking)
            myNavMeshAgent.enemyAgent.SetDestination(enemyTarget.transform.position);
        
        if (enemyTarget != null) {
            if (enemyTarget.CompareTag("Player")) {
                if (!isAttacking) { Attack(); }
            }
        }
    }
    IEnumerator StartPatrolling()
    {
        isIddle = false;
        isPatroling = true;
        NavMeshHit hit;
        if (isPatroling) {
            patrolDestination = transform.position + (Random.insideUnitSphere * 30) + new Vector3(0,300,0);
            if (Physics.Raycast(patrolDestination, Vector3.down, out RaycastHit hitInfo)) {
                if (NavMesh.SamplePosition(hitInfo.point, out hit, .5f, NavMesh.AllAreas))
                {
                    patrolDestination = hit.position + new Vector3( 0,0.5f,0);
                    myNavMeshAgent.enemyAgent.SetDestination(patrolDestination);
                    hasArrived = false;
                    myNavMeshAgent.enemyAnimator.SetBool("isWalking",true);
                    myNavMeshAgent.enemyAnimator.SetBool("isAttacking",false);
                }
            }
        }
        yield return new WaitUntil(() => hasArrived);
        int odds = Random.Range(0, 100);
        if (odds <= 25) {
            StartCoroutine(Iddle());
        }else
            StartCoroutine(StartPatrolling());
    }
     IEnumerator Iddle()
     {
         isIddle = true;
         myNavMeshAgent.enemyAnimator.SetBool("isWalking",false);
         myNavMeshAgent.enemyAnimator.SetBool("isAttacking",false);
         yield return new WaitForSeconds(Random.Range(3, 10));
            StartCoroutine(StartPatrolling());
     }
     IEnumerator CheckLocomotion()
     {
         Vector3 pos0 = transform.position;
         yield return new WaitForSeconds(5);
         Vector3 pos1 = transform.position;
            if (pos0 == pos1) {
                StartCoroutine(StartPatrolling());
            }else {
                StartCoroutine(CheckLocomotion());
            }
     }
     private void DettectPatrolDistance()
     {
         Vector3 distance = transform.position - patrolDestination;
         if (distance.magnitude <= 3) {
             hasArrived = true;
         }
     }
    private void CreateSpawner() {
        GameObject spawn = Instantiate(mySpawner.spawner, transform.position, Quaternion.identity);
        spawn.GetComponent<Spawner>().prefabCode = mySpawner.bestiaryCode;
        mySpawner.spawner = spawn;
    }
    public void Die()
    {
        if(TryGetComponent(out KillDetection kd)) {
            kd.onKill.Invoke();
           if(mySpawner.spawner.TryGetComponent(out Spawner sp)) {
               sp.hasQuestOnCourse = true;
               sp.onKill = this.GetComponent<KillDetection>().onKill;
           }
            Debug.Log("KilledEn");
        }
        Destroy(this.gameObject);
        ItemDropManager.instance.DropItem(dropInfo, transform.position);
        PlayerStats.instance.GainXp(Random.Range(30,100));
        if (mySpawner.spawner.TryGetComponent(out Spawner spawn)) {
            spawn.GetComponent<Spawner>().StartRespawnProcess();
        }
    }
    
}
[Serializable]
public class EnemyNavMeshAgent
{
    public NavMeshAgent enemyAgent;
    public Animator enemyAnimator;
}
[Serializable]
public class Bestiary
{
    [Header("Bestiary")] 
    [SerializeField]
    public string bestiaryCode;
    public GameObject spawner;
}