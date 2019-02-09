using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMusicManager : MonoBehaviour
{
    //https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
