using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMusicManager : MonoBehaviour
{
    public AudioSource soundBiteSource;
    public AudioClip cosmicWeb;
    public AudioClip stars;
    public AudioClip gas;
    public AudioClip darkMatter;
    public AudioClip theUniverse;
    
    public enum ClipState { STARTED, STOPPED};
    public ClipState clipState;

    public delegate void ClipStopped();
    public static event ClipStopped OnClipStopped;

    public delegate void ClipStarted();
    public static event ClipStarted OnClipStarted;


    //https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html
    private void Awake()
    {
        clipState = ClipState.STOPPED;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


    public enum Clips { TheUniverse, Stars, Gas, DarkMatter, CosmicWeb };
    public void PlayClip(Clips clip)
    {
        switch(clip)
        {
            case Clips.CosmicWeb:
                soundBiteSource.clip = cosmicWeb;
                break;
            case Clips.Stars:
                soundBiteSource.clip = stars;
                break;
            case Clips.Gas:
                soundBiteSource.clip = gas;
                break;
            case Clips.DarkMatter:
                soundBiteSource.clip = darkMatter;
                break;
            case Clips.TheUniverse:
                soundBiteSource.clip = theUniverse;
                break;
            default:
                Debug.Log("clip does not exist");
                break;
        }
        soundBiteSource.Play();
    }

    private void Update()
    {
        switch(clipState)
        {
            case ClipState.STARTED:
                if(!soundBiteSource.isPlaying)
                {
                    clipState = ClipState.STOPPED;
                    //fire event that clip ended
                    if (OnClipStopped != null)
                        OnClipStopped();
                }
                break;
            case ClipState.STOPPED:
                if(soundBiteSource.isPlaying)
                {
                    clipState = ClipState.STARTED;
                    //Fire event clip started
                    if (OnClipStarted != null)
                        OnClipStarted();
                }
                break;
        }
    }
}
