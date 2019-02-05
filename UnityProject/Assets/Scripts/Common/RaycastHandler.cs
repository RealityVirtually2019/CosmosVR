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
    Color notSelected = new Color(0.6089801f, 0.7054334f, 0.8018868f, 0.1176471f);
    Color selected = new Color(0.3607919f, 0.8207547f, 0.3523051f, 0.2352941f);
    // Start is called before the first frame update
    void Start()
    {
        hudDescriptionText = GameObject.Find("HudDescriptionText").GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            crossHair.GetComponent<RawImage>().color = selected;
            if(hit.transform.tag == "Galaxy")
            {
                hudDescriptionText.text = "Galaxy";
            }
        }
        else
        {
            crossHair.GetComponent<RawImage>().color = notSelected;
            hudDescriptionText.text = "";

        }
    }
}
