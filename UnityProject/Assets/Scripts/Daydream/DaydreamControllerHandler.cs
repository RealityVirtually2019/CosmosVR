using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaydreamControllerHandler : MonoBehaviour
{
    public static bool touchpadIsClicked;
    public static bool touchpadIsTouched;
    public static bool appIsClicked;
    public static Vector2 touchPos;

    public delegate void SwipeLeft();
    public static event SwipeLeft OnSwipeLeft;

    public delegate void SwipeRight();
    public static event SwipeRight OnSwipeRight;

    public delegate void AppClicked();
    public static event AppClicked OnAppClicked;


    public enum SwipeState {NOTSTARTED, INPROGRESS};
    public SwipeState swipeState;
    Vector2 initialSwipePos;
    Vector2 endingSwipePos;
    

    Vector2 defaultTouchPos;

    private void Awake()
    {
        PlatformInUse.CurrentPlatform = PlatformInUse.Platform.DAYDREAM;
    }

    private void Start()
    {
        defaultTouchPos = new Vector2(0.0f, 0.0f);
        initialSwipePos = new Vector2(0.0f, 0.0f);
        endingSwipePos = new Vector2(0.0f, 0.0f);

    }

    // Update is called once per frame
    void Update()
    {
        //click and holds
        if (GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButton(GvrControllerButton.App))
        {
            //Load Scene
            appIsClicked = true;
        }
        else
        {
            appIsClicked = false;
        }

        if (GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButton(GvrControllerButton.TouchPadButton))
        {
            //Load Scene
            touchpadIsClicked = true;
        }
        else
        {
            touchpadIsClicked = false;
        }

        if (GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButton(GvrControllerButton.TouchPadTouch))
        {
            //Load Scene
            touchpadIsTouched = true;
            touchPos = GvrControllerInput.GetDevice(GvrControllerHand.Dominant).TouchPos;
        }
        else
        {
            touchpadIsTouched = false;
            touchPos = defaultTouchPos;
        }

        //click events
        if (GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButtonDown(GvrControllerButton.App))
        {
            if (OnAppClicked != null)
                OnAppClicked();
        }

        //swipe events
        CheckForSwipe();
    }

    void CheckForSwipe()
    {
        //Debug.Log(GvrControllerInput.GetDevice(GvrControllerHand.Dominant).TouchPos);
        switch (swipeState)
        {
            case SwipeState.NOTSTARTED:
                if(GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButton(GvrControllerButton.TouchPadTouch)) 
                {
                    swipeState = SwipeState.INPROGRESS;
                    initialSwipePos = GvrControllerInput.GetDevice(GvrControllerHand.Dominant).TouchPos;
                }
                break;
            case SwipeState.INPROGRESS:
                if (!GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButton(GvrControllerButton.TouchPadTouch))
                {
                    swipeState = SwipeState.NOTSTARTED;
                    endingSwipePos = GvrControllerInput.GetDevice(GvrControllerHand.Dominant).TouchPos;

                    if((initialSwipePos.x - endingSwipePos.x) > 1.0f)
                    {
                        if (OnSwipeLeft != null)
                            OnSwipeLeft();
                    }
                    if ((initialSwipePos.x - endingSwipePos.x) < -1.0f)
                    {
                        if (OnSwipeRight != null)
                            OnSwipeRight();
                    }

                }
                break;
        }
    }
}
