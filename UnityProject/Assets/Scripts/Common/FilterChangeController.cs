using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FilterChangeController : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObject;
    public GameObject stars;
    public GameObject gas;
    public GameObject darkMatter;
    Text particleUIText;

    public enum FilterState { STARS,GAS,DARKMATTER,ALL};
    public FilterState filterState;

    // Start is called before the first frame update
    void Start()
    {
        filterState = FilterState.STARS;

        particleUIText = GameObject.Find("particleTypeText").GetComponent<Text>();

        if (PlatformInUse.CurrentPlatform == PlatformInUse.Platform.DAYDREAM)
        {
            DaydreamControllerHandler.OnSwipeRight += IncrementState;
            DaydreamControllerHandler.OnSwipeLeft += DecrementState;
            DaydreamControllerHandler.OnAppClicked += ChangeScene;
        }
        else if (PlatformInUse.CurrentPlatform == PlatformInUse.Platform.OCULUSGO)
        {
            OculusGoControllerHandler.OnSwipeRight += IncrementState;
            OculusGoControllerHandler.OnSwipeLeft += DecrementState;
            OculusGoControllerHandler.OnAppClicked += ChangeScene;
        }
        else if (PlatformInUse.CurrentPlatform == PlatformInUse.Platform.VIVE)
        {
            trackedObject = GetComponent<SteamVR_TrackedObject>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlatformInUse.CurrentPlatform == PlatformInUse.Platform.VIVE)
        {
            HandleViveInput();
        }

        switch (filterState)
        {
            case FilterState.STARS:
                stars.SetActive(true);
                gas.SetActive(false);
                darkMatter.SetActive(false);
                particleUIText.text = "Star Brightness";
                break;
            case FilterState.GAS:
                stars.SetActive(false);
                gas.SetActive(true);
                darkMatter.SetActive(false);
                particleUIText.text = "Gas Density";
                break;
            case FilterState.DARKMATTER:
                stars.SetActive(false);
                gas.SetActive(false);
                darkMatter.SetActive(true);
                particleUIText.text = "Dark Matter Density";
                break;
            case FilterState.ALL:
                stars.SetActive(true);
                gas.SetActive(true);
                darkMatter.SetActive(true);
                particleUIText.text = "Star + Gas + DM";
                break;
            default:
                break;
        }
    }

    void HandleViveInput()
    {
        var device = SteamVR_Controller.Input((int)trackedObject.index);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            //change state
            IncrementState();
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            ChangeScene();
        }
    }

    void ChangeScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 1)
            SceneManager.LoadScene(2);
        else if (sceneIndex == 2)
            SceneManager.LoadScene(1);
    }

    void IncrementState()
    {
        int numStates = System.Enum.GetValues(typeof(FilterState)).Length;

        int currentState = (int)filterState;
        currentState++;

        if(currentState >= numStates)
        {
            currentState = 0;
        }

        filterState = (FilterState)currentState;
    }

    void DecrementState()
    {
        int numStates = System.Enum.GetValues(typeof(FilterState)).Length;

        int currentState = (int)filterState;
        currentState--;

        if (currentState < 0)
        {
            currentState = numStates - 1;
        }

        filterState = (FilterState)currentState;
    }
}
