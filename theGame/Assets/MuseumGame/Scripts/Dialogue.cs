using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public GameObject alienGooHolder;
    public GameObject janitorGooHolder;
    public TextMeshProUGUI alienTextComponent;
    public string[] lines;
    public float textSpeed;

    public AudioSource alienSounds;
    
    private int index;
    
    // Start is called before the first frame update
    void Start()
    {
        
        alienTextComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (alienTextComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                
                alienTextComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            alienTextComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        
        if (index < lines.Length - 1)
        {
            index++;
            alienTextComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            alienSounds.Play();
        }
        else
        { 
            index = 0;
            alienGooHolder.SetActive(false);
            janitorGooHolder.SetActive(true);
            alienSounds.Stop();
        }
    }
}
