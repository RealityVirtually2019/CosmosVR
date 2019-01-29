using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAroundDaydreamController : MonoBehaviour
{
    public GameObject vr_camera;
    public AudioSource JetpackAudio;
    Vector3 vr_cameraDirection;

    // Start is called before the first frame update
    void Start()
    {
        JetpackAudio = GameObject.Find("JetPack").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        vr_cameraDirection = vr_camera.transform.forward;
        if (PlatformInUse.CurrentPlatform == PlatformInUse.Platform.DAYDREAM)
        {
            HandleDaydreamInput();
        }
        else if (PlatformInUse.CurrentPlatform == PlatformInUse.Platform.VIVE)
        {
            HandleViveInput();
        }
        else
        {
            Debug.Log("Platform not selected");
        }
    }

    void HandleDaydreamInput()
    {
        float tempSpeed = 0.0f;
        float flySpeed = 0.0f;

        if (DaydreamControllerHandler.touchPos.y > 0.6f)
        {
            tempSpeed = 1.5f;
        }
        else if (DaydreamControllerHandler.touchPos.y < -0.6f)
        {
            tempSpeed = -1.5f;
        }

        if (DaydreamControllerHandler.touchpadIsTouched)
        {
            flySpeed = tempSpeed;
        }
        if(DaydreamControllerHandler.touchpadIsClicked)
        {
            flySpeed = tempSpeed * 3.0f;
        }

        if((flySpeed > 0.1f) || (flySpeed < -0.1f))
        {
            PlayJetPackAudio();
        }
        else
        {
            StopJetpackAudio();
        }

        transform.Translate(vr_cameraDirection * Time.deltaTime * flySpeed);

    }

    void PlayJetPackAudio()
    {
        if(!JetpackAudio.isPlaying)
        {
            JetpackAudio.Play();
        }
    }

    void StopJetpackAudio()
    {
        if (JetpackAudio.isPlaying)
        {
            JetpackAudio.Stop();
        }
    }

    void HandleViveInput()
    {
        //TODO - add Vive stuff here
    }


}
