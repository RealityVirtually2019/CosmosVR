using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAroundDaydreamController : MonoBehaviour
{
    public GameObject dayDreamCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButton(GvrControllerButton.TouchPadTouch))
        {
            var pointingDirection = dayDreamCamera.transform.forward;
            transform.Translate(pointingDirection * Time.deltaTime * 5.0f);
        }
    }
}
