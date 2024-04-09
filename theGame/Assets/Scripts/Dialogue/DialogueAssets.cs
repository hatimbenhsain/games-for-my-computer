using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using UnityEditor;

public class DialogueAssets : MonoBehaviour
{
    public TMP_Text lineContainer;
    public TMP_Text talkerContainer;
    private string prevLine;
    private string prevTalker;
    private DialogueRunner dialogueRunner;

    public Image portraitRenderer1;
    public Image portraitRenderer2;
    public AudioSource voiceAudioSource;

    private GameObject interlocutor;

    [System.Serializable]    
    public struct DialogueCharacter{
        public string name;
        public Sprite portraitSprite;
        public AudioClip[] voiceClip;
    }

    public DialogueCharacter[] dialogueCharacters;
    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner=FindObjectOfType<DialogueRunner>();
        interlocutor=null;
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueRunner.IsDialogueRunning){
            if(talkerContainer.text!=prevTalker || lineContainer.text!=prevLine){
                Debug.Log("diff talker");
                DialogueCharacter dc=new DialogueCharacter();
                bool foundDC=false;
                foreach(DialogueCharacter d in dialogueCharacters){
                    if(d.name.ToLower()==talkerContainer.text.ToLower()){
                        dc=d;
                        foundDC=true;
                        Debug.Log("new talker");
                        break;
                    }
                }
                if(foundDC){
                    portraitRenderer1.sprite=dc.portraitSprite;
                    portraitRenderer2.sprite=dc.portraitSprite;
                    voiceAudioSource.clip=dc.voiceClip[Random.Range(0,dc.voiceClip.Length)];
                    voiceAudioSource.pitch=Random.Range(0.9f,1.1f);
                    voiceAudioSource.Play();
                    portraitRenderer1.color=Color.white;
                    portraitRenderer2.color=Color.white;
                }else{
                    portraitRenderer1.sprite=null;
                    portraitRenderer2.sprite=null;
                    portraitRenderer1.color=new Color(0f,0f,0f,0f);
                    portraitRenderer2.color=new Color(0f,0f,0f,0f);
                }
            }
            prevTalker=talkerContainer.text;
            prevLine=lineContainer.text;
        }
    }

    [YarnCommand]
    void RainView(){
        //CODE TO CHANGE TO RAIN VIEW DIALOGUE
    }

    [YarnCommand]
    public void PlaceInterlocutor(string s){
        GameObject prefab=(GameObject)AssetDatabase.LoadAssetAtPath("Assets/Level/Prefabs/NPCs/"+s+".prefab",typeof(GameObject));
        interlocutor=Instantiate(prefab,GameObject.Find("InterlocutorTransform").transform);
        interlocutor.transform.parent=interlocutor.transform.parent.parent.parent.parent;
    }

    [YarnCommand]
    public void DeleteInterlocutor(){
        Destroy(interlocutor);
    }
}
