using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    private float shakeTime=-1f;
    private float shakeLength=5f;

    private bool shaking=false;

    public List<GameObject> windows;

    public float shakeIntensity=10f;

    private Canvas canvas;
    private float cWidth;
    private float cHeight;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        
    }

    void Update()
    {
        if(shaking){
            cWidth = canvas.GetComponent<RectTransform>().rect.width;
            cHeight = canvas.GetComponent<RectTransform>().rect.height;
            foreach(GameObject w in windows){
                if(w.activeInHierarchy){
                    RectTransform r=w.GetComponent<RectTransform>();
                    Vector2 pos=r.anchoredPosition;
                    float intensity=2f+(shakeTime-shakeLength)*shakeIntensity/shakeLength;
                    pos=new Vector2(pos.x+Random.Range(-0.5f,0.5f)*shakeIntensity,pos.y+Random.Range(-0.5f,0.5f)*shakeIntensity);
                    pos=new Vector2(Mathf.Min(Mathf.Max(pos.x,-cWidth/2f),cWidth/2f),Mathf.Min(Mathf.Max(pos.y,-cHeight/2f),cHeight/2f));
                    r.anchoredPosition=pos;
                }
            }
        }

        if(shakeTime>0f){
            shakeTime-=Time.deltaTime;
            shaking=true;
        }else{
            shakeTime=-1f;
            shaking=false;
        }
    }

    public void WhiteScreen(){
        shaking=true;
        shakeTime=shakeLength;
        GetComponent<Animator>().SetTrigger("WhiteScreen");
    }

    public void RedScreen(){
        GetComponent<Animator>().SetTrigger("RedScreen");
    }
}
