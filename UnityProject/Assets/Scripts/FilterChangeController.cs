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
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        particleUIText = GameObject.Find("particleTypeText").GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var device = SteamVR_Controller.Input((int)trackedObject.index);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            //change state
            switch (filterState)
            { 
              case FilterState.STARS:
                    filterState = FilterState.GAS;
                    break;
                case FilterState.GAS:
                    filterState = FilterState.DARKMATTER;
                    break;
                case FilterState.DARKMATTER:
                    filterState = FilterState.ALL;
                    break;
                case FilterState.ALL:
                    filterState = FilterState.STARS;
                    break;
                default:
                    filterState = FilterState.STARS;
                    break;
            }
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

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            SceneManager.LoadScene(2);
        }

    }
}
