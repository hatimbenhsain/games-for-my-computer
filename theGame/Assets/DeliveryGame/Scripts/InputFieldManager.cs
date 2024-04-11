using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldManager : MonoBehaviour
{
    public static InputFieldManager Instance { get; private set; }

    private TMP_InputField activeField;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void RegisterField(TMP_InputField field)
    {
        // Optionally keep a list of fields if needed
    }

    public void SetActiveField(TMP_InputField field)
    {
        activeField = field;
        Debug.Log("Active field set: " + field.name);
    }

    public void ClearActiveField()
    {
        activeField = null;
    }

    public TMP_InputField GetActiveField()
    {
        return activeField;
    }
}