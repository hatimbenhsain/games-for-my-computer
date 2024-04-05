using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAssets : MonoBehaviour
{

    [System.Serializable]    
    public struct DialogueCharacter{
        public string name;
        public Sprite portraitSprite;
        public AudioClip voiceClip;
    }

    public DialogueCharacter[] dialogueCharacters;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
