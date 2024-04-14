using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Package : MonoBehaviour
{
    public GameObject[] items;
    public int index;

    private int hitTimes;
    public int hitsItTakesToOpen=3;

    // Start is called before the first frame update
    void Start()
    {
        hitTimes=0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        //Open cardboard box and replace with random item
        if(other.gameObject.tag=="Player"){
            hitTimes++;
            if(hitTimes>=hitsItTakesToOpen){
                //Instantiate(items[(int)Mathf.Floor(Random.Range(0,2.99f))],transform.position,Quaternion.identity);
                Instantiate(items[index%(items.Length)],transform.position,Quaternion.identity);
                index++;
                Destroy(transform.parent.parent.gameObject);
            }
        }
    }

    public void PlayerCollide(){
        hitTimes++;
        if(hitTimes>=hitsItTakesToOpen){
            //Instantiate(items[(int)Mathf.Floor(Random.Range(0,2.99f))],transform.position,Quaternion.identity);
            Debug.Log("instantiate "+index);
            int k=index%(items.Length);
            Debug.Log(k);
            Debug.Log(items[k]);
            Instantiate(items[index%(items.Length)],transform.position,Quaternion.identity);
            index++;
            Destroy(transform.parent.parent.gameObject);
        }
    }
}
