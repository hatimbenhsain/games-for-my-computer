using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CutsceneDialogue2 : MonoBehaviour
{
    public TMP_Text txt;

    private string[] lines={
        "",
        "(I didn't rehearse that part. I hope I still look attractive without the jacket.)", 
        "",
        "(Wait is this a remix?)", 
        ""};

    private int index=0;
    void Start()
    {
        txt.text=lines[index];
    }

    // Update is called once per frame
    void Update()
    {
        txt.text=lines[Mathf.Min(index,lines.Length-1)];
    }

    public void AdvanceText(){
        index+=1;
    }

    public void ResetText(){
        index=0;
    }

    public void SetText(int i){
        index=i;
    }
    
}
