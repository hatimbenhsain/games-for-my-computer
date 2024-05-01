using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldActivator : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private TMP_InputField myField;
    private void Start()
    {
        InputFieldManager.Instance.RegisterField(myField);
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        Activate();
    }

    public void Activate(){
        InputFieldManager.Instance.SetActiveField(myField);
    }
    
}
