using UnityEngine;
using TMPro; // Ensure this namespace is included

public class TextButton : MonoBehaviour
{
    public string character;
    public bool erase = false;

    public void Clicked()
    {
        TMP_InputField activeTextField = InputFieldManager.Instance.GetActiveField();
        if (activeTextField != null)
        {
            if (!erase)
            {
                activeTextField.text += character;
            }
            else if (activeTextField.text.Length > 0)
            {
                activeTextField.text = activeTextField.text.Substring(0, activeTextField.text.Length - 1);
            }
        }
    }
}