using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusGoControllerHandler : MonoBehaviour
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

    public delegate void LeftClicked();
    public static event LeftClicked OnLeftClicked;

    public delegate void RightClicked();
    public static event RightClicked OnRightClicked;


    public enum SwipeState {NOTSTARTED, INPROGRESS};
    SwipeState swipeState;
    Vector2 initialSwipePos;
    Vector2 endingSwipePos;
    

    Vector2 defaultTouchPos;

    private void Awake()
    {
        PlatformInUse.CurrentPlatform = PlatformInUse.Platform.OCULUSGO;
        swipeState = SwipeState.NOTSTARTED;
    }

    private void Start()
    {
        defaultTouchPos = new Vector2(0.0f, 0.0f);
        initialSwipePos = new Vector2(0.0f, 0.0f);
        endingSwipePos = new Vector2(0.0f, 0.0f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        OVRInput.Update();

        //click events
        if (OVRInput.GetDown(OVRInput.Button.Back))
        {
            if (OnAppClicked != null)
                OnAppClicked();
        }
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if(OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x > 0.4f)
            {
                if (OnRightClicked != null)
                    OnRightClicked();
            }
            if (OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x < -0.4f)
            {
                if (OnLeftClicked != null)
                    OnLeftClicked();
            }
        }

        //swipe events
        CheckForSwipe();

        //click and holds
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            //Load Scene
            appIsClicked = true;
        }
        else
        {
            appIsClicked = false;
        }

        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            //Load Scene
            touchpadIsClicked = true;
        }
        else
        {
            touchpadIsClicked = false;
        }

        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
        {
            //Load Scene
            touchpadIsTouched = true;
            touchPos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        }
        else
        {
            touchpadIsTouched = false;
            touchPos = defaultTouchPos;
        }


    }

    void CheckForSwipe()
    { 
        switch (swipeState)
        {
            case SwipeState.NOTSTARTED:
                if(OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) 
                {
                    swipeState = SwipeState.INPROGRESS;
                    initialSwipePos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
                }
                break;
            case SwipeState.INPROGRESS:
                if (!OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
                {
                    swipeState = SwipeState.NOTSTARTED;
                    endingSwipePos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

                    if((initialSwipePos.x - endingSwipePos.x) > 0.5f)
                    {
                        if (OnSwipeLeft != null)
                            OnSwipeLeft();
                    }
                    if ((initialSwipePos.x - endingSwipePos.x) < -0.5f)
                    {
                        if (OnSwipeRight != null)
                            OnSwipeRight();
                    }
                }
                break;
        }
    }
}
