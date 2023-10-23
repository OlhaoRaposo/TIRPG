using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[CustomEditor(typeof(EnemyScript))]

public class EnemyScript : MonoBehaviour
{
    public enum EnemyBehaviour { ranged, melee, rangedAndMelee }
    public EnemyBehaviour myBehaviour;
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isPatrolling;
    [SerializeField] private bool isInStandBy;
    [SerializeField] private bool hasArrived;
    [SerializeField] private Vector3 patrolDestination;
    [Space]
    [Header("CanvasReference")]
    [SerializeField] public Canvas alert;
    public Canvas lifeCanvas;
    public GameObject damageCanvas;
    public Image lifeBar;
    [Header("WeaponReferences")]
    public Weapon myWeapon;
    private bool waitingForSpecial;
    [Header("EnemyReferences")] 
    public QuestType.EnemyTypes questType;
    public ItemDropInfo[] dropInfo;
    public GameObject enemyTarget;
    public float life;
    public float energy;
    public float rangeDetection;
    public Vector3 distanceFromPlayer;
    public NavMeshAgent enemyAgent;
    public Animator enemyAnimator;
    [SerializeField] private List<Collider> targetsDettectes = new List<Collider>();
    
    private void Start()
    {
        GetEnemyReferences();
        StartCoroutine(Patrol());
        
    }
    private void Update()
    {
        DettectPlayerOrAudioNearby();
        DettectPatrolDistance();
        RotationController();
    }
    private void RotationController()
    {
        if (enemyAgent.remainingDistance > 0.1f)
        {
            Vector3 nextPosition = enemyAgent.path.corners[1];
            Quaternion targetRotation = Quaternion.LookRotation(nextPosition - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
    //PatrolRegion
    #region PatrolRegion
    IEnumerator Patrol()
    {
        NavMeshHit hit;
        if (isPatrolling) {
            patrolDestination = transform.position + (Random.insideUnitSphere * (rangeDetection * 2) + new Vector3(0,300,0));
            if (Physics.Raycast(patrolDestination, Vector3.down, out RaycastHit hitInfo)) {
                if (NavMesh.SamplePosition(hitInfo.point, out hit, .5f, NavMesh.AllAreas)) {
                    patrolDestination = hit.position;
                    enemyAgent.SetDestination(patrolDestination);
                    hasArrived = false;
                    enemyAnimator.SetBool("isWalking",true);
                }
            }
        }
        yield return new WaitUntil(() => hasArrived);
        Decide();
    }
    private void Decide()
    {
        int odds = Random.Range(0, 100);
        if (odds <= 25) {
            StartCoroutine(Chill());
        }else
            StartCoroutine(Patrol());
    }
    IEnumerator Chill()
    {
        enemyAnimator.SetBool("isWalking",false);
        enemyAnimator.SetBool("isShooting",false);
        yield return new WaitForSeconds(Random.Range(3, 10));
        StartCoroutine(Patrol());
    }
    private void DettectPatrolDistance()
    {
        Vector3 distance = transform.position - patrolDestination;
        hasArrived = distance.magnitude <= 1;
    }
        #endregion
    
    void DettectPlayerOrAudioNearby()
    {
        lifeCanvas.gameObject.SetActive(isAttacking);
        targetsDettectes = Physics.OverlapSphere(transform.position, rangeDetection).ToList();
        foreach (var detections in targetsDettectes) {
            if (detections.gameObject.CompareTag("Player"))
                enemyTarget = detections.gameObject;
        }
        
        if (enemyTarget != null) {
            if (enemyTarget.CompareTag("Player")) {
                if (!isAttacking)
                {
                    Attack();
                }
                if (enemyAgent.remainingDistance < 1)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(enemyTarget.transform.GetChild(0).transform.position - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 8 * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                }
            }
        }
        
        if(enemyTarget!= null)
            distanceFromPlayer = transform.position - enemyTarget.transform.position;
        
        if (distanceFromPlayer.magnitude >= rangeDetection * 5) {
            enemyTarget = null;
            isAttacking = false;
            if (!isPatrolling)
            {
                isPatrolling = true;
                StartCoroutine(Patrol());
                StopCoroutine(Attacking());
            }
        }
    }
    private void Attack()
    {
        isAttacking = true;
        isPatrolling = false;
        StopCoroutine(Patrol());
        StartCoroutine(Attacking());
        StartCoroutine(LoadEnergy());
        alert.gameObject.SetActive(true);
    }
    IEnumerator Reposicione()
    {
        yield return new WaitForSeconds(0);
        NavMeshHit hit;
        patrolDestination = (Random.insideUnitSphere * rangeDetection + new Vector3(0,300,0));
        if (Physics.Raycast(patrolDestination, Vector3.down, out RaycastHit hitInfo)) {
            if (NavMesh.SamplePosition(hitInfo.point, out hit, .5f, NavMesh.AllAreas)) {
                patrolDestination = hit.position;
                Vector3 distance = patrolDestination - enemyTarget.transform.position;
                if (distance.magnitude >= rangeDetection * 1.6f)
                    StartCoroutine(Reposicione());else {
                    enemyAgent.SetDestination(patrolDestination);
                    enemyAnimator.SetBool("isShooting",false);
                    enemyAnimator.SetBool("isWalking",true);
                }
                
            }
        }
    }

    IEnumerator LoadEnergy()
    {
        yield return new WaitForSeconds(1);
        if (isAttacking) {
            StartCoroutine(LoadEnergy());
            energy++;

        }
    }
    IEnumerator Attacking()
    {
        if (!isAttacking)
            yield break;
        if (myWeapon.atualAmmo <= 0)
        {
            StartCoroutine(Reposicione());
            yield return new WaitForSeconds(5);
            myWeapon.atualAmmo = myWeapon.maxAmmo;
        }
        yield return new WaitForSeconds(myWeapon.fireRate);
       AttackBehaviour normalAttack = null;
       AttackBehaviour normalAreaAttack = null;
       AttackBehaviour especialAttack = null;
       foreach (var weaponAttack in myWeapon.attacks)
       {
           switch (weaponAttack.attackType)
           {
               case AttackBehaviour.AttacksList.normalAttack:
                   normalAttack = weaponAttack;
                   break;
               case AttackBehaviour.AttacksList.normalAreaAttack:
                   normalAreaAttack = weaponAttack;
                   break;
               case AttackBehaviour.AttacksList.especialAttack:
                   especialAttack = weaponAttack;
                   break;
           }
       }
       
       if (waitingForSpecial)
       {
           if (energy >= especialAttack.minEnergy)
           {
               myWeapon.Attack("EspecialAttack");
               energy -= especialAttack.minEnergy;
               waitingForSpecial = false; 
           }
       }else
       {
           if (energy < normalAreaAttack.minEnergy)
           {
               myWeapon.Attack("NormalAttack");
           }else if (energy <= especialAttack.minEnergy && energy >= normalAreaAttack.minEnergy)
           {
               int odds = Random.Range(0, 100);
               if (odds < 15) {
                   myWeapon.Attack("NormalAreaAttack");
                   energy-= normalAreaAttack.minEnergy;
               }else {
                   myWeapon.Attack("NormalAttack");
                   int odd = Random.Range(0, 100);
                   if(odds < 45)
                       waitingForSpecial = true;
               }
           }
       }
       enemyAgent.SetDestination(transform.position);
       enemyAnimator.SetBool("isShooting",true);
       enemyAnimator.SetBool("isWalking",false);
       
       if (isAttacking)
           StartCoroutine(Attacking());
    }
    private void GetEnemyReferences()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        float actualDamage;
        actualDamage = Random.Range(.5f, 1);
        life -= Mathf.Round((actualDamage * damage));
        lifeBar.fillAmount = life / 100;
        GameObject damageObj = Instantiate(damageCanvas, transform.position + new Vector3(-2, 2.8f,0) , transform.rotation, transform);
        damageObj.transform.GetChild(0).GetComponent<Text>().text = Mathf.Round((actualDamage * damage)).ToString();

        if (!isAttacking) {
            enemyTarget = GameObject.FindWithTag("Player");
            Attack();
        }
        
        if (life <= 0)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ItemDropManager.instance.DropItem(dropInfo, transform.position);
        QuestController.instance.EnemyEliminated(questType);
    }
}
