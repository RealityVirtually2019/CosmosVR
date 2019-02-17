using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameStates {START, UNIVERSEISBIG, UNIVERSEFINISHED, STARS, STARSFINISHED, GAS, GASFINISHED, DARKMATTER, DARKMATTERFINISHED, ALL};

    public static GameStates GameState;

    int secondsToDelay = 3;

    public InGameMusicManager musicManager;
    public PromptTextManager promptTextManager;
    public FilterChangeController filterChangeController;

    //https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameStates.START);
        StartCoroutine(WaitFunction());
        InGameMusicManager.OnClipStopped += musicClipStopped;
        if(PlatformInUse.CurrentPlatform == PlatformInUse.Platform.OCULUSGO)
        {
            OculusGoControllerHandler.OnSwipeRight += IncrementFilter;
        }
        else if(PlatformInUse.CurrentPlatform == PlatformInUse.Platform.DAYDREAM)
        {
            DaydreamControllerHandler.OnSwipeRight += IncrementFilter;

        }
    }

    IEnumerator WaitFunction()
    {
        yield return new WaitForSeconds(secondsToDelay);
        ChangeState(GameStates.UNIVERSEISBIG);
    }

    void musicClipStopped()
    {
        switch(GameState)
        {
            case GameStates.UNIVERSEISBIG:
                ChangeState(GameStates.UNIVERSEFINISHED);
                break;
            case GameStates.STARS:
                ChangeState(GameStates.STARSFINISHED);
                break;
            case GameStates.GAS:
                ChangeState(GameStates.GASFINISHED);
                break;
            case GameStates.DARKMATTER:
                ChangeState(GameStates.DARKMATTERFINISHED);
                break;
        }
    }

    //Change to a different state and call entrance actions
    public void ChangeState(GameStates newState)
    {
        switch(newState)
        {
            case GameStates.START:
                //entrance to start function
                promptTextManager.SetText("Turn your head to look around you");
                promptTextManager.setTextVisible();
                break;
            case GameStates.UNIVERSEISBIG:
                //entrance to universe is big
                musicManager.PlayClip(InGameMusicManager.Clips.TheUniverse);
                break;
            case GameStates.UNIVERSEFINISHED:
                //set text to fly around
                promptTextManager.SetText("Hold trigger to fly around");
                promptTextManager.setTextVisible();
                //allow flight
                break;
            case GameStates.STARS:
                promptTextManager.setTextInivisble();
                musicManager.PlayClip(InGameMusicManager.Clips.Stars);
                break;
            case GameStates.STARSFINISHED:
                promptTextManager.setTextVisible();
                promptTextManager.SetText("Swipe right on the thumbpad to change filter");
                break;
            case GameStates.GAS:
                promptTextManager.setTextInivisble();
                musicManager.PlayClip(InGameMusicManager.Clips.Gas);
                filterChangeController.filterState = FilterChangeController.FilterState.GAS;
                break;
            case GameStates.GASFINISHED:
                promptTextManager.setTextVisible();
                break;
            case GameStates.DARKMATTER:
                promptTextManager.setTextInivisble();
                filterChangeController.filterState = FilterChangeController.FilterState.DARKMATTER;
                musicManager.PlayClip(InGameMusicManager.Clips.DarkMatter);
                break;
            case GameStates.DARKMATTERFINISHED:
                promptTextManager.setTextVisible();
                break;
            case GameStates.ALL:
                promptTextManager.SetText("Thanks for trying out CosmosVR!");
                filterChangeController.StartTrackingButtonPresses();
                break;
            default:
                break;
        }
        GameState = newState;

    }


    //Button Press handlers:
    private void IncrementFilter()
    {
        switch (GameState)
        {
            case GameStates.STARSFINISHED:
                ChangeState(GameStates.GAS);
                break;
            case GameStates.GASFINISHED:
                ChangeState(GameStates.DARKMATTER);
                break;
            case GameStates.DARKMATTERFINISHED:
                ChangeState(GameStates.ALL);
                break;
            default:
                break;
        }
    }


    private void Update()
    {
        if(PlatformInUse.CurrentPlatform == PlatformInUse.Platform.OCULUSGO)
        {
            switch (GameState)
            { 
                case GameStates.UNIVERSEFINISHED:
                    if (OculusGoControllerHandler.triggerIsClicked)
                    {
                        ChangeState(GameStates.STARS);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
