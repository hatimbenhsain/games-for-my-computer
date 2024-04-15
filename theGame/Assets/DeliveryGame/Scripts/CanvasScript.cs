using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    private float shakeTime=-1f;
    private float shakeLength=5f;

    private bool shaking=false;

    public List<GameObject> windows;
    public DraggableWindow[] draggableWindows;

    public float shakeIntensity=10f;

    private Canvas canvas;
    private float cWidth;
    private float cHeight;

    private float redScreenCooldownTimer;
    private float redSCreenCooldownDuration;

    public float scatterLength=4f;
    private float scatterTime=-1f;
    public float scatterSpeed=1f;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        redSCreenCooldownDuration=2f;
        
    }

    void Update()
    {
        if(shaking){
            cWidth = canvas.GetComponent<RectTransform>().rect.width;
            cHeight = canvas.GetComponent<RectTransform>().rect.height;
            foreach(GameObject w in windows){
                if(w.activeInHierarchy){
                    DraggableWindow dw=w.GetComponent<DraggableWindow>();
                    if(dw!=null && dw.shakeImmune){
                        continue;
                    }
                    RectTransform r=w.GetComponent<RectTransform>();
                    Vector2 pos=r.anchoredPosition;
                    float intensity=2f+(shakeTime-shakeLength)*shakeIntensity/shakeLength;
                    pos=new Vector2(pos.x+Random.Range(-0.5f,0.5f)*shakeIntensity,pos.y+Random.Range(-0.5f,0.5f)*shakeIntensity);
                    pos=new Vector2(Mathf.Min(Mathf.Max(pos.x,-cWidth/2f),cWidth/2f),Mathf.Min(Mathf.Max(pos.y,-cHeight/2f),cHeight/2f));
                    r.anchoredPosition=pos;
                }
            }
        }

        if(scatterTime<scatterLength && scatterTime>=0f){
            cWidth = canvas.GetComponent<RectTransform>().rect.width;
            cHeight = canvas.GetComponent<RectTransform>().rect.height;
            float s=scatterSpeed;
            if(scatterTime>scatterLength-1){
                s=(1-easeOutSine(scatterTime-(scatterLength-1)))*scatterSpeed;
            }
            foreach(GameObject wd in windows){
                if(wd.activeInHierarchy){
                    DraggableWindow dw=wd.GetComponent<DraggableWindow>();
                    if(dw!=null && dw.scatterImmune){
                        continue;
                    }
                    RectTransform r=wd.GetComponent<RectTransform>();
                    Vector2 pos=r.anchoredPosition;
                    if(scatterTime==0f){
                        // Vector2 pos=new Vector2(cWidth*Random.Range(0f,1f)-cWidth/2,cHeight*Random.Range(0f,1f)-cHeight/2);
                        // dw.scatterTarget=pos;
                        // dw.currentOrigin=r.anchoredPosition;

                        float angle=Random.Range(0,2*Mathf.PI);
                        Vector2 dir=new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
                        dw.scatterDirection=dir;
                    }else{
                        // float k=easeOutSine(scatterTime/scatterLength);
                        // float x=dw.currentOrigin.x+k*(dw.scatterTarget.x-dw.currentOrigin.x);
                        // float y=dw.currentOrigin.y+k*(dw.scatterTarget.y-dw.currentOrigin.y);
                        // r.anchoredPosition=new Vector2(x,y);
                        float x=pos.x+dw.scatterDirection.x*s*Time.deltaTime;
                        float y=pos.y+dw.scatterDirection.y*s*Time.deltaTime;
                        float w=r.rect.width;
                        float h=r.rect.height;
                        if(x-h/2<-cWidth/2 || x+h/2>cWidth/2 ){
                            dw.scatterDirection=new Vector2(-dw.scatterDirection.x,dw.scatterDirection.y);
                        }
                        if(y-w/2<-cHeight/2 || y+w/2>cHeight/2){
                            dw.scatterDirection=new Vector2(dw.scatterDirection.x,-dw.scatterDirection.y);
                        }
                        r.anchoredPosition=new Vector2(x,y);
                    }
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

        if(scatterTime>=0f){
            scatterTime+=Time.deltaTime;
        }

        redScreenCooldownTimer+=Time.deltaTime;
    }

    public void WhiteScreen(){
        shaking=true;
        shakeTime=shakeLength;
        draggableWindows=FindObjectsOfType<DraggableWindow>();
        windows=new List<GameObject>();
        foreach(DraggableWindow dw in draggableWindows){
            windows.Add(dw.gameObject);
        }        
        foreach(GameObject w in windows){
            DraggableWindow dw=w.GetComponent<DraggableWindow>();
            if(dw!=null){
                dw.shakeImmune=false;
            }
        }
        GetComponent<Animator>().SetTrigger("WhiteScreen");
    }

    public void RedScreen(){
        if(redScreenCooldownTimer>redSCreenCooldownDuration){
            GetComponent<Animator>().SetTrigger("RedScreen");
            Debug.Log("red screen from canvas script");
            redScreenCooldownTimer=0f;
        }
    }

    public void BlueScreen(){
        scatterTime=0f;
        draggableWindows=FindObjectsOfType<DraggableWindow>();
        windows=new List<GameObject>();
        foreach(DraggableWindow dw in draggableWindows){
            windows.Add(dw.gameObject);
        }        
        foreach(GameObject w in windows){
            DraggableWindow dw=w.GetComponent<DraggableWindow>();
            if(dw!=null){
                dw.scatterImmune=false;
            }
        }
        GetComponent<Animator>().SetTrigger("BlueScreen");
    }

    public float easeOutSine(float x ){
        return Mathf.Sin((x*Mathf.PI)/2);
    }
}
