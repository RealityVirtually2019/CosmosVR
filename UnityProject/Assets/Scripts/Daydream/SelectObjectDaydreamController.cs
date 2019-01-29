using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectObjectDaydreamController : MonoBehaviour
{

    TextMesh text;
    public SpriteRenderer controllerWireframe;
    public ParticleSystem glow;

    // Start is called before the first frame update
    void Start()
    {
        glow = GameObject.Find("Glow").GetComponent<ParticleSystem>();
        controllerWireframe = GameObject.Find("ControllerWireframe").GetComponent<SpriteRenderer>();
        text = GameObject.Find("HelmetText").GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Helmet")
            {
                Debug.Log("hit");
                text.text = "Click the (-) (app button) to put on the headset";
                controllerWireframe.enabled = true;
                if (glow.isStopped)
                {
                    glow.Play();
                }

                    if (GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButton(GvrControllerButton.App))
                {
                    //Load Scene
                    SceneManager.LoadScene(1);
                }
            }
            else
            {
                text.text = "";
                controllerWireframe.enabled = false;
                glow.Stop();
            }
        }
        else
        {
            text.text = "";
            controllerWireframe.enabled = false;
            glow.Stop();

        }
    }
}
