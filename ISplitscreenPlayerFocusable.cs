using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nova;

public interface ISplitscreenPlayerFocusable
{
    ///The player who is allowed to focus and navigate over this UI Block
    public int GetPlayerIndex();

    public void SetPlayerIndex(int inPlayerIdx);
    
    ///Get the UI Block we're focusing on
    //public UIBlock GetUIBlock();  //Not gonna need that AFAIK

    ///Called when the player navigates to our UI Block
    public void HandlePlayerFocus();
    ///Called when the player navigates away from our UI Block
    public void HandlePlayerUnfocus();
    ///Called when the player Selects our block
    public void HandlePlayerSelect();
    ///Called when the player presses cancel while our block is focused
    public void HandlePlayerCancel();
}
