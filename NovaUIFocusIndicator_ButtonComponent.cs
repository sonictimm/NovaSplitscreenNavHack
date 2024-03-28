using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nova;

public class NovaUIFocusIndicator_ButtonComponent : MonoBehaviour
{
    public UIBlock MyFocusIndicator;

    private Interactable MyInteractable;

    private int MyPlayerIndex = 0;
    
    void Awake()
    {
        MyInteractable = GetComponent<Interactable>();

        if (MyInteractable != null
            && MyFocusIndicator != null)
        {
            Navigation.OnNavigationFocusChanged += HandleNavigationFocusChanged;
        }

        ISplitscreenPlayerFocusable focusable = MyInteractable.gameObject.GetComponent<ISplitscreenPlayerFocusable>();
        if (focusable != null)
        {
            MyPlayerIndex = focusable.GetPlayerIndex();
        }
        MyFocusIndicator.Visible = false;

    }

    private void Start()
    {
    }

    //Unfortunately this is called on EVERY focusable thing all the time.
    void HandleNavigationFocusChanged(uint controlID, UIBlock focused)
    {
        if (MyInteractable == null
            || controlID != MyPlayerIndex)
        {
            return;
        }

        //Navigation.TryGetFocusedUIBlock(controlID, out focused);
        //Debug.Log("Focus Change called on " + this.name);
        if (focused == GetComponent<UIBlock>())
        {
            SetFocusedState(true);
        }
        else
        {
        }
    }

    public void SetFocusedState(bool inFocused)
    {
        MyFocusIndicator.Visible = inFocused;
    }
        
}
