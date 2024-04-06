using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//To-do: Add Comments

public class Boss : MonoBehaviour
{
    public enum State {Idle,Patroling,Charging,Sphering}
    private NavMeshAgent agent;
    private Transform player;
    public Transform[] patrolPoints;
    public Transform currentPatrolPoint;
    private Transform prevPatrolPoint;
    [SerializeField]public State currentState;

    public float patrolLength=5f; //in seconds
    private float patrolTime=0f;

    public float chargeLength=1f;
    public float chargeTime=0f;
    private float prevChargeTime=0f;
    //probably wouold be more efficient to have one "time" value for every state
    private float sphereTime=0f;

    private float minRunSpeed=3f;
    public float walkSpeed=1.5f;
    public float walkAcceleration=8f;
    public float chargeSpeed=5f;
    public float chargeAcceleration=16f;

    private Animator animator;

    public GameObject spheresPrefab;

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
        animator=GetComponent<Animator>();
        agent.speed=walkSpeed;
        agent.acceleration=walkAcceleration;
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
            case State.Sphering:
                Sphere();
                break;
        }

        if(agent.velocity.magnitude>=minRunSpeed){
            animator.SetBool("running",true);
            animator.SetBool("walking",false);
        }else if(agent.velocity.magnitude>0f){
            animator.SetBool("running",false);
            animator.SetBool("walking",true);
        }else{
            animator.SetBool("running",false);
            animator.SetBool("walking",false);
        }
        
    }

    void ChasePlayer(){
        agent.SetDestination(player.position);
        transform.LookAt(player);
    }

    void Charge(){
        agent.speed=chargeSpeed;
        agent.acceleration=chargeAcceleration;
        if(chargeTime>chargeLength){
            if(prevChargeTime<=chargeLength){
                //chargeTime=0f;
                GetComponent<AudioSource>().pitch=1.5f;
                GetComponent<AudioSource>().Play();
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
        agent.speed=walkSpeed;
        agent.acceleration=walkAcceleration;
        if(patrolTime>patrolLength){
            float r=Random.Range(0f,1f);
            if(r<=0.5f){
                currentState=State.Charging;
            }else{
                currentState=State.Sphering;
                sphereTime=0f;
                animator.SetTrigger("attacking");
            }
            patrolTime=0f;
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

    //throwing out spheres
    void Sphere(){
        if(sphereTime==0f){
            Instantiate(spheresPrefab,transform);
            transform.LookAt(player);
            agent.SetDestination(transform.position);
            GetComponent<AudioSource>().pitch=1f;
            GetComponent<AudioSource>().Play();
        }else if(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name!="attack1"){
            currentState=State.Patroling;
            patrolTime=0f;
        }
        sphereTime+=Time.deltaTime;
    }
}
