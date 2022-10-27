using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    private const float DEADZONE = 100.0f;
    public static MobileInput instance { set; get; }

    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private Vector2 swipeDelta, startTouch;

    public bool Tap { get { return tap; } }
    public bool SwipeLeft { get { return swipeLeft;  } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public Vector2 StartTouch { get { return startTouch; } }

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Reset all the boolean
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        // Let check for inputs
        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            startTouch = Input.mousePosition;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }
        #endregion
        #region Mobile Inputs
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouch = Input.mousePosition;
            }

            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch = swipeDelta = Vector2.zero;
            }

        }

        
        #endregion

        // Calculate distance
        swipeDelta = Vector2.zero;
        if (startTouch != Vector2.zero)
        {
            //Check for mobile inputs
            if (Input.touches.Length != 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }

            // Check for standalone inputs
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        // Check if swipe is beyound the deadzone
        if (swipeDelta.magnitude > DEADZONE)
        {
            // This is a comfirmed swipe
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Left or Right
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }

            else
            {
                // Top or down
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }

            startTouch = swipeDelta = Vector2.zero;
        }
    }
}
