using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TonguesScript : MonoBehaviour
{
    public GameObject Tongue1;
    public GameObject Tongue2;

    public Button TongueButton;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Tongue1 != null)
            Tongue1.SetActive(false);

        if (Tongue2 != null)
            Tongue2.SetActive(false);

        if (TongueButton != null)
        {
            TongueButton.onClick.AddListener(OnButtonClick);
        }
    }

    public void OnButtonClick()
    {
        if (Tongue1 != null && Tongue2 != null)
        {
            Tongue1.SetActive(!Tongue1.activeSelf);
            Tongue2.SetActive(!Tongue2.activeSelf);
        }
    }

}
