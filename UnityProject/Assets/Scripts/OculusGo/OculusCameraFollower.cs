using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusCameraFollower : MonoBehaviour
{

    public GameObject oculusCamera;

    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = oculusCamera.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = oculusCamera.transform.position + offset;
    }
}
