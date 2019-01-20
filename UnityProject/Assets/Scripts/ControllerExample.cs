using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerExample : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObject;

    // Start is called before the first frame update
    void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        var device = SteamVR_Controller.Input((int)trackedObject.index);
        if(device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            //trigger press
        }
        if(device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {

        }
        // default axis is thumb position on touchpad
        var touchpadPosition = device.GetAxis();

        var triggerPressure = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;

        Debug.Log(triggerPressure);
        var pointingDirection = transform.forward;

    }
}
