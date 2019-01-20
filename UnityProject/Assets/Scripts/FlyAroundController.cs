using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAroundController : MonoBehaviour
{
    public SteamVR_TrackedObject rightController;
    public SteamVR_TrackedObject leftController;

    public GameObject EyeCamera;
       
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var rightDevice = SteamVR_Controller.Input((int)rightController.index);

        var pointingDirection = EyeCamera.transform.forward;

        var triggerPressure_r = rightDevice.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
        
        //right trigger press and hold
        if (triggerPressure_r > 0.1f)
        {
            transform.Translate(pointingDirection * Time.deltaTime * triggerPressure_r * 5.0f);
        }

        //Commented out temporarily because our device only had one functional controller
        //var leftDevice = SteamVR_Controller.Input((int)leftController.index);
        //var triggerPressure_l = leftDevice.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;

        ////double trigger press and hold
        //if ((triggerPressure_l) > 0.1f && (triggerPressure_r > 0.1f))
        //{ 
        //    float triggerPressure_avg = (triggerPressure_l + triggerPressure_r) / 2.0f;
        //    transform.Translate(pointingDirection * Time.deltaTime * triggerPressure_avg * 5.0f);
        //}
    }
}
