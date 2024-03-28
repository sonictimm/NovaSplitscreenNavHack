using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Nova;

public class NovaSplitscreenButton : MonoBehaviour, ISplitscreenPlayerFocusable
{
    public int PlayerIndex = -1;

    //If true, will Automatically "Press" the button as soon as Focus arrives here.
    public bool AutoSelect = false;

    public UnityEvent OnFocused = null;


    ///Interface Implementation
    public int GetPlayerIndex() { return PlayerIndex; }

    public void SetPlayerIndex(int inPlayerIdx)
    {
        PlayerIndex = inPlayerIdx;
    }

    ///Called when the player navigates to our UI Block
    public void HandlePlayerFocus()
    {
        NovaUIFocusIndicator_ButtonComponent focus = GetComponent<NovaUIFocusIndicator_ButtonComponent>();

        if (focus != null)
        {
            focus.SetFocusedState(true);
        }

        if (OnFocused != null)
        {
            OnFocused.Invoke();
        }

        if (AutoSelect)
        {
            HandlePlayerSelect();
        }
    }
    ///Called when the player navigates away from our UI Block
    public void HandlePlayerUnfocus()
    {
        NovaUIFocusIndicator_ButtonComponent focus = GetComponent<NovaUIFocusIndicator_ButtonComponent>();

        if (focus != null)
        {
            focus.SetFocusedState(false);
        }
    }
        
    ///Called when the player Selects our block
    public void HandlePlayerSelect()
    {
        NovaSamples.UIControls.Button btn = GetComponent<NovaSamples.UIControls.Button>();

        if (btn != null)
        {
            if (btn.OnClicked != null)
            {
                btn.OnClicked.Invoke();
            }    
        }
    }

    ///Called when the player presses cancel while our block is focused
    public void HandlePlayerCancel()
    {
        //idk, feels like it should be managed by some larger multi-menu mgr
        return;
    }

    ///End Interface Impl.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
