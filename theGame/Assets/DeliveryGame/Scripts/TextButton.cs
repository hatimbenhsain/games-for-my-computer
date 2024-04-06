using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextButton : MonoBehaviour
{
    public string character;
    public TMP_InputField textField;
    public bool erase=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked(){
        if(!erase){
            textField.text=textField.text+character;
        }else if(textField.text.Length>0){
            textField.text=textField.text.Substring(0,textField.text.Length-1);
        }
    }
}
