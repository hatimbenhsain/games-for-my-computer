using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class MuseumGameManager : MonoBehaviour
{
    public Material controlledMaterial; 
    public Image scannerUIImage; 
    public Image scannerFrame;
    public float lerpSpeed = 2.0f;

    // material params for default mode

    public float defaultCellMoveSpeed = 1;
    public float defaultZoom = 15;
    public float defaultBeat = 4;
    public float defaultNoiseSpeed = 0.2f;
    public float defaultBlendFactor = 0.5f;
    public Color defaultColor = Color.white;
    public Color defaultFrameColor = Color.white;

    // material params for alien mode
    public float alienCellMoveSpeed = 1;
    public float alienZoom = 15;
    public float alienBeat = 4;
    public float alienNoiseSpeed = 0.2f;
    public float alienBlendFactor = 0.5f;
    public Color alienColor = Color.white;
    public Color alienFrameColor = Color.white;
    private bool lightFlickered = false;

    //lighting
    public GameObject lightingGroup;
    public AudioSource clangingMetal;

    // scanned alien
    public bool isAlienScanned = false;
    public bool isAlien = false;
    public bool isScannerOn = false;

    // obstacles
    public GameObject obstacle1;
    public GameObject obstacle2;


    // alien count
    public int alienCount = 0;

    public bool isFloating = true;
    public bool flickeringOn = false;
    GameObject floatingObjects;
    void Start()
    {
        floatingObjects = GameObject.Find("FloatingObjects");
        Cursor.visible = false;
    }
    
    void Update()
    {
        UpdateMaterialProperties();
        UpdateScannerUI();
        UpdateObstacles();
        if (!isFloating)
        {
            DisableAllFloating(floatingObjects);
        }
        
        if (flickeringOn == true)
        {
            //StartCoroutine(lightFlickeringCoroutine());
        }
        else
        {
            //StopCoroutine(lightFlickeringCoroutine());
        }
        
        
    }

    void UpdateMaterialProperties()
    {
        if (controlledMaterial != null)
        {

            float cellMoveSpeed = Mathf.Lerp(controlledMaterial.GetFloat("_CellMoveSpeed"),
                isAlienScanned ? alienCellMoveSpeed : defaultCellMoveSpeed, Time.deltaTime * lerpSpeed);
            float zoom = Mathf.Lerp(controlledMaterial.GetFloat("_Zoom"),
                isAlienScanned ? alienZoom : defaultZoom, Time.deltaTime * lerpSpeed);
            float beat = Mathf.Lerp(controlledMaterial.GetFloat("_Beat"),
                isAlienScanned ? alienBeat : defaultBeat, Time.deltaTime * lerpSpeed);
            float noiseSpeed = Mathf.Lerp(controlledMaterial.GetFloat("_NoiseSpeed"),
                isAlienScanned ? alienNoiseSpeed : defaultNoiseSpeed, Time.deltaTime * lerpSpeed);
            float blendFactor = Mathf.Lerp(controlledMaterial.GetFloat("_BlendFactor"),
                isAlienScanned ? alienBlendFactor : defaultBlendFactor, Time.deltaTime * lerpSpeed);
            Color currentColor = controlledMaterial.GetColor("_MyColor");
            Color targetColor = isAlienScanned ? alienColor : defaultColor;
            Color lerpedColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * lerpSpeed);

            controlledMaterial.SetFloat("_CellMoveSpeed", cellMoveSpeed);
            controlledMaterial.SetFloat("_Zoom", zoom);
            controlledMaterial.SetFloat("_Beat", beat);
            controlledMaterial.SetFloat("_NoiseSpeed", noiseSpeed);
            controlledMaterial.SetFloat("_BlendFactor", blendFactor);
            controlledMaterial.SetColor("_MyColor", lerpedColor);
        }
    }

    void UpdateScannerUI()
    {
        if (scannerFrame != null)
        {
            Color targetColor;
            if (isAlienScanned)
            {
                targetColor = alienFrameColor; 
            }
            else
            {
                targetColor = defaultFrameColor; 
            }

            scannerFrame.color = Color.Lerp(scannerFrame.color, targetColor, Time.deltaTime * lerpSpeed);
        }

        // control UI image visibility based on isScannerOn
        if (scannerUIImage != null)
        {
            scannerUIImage.gameObject.SetActive(isScannerOn);
        }
    }


    void UpdateObstacles()
    {
        // control obstacle based on alienCount
        if (obstacle1 != null && alienCount == 1)
        {
            obstacle1.SetActive(false);
        }

        if (obstacle2 != null && alienCount == 4)
        {
            if (!lightFlickered)
            {
                StartCoroutine(lightFlickeringCoroutine());
                lightFlickered = true;
            }
            obstacle2.SetActive(false);
            flickeringOn = true;
            alienCount++; //coroutine being weird if it stays on aliencount 4, it doesn't stop as it's always turning the bool back on. If u have a better way that's cool~
        }
        
    }

    // make all the floating object not floating
    void DisableAllFloating(GameObject parent)
    {
        FloatingObjects[] floatingObjects = parent.GetComponentsInChildren<FloatingObjects>();
        foreach (FloatingObjects floater in floatingObjects)
        {
            floater.enableFloating = false;
        }
    }

    //Lights Flickering
    IEnumerator lightFlickeringCoroutine(){
        lightingGroup.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        lightingGroup.SetActive(true);
        clangingMetal.Play();
        yield return new WaitForSeconds(0.5f);
        lightingGroup.SetActive(false);
        yield return new WaitForSeconds(0.75f);
        lightingGroup.SetActive(true);
        flickeringOn = false;
    }

    [YarnCommand]
    public void disableFloat(){
        isFloating = false;
    }
    [YarnCommand]
    public void UpdateAlienCount(){
        alienCount++;
    }
}
