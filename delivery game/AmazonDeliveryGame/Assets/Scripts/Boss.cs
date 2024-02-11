using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    enum State {Idle,Patroling,Charging}
    private NavMeshAgent agent;
    private Transform player;
    public Transform[] patrolPoints;
    public Transform currentPatrolPoint;
    private Transform prevPatrolPoint;
    private State currentState;

    void Start()
    {
        player=GameObject.FindWithTag("Player").transform;
        agent=GetComponent<NavMeshAgent>();
        GameObject[] go=GameObject.FindGameObjectsWithTag("PatrolPoint");
        patrolPoints=new Transform[go.Length];
        for(int i=0;i<go.Length;i++){
            patrolPoints[i]=go[i].transform;
        }
        currentState=State.Patroling;
        currentPatrolPoint=patrolPoints[0];
        prevPatrolPoint=currentPatrolPoint;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState){
            case State.Idle:
                break;
            case State.Patroling:
                Patrol();
                break;
            case State.Charging:
                break;
        }
        // if(Vector3.Distance(transform.position,player.position)>1f){
        //     ChasePlayer();
        // }
        
    }

    void ChasePlayer(){
        agent.SetDestination(player.position);
        transform.LookAt(player);
    }

    void Patrol(){
        if(Vector3.Distance(currentPatrolPoint.position,transform.position)<1f){
            //Find new patrol point
            Transform newPatrolPoint=currentPatrolPoint;
            float minDistance=10000f;
            foreach(Transform p in patrolPoints){
                if(p!=currentPatrolPoint && p!=prevPatrolPoint){
                    float d=Vector3.Distance(p.position,transform.position);
                    if(d<minDistance){
                        minDistance=d;
                        newPatrolPoint=p;
                    }
                }
            }
            prevPatrolPoint=currentPatrolPoint;
            currentPatrolPoint=newPatrolPoint;
        }
        agent.SetDestination(currentPatrolPoint.position);
    }
}
