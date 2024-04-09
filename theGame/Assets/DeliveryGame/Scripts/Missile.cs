using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Missile : MonoBehaviour
{

    public Transform target;
    private CharacterController targetController;
    private Rigidbody rb;
    private Animator animator;
    public AnimationClip rocketClip;

    public float lifeSpan=14f;
    public float rotationSpeed=95f;
    public float speed=15f;

    private float life=0f;

    [Header("PREDICTION")] 
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;
    private Vector3 _standardPrediction, _deviatedPrediction;

    [Header("DEVIATION")] 
    [SerializeField] private float _deviationAmount = 50;
    [SerializeField] private float _deviationSpeed = 2;

    private CanvasScript canvasScript;

    void Start()
    {
        rb=GetComponent<Rigidbody>();
        target=GameObject.FindWithTag("Player").transform;
        targetController=target.gameObject.GetComponent<CharacterController>();
        animator=GetComponent<Animator>();
        animator.speed=rocketClip.length/lifeSpan;
        transform.LookAt(target);
        canvasScript=FindObjectOfType<CanvasScript>();
    }

    void FixedUpdate(){
        rb.velocity=transform.forward*speed*(Mathf.Min(life,1f));
        float leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(
        transform.position,target.transform.position));
        PredictMovement(leadTimePercentage);
        AddDeviation(leadTimePercentage);
        RotateRocket();
    }

    void Update(){
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime>=1){
            Destroy(gameObject);
        }
        life+=Time.deltaTime;
    }

    private void PredictMovement(float leadTimePercentage){
        float predictionTime=Mathf.Lerp(0,_maxTimePrediction,leadTimePercentage);
        _standardPrediction=(target.position+targetController.center)+targetController.velocity*predictionTime;
    }

    private void AddDeviation(float leadTimePercentage) {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);
        
        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void RotateRocket(){
        Vector3 heading=_deviatedPrediction-transform.position;
        var rotation=Quaternion.LookRotation(heading);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,rotationSpeed*Time.fixedDeltaTime));
    }

    private void OnCollide(Collider other) {
        if(other.gameObject.tag=="Player"){
            canvasScript.BlueScreen();
            Debug.Log("blue screen");
        }else{
            Debug.Log(other.gameObject);
        }
    }
}
