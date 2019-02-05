using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectObjectDaydreamController : MonoBehaviour
{

    TextMesh text;
    public SpriteRenderer controllerWireframe;
    public ParticleSystem glow;

    bool lookingAtHelmet = false;
    
    // Start is called before the first frame update
    void Start()
    {
        glow = GameObject.Find("Glow").GetComponent<ParticleSystem>();
        if (PlatformInUse.CurrentPlatform == PlatformInUse.Platform.DAYDREAM)
        {
            controllerWireframe = GameObject.Find("ControllerWireframe").GetComponent<SpriteRenderer>();
        }
        else if (PlatformInUse.CurrentPlatform == PlatformInUse.Platform.OCULUSGO)
        {
            controllerWireframe = GameObject.Find("OculusControllerWireframe").GetComponent<SpriteRenderer>();  
        }
        text = GameObject.Find("HelmetText").GetComponent<TextMesh>();
    }

    void AppClicked()
    {
        if(lookingAtHelmet)
        {
            SceneManager.LoadScene(1);
        }
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Helmet")
            {
                if (PlatformInUse.CurrentPlatform == PlatformInUse.Platform.DAYDREAM)
                {
                    text.text = "Click the (-) (app button) to put on the headset";
                    if (DaydreamControllerHandler.appIsClicked)
                        SceneManager.LoadScene(1);
                }
                if (PlatformInUse.CurrentPlatform == PlatformInUse.Platform.OCULUSGO)
                {
                    text.text = "Click the touchpad button to put on the headset";
                    if (OculusGoControllerHandler.touchpadIsClicked)
                        SceneManager.LoadScene(1);
                }
                controllerWireframe.enabled = true;
                lookingAtHelmet = true;
                if (glow.isStopped)
                {
                    glow.Play();
                }
            }
            else
            {
                text.text = "";
                controllerWireframe.enabled = false;
                lookingAtHelmet = false;
                glow.Stop();
            }
        }
        else
        {
            text.text = "";
            controllerWireframe.enabled = false;
            lookingAtHelmet = false;
            glow.Stop();

        }
    }
}
