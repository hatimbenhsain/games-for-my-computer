using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackMessage : MonoBehaviour
{
    public Sprite cutSprite;
    public Sprite organSprite;
    public Sprite stitchSprite;

    public Image feedbackUI;

    public float lifeSpan=1f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer=0f;
        feedbackUI.GetComponent<Animator>().speed=1;
    }

    // Update is called once per frame
    void Update()
    {
        feedbackUI.GetComponent<Animator>().speed=1;
        if(timer>=lifeSpan){
            Debug.Log("destroy at "+timer);
            Destroy(gameObject);
        }
        timer+=Time.deltaTime;
    }

    public void SetSprite(int type){
        Sprite s=cutSprite;
        switch(type){
            case 0:
                s=cutSprite;
                break;
            case 1:
                s=organSprite;
                break;
            case 2:
                s=stitchSprite;
                break;
        }
        feedbackUI.GetComponent<Image>().sprite=s;
    }
}
