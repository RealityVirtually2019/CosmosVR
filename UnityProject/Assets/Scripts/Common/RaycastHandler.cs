using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastHandler : MonoBehaviour
{
    public Camera cam;

    public GameObject galaxy;

    public GameObject crossHair;

    public Text hudDescriptionText;


    //Color 
    Color notSelected = new Color(0.2f, 0.5f, 0.7f, 0.6f);
    Color selected = new Color(0.0f, 0.7f, 0.0f, 0.9f);

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            crossHair.GetComponent<Image>().color = selected;
            if(hit.transform.tag == "Galaxy")
            {
                hudDescriptionText.text = "Galaxy";
            }
            else
            {
                crossHair.GetComponent<Image>().color = notSelected;
                hudDescriptionText.text = "";
            }
        }
        else
        {
            crossHair.GetComponent<Image>().color = notSelected;
            hudDescriptionText.text = "";

        }
    }
}
