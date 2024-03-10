using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;//need this to be able to use yarn command

public class AvatarSwitch : MonoBehaviour
{
    public GameObject spriteRenderer01;
    public GameObject spriteRenderer02;

    public Sprite characterSprite;
    public Sprite playerSprite;

    // Start is called before the first frame update
    void Start()
    {
       spriteRenderer01.GetComponent<Image>().sprite = playerSprite;
       spriteRenderer02.GetComponent<Image>().sprite= playerSprite;
        
    }

    [YarnCommand]//hey yarn pay attention to this cause its a command
    public void ChangeAvatartoCharacter()//name of the command 
    {
        spriteRenderer01.GetComponent<Image>().sprite = characterSprite;
        spriteRenderer02.GetComponent<Image>().sprite = characterSprite;

    }

        [YarnCommand]//hey yarn pay attention to this cause its a command
    public void ChangeAvatartoPlayer()//name of the command 
    {
        spriteRenderer01.GetComponent<Image>().sprite = playerSprite;
        spriteRenderer02.GetComponent<Image>().sprite = playerSprite;
    }

}
