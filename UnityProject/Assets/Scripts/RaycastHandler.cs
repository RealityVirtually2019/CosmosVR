using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastHandler : MonoBehaviour
{
    public Camera cam;

    public GameObject galaxy;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.transform.tag == "Galaxy")
            {
                galaxy.SetActive(true);
                galaxy.GetComponent<TextMesh>().text = "Galaxy is visible";
            }
        }
        else
        {
            // galaxy.SetActive(false);
            galaxy.GetComponent<TextMesh>().text = "";

        }
    }
}
