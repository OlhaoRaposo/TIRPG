using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{
    public bool isPatroling, isIddle;
    public Vector3 patrolDestination;
    public NavMeshAgent enemyAgent;
    public bool hasArrived;

    void Start()
    {
        enemyAgent = this.GetComponent<NavMeshAgent>();
        StartCoroutine(StartPatrolling());
        StartCoroutine(CheckIfIsStoped());
    }
    private void Update()
    {
        DettectPatrolDistance();
    }

    IEnumerator StartPatrolling()
    {
        isPatroling = true;
        isIddle = false;
        NavMeshHit hit;
        if (isPatroling) {
            patrolDestination = transform.position + (Random.insideUnitSphere * 20) + new Vector3(0,5,0);
            if (Physics.Raycast(patrolDestination, Vector3.down, out RaycastHit hitInfo)) {
                enemyAgent.SetDestination(hitInfo.point);
                if (NavMesh.SamplePosition(hitInfo.point, out hit, .5f, NavMesh.AllAreas))
                {
                    patrolDestination = hit.position + new Vector3( 0,0,0);
                    enemyAgent.SetDestination(patrolDestination);
                    hasArrived = false;
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
        isPatroling = false;
        enemyAgent.SetDestination(transform.position);
        yield return new WaitForSeconds(Random.Range(3, 8));
        StartCoroutine(StartPatrolling());
    }
    IEnumerator CheckIfIsStoped()
    {
        Vector3 pos0 = transform.position;
        yield return new WaitForSeconds(8);
        Vector3 pos1 = transform.position;
        Vector3 distance = pos1 - pos0;
        if (distance.magnitude <= 1) 
            StartCoroutine(StartPatrolling());
        StartCoroutine(CheckIfIsStoped());
    }
    private void DettectPatrolDistance()
    {
        Vector3 distance = transform.position - patrolDestination;
        if (distance.magnitude <= 2) {
            hasArrived = true;
        }
    }
}
