using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartScript : MonoBehaviour
{
    public GameObject CoupleNotKissing;
    public GameObject Heart;
    public GameObject KissingCouple;

    public Button HeartButton;
    
    // Start is called before the first frame update
    void Start()
    {
        if (CoupleNotKissing != null)
            CoupleNotKissing.SetActive(false);

        if (Heart != null)
            Heart.SetActive(false);
        
        if (KissingCouple != null)
            KissingCouple.SetActive(true);

        if (HeartButton != null)
        {
            HeartButton.onClick.AddListener(OnButtonClick);
        }
    }

    public void OnButtonClick()
    {
        if (CoupleNotKissing != null && Heart != null && KissingCouple != null)
        {
            CoupleNotKissing.SetActive(!CoupleNotKissing.activeSelf);
            Heart.SetActive(!Heart.activeSelf);
            KissingCouple.SetActive(false);
        }
    }

}
    
    
