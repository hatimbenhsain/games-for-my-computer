using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_InputField))]
public class FieldSelector : MonoBehaviour, IPointerClickHandler
{
    private TMP_InputField myField;

    private void Start()
    {
        myField = GetComponent<TMP_InputField>();
        InputFieldManager.Instance.RegisterField(myField);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InputFieldManager.Instance.SetActiveField(myField);
    }
}