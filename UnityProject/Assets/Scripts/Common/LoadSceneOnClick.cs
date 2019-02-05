using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public int SceneToLoad = 0;
    public void LoadScene()
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    private void Start()
    {
        if (PlatformInUse.CurrentPlatform == PlatformInUse.Platform.OCULUSGO)
        {
            OculusGoControllerHandler.OnTouchpadClicked += LoadScene;
        }
    }

    private void OnDestroy()
    {
        if (PlatformInUse.CurrentPlatform == PlatformInUse.Platform.OCULUSGO)
        {
            OculusGoControllerHandler.OnTouchpadClicked -= LoadScene;
        }
    }
}
