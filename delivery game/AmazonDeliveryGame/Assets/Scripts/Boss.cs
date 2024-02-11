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
    [SerializeField]private State currentState;

    public float patrolLength=5f; //in seconds
    private float patrolTime=0f;

    public float chargeLength=1f;
    private float chargeTime=0f;
    private float prevChargeTime=0f;

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
                Charge();
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

    void Charge(){
        if(chargeTime>chargeLength){
            if(prevChargeTime<=chargeLength){
                agent.SetDestination(player.position);
                transform.LookAt(player);
                //finding wall
                Vector3 target=player.position;
                Vector3 v=Vector3.Normalize(target-transform.position);
                float maxDistance=100f;
                RaycastHit hit;
                int layerMask=1<<7;
                if(Physics.Raycast(transform.position,v,out hit,maxDistance,layerMask)){
                    target=transform.position+hit.distance*v;
                    agent.SetDestination(target);
                }
            }else{
                if(Vector3.Distance(agent.destination,transform.position)<1f){
                    currentState=State.Patroling;
                    chargeTime=0f;
                    patrolTime=0f;
                }
            }
        }else{
            transform.LookAt(player);
            agent.SetDestination(transform.position);
        }
        prevChargeTime=chargeTime;
        chargeTime+=Time.deltaTime;
    }

    void Patrol(){
        if(patrolTime>patrolLength){
            currentState=State.Charging;
            patrolTime=0f;
            chargeTime=0f;
            return;
        }

        if(patrolTime==0f || Vector3.Distance(currentPatrolPoint.position,transform.position)<1f){
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
            Debug.Log("new patrol point");
            prevPatrolPoint=currentPatrolPoint;
            currentPatrolPoint=newPatrolPoint;
        }
        agent.SetDestination(currentPatrolPoint.position);

        patrolTime+=Time.deltaTime;
    }
}
