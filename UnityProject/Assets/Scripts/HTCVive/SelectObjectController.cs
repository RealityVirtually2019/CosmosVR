using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectObjectController : MonoBehaviour
{

    TextMesh text;
    public SteamVR_TrackedObject rightController;




    // Start is called before the first frame update
    void Start()
    {
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
                text.text = "Press the trigger to put on the headset";

                var rightDevice = SteamVR_Controller.Input((int)rightController.index);
                var triggerPressure_r = rightDevice.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
                if (rightDevice.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                {
                    //Load Scene
                    SceneManager.LoadScene(1);
                }
            }
            else
            {
                text.text = "";
            }
        }
        else
        {
            text.text = "";
        }
    }
}
