using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class pulsingCamera : MonoBehaviour
{
    public CinemachineFreeLook cam1;

    public CinemachineFreeLook cam2;

    public CinemachineFreeLook cam3;
    
    public CinemachineFreeLook cam4;

    public Boolean pulse;

    public int normal = 40;
    public int zoom = 30;

    public float smooth = 5;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Switch", 0, 0.5f); //repeating the function switch continuously
    }
    
    void Update()
    {
        if (pulse)
        {
            //apply the pulse effect to every camera. on zoom the FOV smooths at smooth rate from normal to zoom
            cam1.m_Lens.FieldOfView = Mathf.Lerp(cam1.m_Lens.FieldOfView, zoom, Time.deltaTime*smooth); 
            cam2.m_Lens.FieldOfView = Mathf.Lerp(cam2.m_Lens.FieldOfView, zoom, Time.deltaTime*smooth);
            cam3.m_Lens.FieldOfView = Mathf.Lerp(cam3.m_Lens.FieldOfView, zoom, Time.deltaTime*smooth);
            cam4.m_Lens.FieldOfView = Mathf.Lerp(cam3.m_Lens.FieldOfView, zoom, Time.deltaTime*smooth);
        }
        else
        {
            //on normal the FOV smooths at smooth rate from zoom to normal
            cam1.m_Lens.FieldOfView = Mathf.Lerp(cam1.m_Lens.FieldOfView, normal, Time.deltaTime*smooth);
            cam2.m_Lens.FieldOfView = Mathf.Lerp(cam2.m_Lens.FieldOfView, normal, Time.deltaTime*smooth);
            cam3.m_Lens.FieldOfView = Mathf.Lerp(cam3.m_Lens.FieldOfView, normal, Time.deltaTime*smooth);
            cam4.m_Lens.FieldOfView = Mathf.Lerp(cam3.m_Lens.FieldOfView, normal, Time.deltaTime*smooth);
        }
    }

    void Switch()
    {
        pulse = !pulse; //if pulse is pulsing otherwise it's not pulsing
    }
}
