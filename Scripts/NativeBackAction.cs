using UnityEngine;
using UnityEngine.UI;
using Extensions;
using System;

public class NativeBackAction : MonoBehaviour
{
    [Space(10)]
    [SerializeField] private Button[] backButtons;

    [Space]
    [SerializeField] private GameObject[] OverlayPanels;

    private Func<bool> inpIdentifier;

    //swipe vars
    private Vector2 startTouchPosition;
    private Vector2 currentPosition;
    private Vector2 endTouchPosition;
    private bool stopTouch = false;
    public float swipeRange = 100;

    private void Awake()
    {
#if UNITY_ANDROID
        inpIdentifier = () => Input.GetKeyDown(KeyCode.Escape);
#endif
#if UNITY_IOS
        inpIdentifier = () => false;//Swipe();
#endif
    }
    void Update()
    {
        if (inpIdentifier() && !isOverlayEnabled())
        {
            DoFirstEnabledBack();
        }
       
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //    Debug.Log("Swipped? " + Swipe());
#if UNITY_IOS
        Swipe();
#endif
    }
    private void DoFirstEnabledBack()
    {
        if (backButtons.Length > 0)
        {
            for (int i = 0; i < backButtons.Length; i++)
            {
                if (backButtons[i].IsActiveInHierachyAndInteractable())
                {
                    backButtons[i].onClick.Invoke();
                    return;
                }
            }

        }
    }
    private bool isOverlayEnabled()
    {
        bool isOverlayEnabled = false;
        if(OverlayPanels.Length > 0)
        {
            for(int i = 0; i < OverlayPanels.Length; i++)
            {
                isOverlayEnabled = isOverlayEnabled || OverlayPanels[i].activeInHierarchy;
                if(isOverlayEnabled)
                {
                    break;
                }
            }
            return isOverlayEnabled;
        }
        else
        {
            return isOverlayEnabled;
        }
    }
    public bool Swipe()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentPosition = Input.GetTouch(0).position;
            Vector2 Distance = currentPosition - startTouchPosition;

            if (!stopTouch)
            {

                //if (Distance.x < -swipeRange)
                //{
                //    Debug.Log("Swipped Left");
                //    //outputText.text = "Left";
                //    stopTouch = true;
                //    return false;
                //}
                //else
                if (Distance.x > swipeRange)
                {
                    // outputText.text = "Right";
                    Debug.Log("Swipped Right");
                    // DoFirstEnabledBack();
                    inpIdentifier = () => false;
                    stopTouch = true;
                    return true;
                }
                //else if (Distance.y > swipeRange)
                //{
                //    // outputText.text = "Up";
                //    stopTouch = true;
                //    return false;
                //}
                //else if (Distance.y < -swipeRange)
                //{
                //    // outputText.text = "Down";
                //    stopTouch = true;
                //    return false;
                //}
            }
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stopTouch = false;
            endTouchPosition = Input.GetTouch(0).position;
        }

        return false;
    }
}
