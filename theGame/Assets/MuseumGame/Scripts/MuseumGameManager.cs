using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // scanned alien
    public bool isAlien = false;
    public bool isScannerOn = false;

    // obstacles
    public GameObject obstacle1;
    public GameObject obstacle2;

    // alien count
    public int alienCount = 0;

    public bool isFloating = true;
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
    }

    void UpdateMaterialProperties()
    {
        if (controlledMaterial != null)
        {

            float cellMoveSpeed = Mathf.Lerp(controlledMaterial.GetFloat("_CellMoveSpeed"),
                isAlien ? alienCellMoveSpeed : defaultCellMoveSpeed, Time.deltaTime * lerpSpeed);
            float zoom = Mathf.Lerp(controlledMaterial.GetFloat("_Zoom"),
                isAlien ? alienZoom : defaultZoom, Time.deltaTime * lerpSpeed);
            float beat = Mathf.Lerp(controlledMaterial.GetFloat("_Beat"),
                isAlien ? alienBeat : defaultBeat, Time.deltaTime * lerpSpeed);
            float noiseSpeed = Mathf.Lerp(controlledMaterial.GetFloat("_NoiseSpeed"),
                isAlien ? alienNoiseSpeed : defaultNoiseSpeed, Time.deltaTime * lerpSpeed);
            float blendFactor = Mathf.Lerp(controlledMaterial.GetFloat("_BlendFactor"),
                isAlien ? alienBlendFactor : defaultBlendFactor, Time.deltaTime * lerpSpeed);
            Color currentColor = controlledMaterial.GetColor("_MyColor");
            Color targetColor = isAlien ? alienColor : defaultColor;
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
            if (isAlien)
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

        if (obstacle2 != null && alienCount == 5)
        {
            obstacle2.SetActive(false);
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
}
