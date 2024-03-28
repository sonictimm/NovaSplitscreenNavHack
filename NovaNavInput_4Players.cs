using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nova;
using Rewired;

/** 
 * Allows Rewired to work with Nova UI navigation, Focus, & Selection
 * See DBBButton_AnyPlayer for the "AnyPlayer" version.
 * */
public class NovaNavInput_4Players : MonoBehaviour
{
    public bool UICategoryLoadAtStart = false;

    protected IList<Player> Players;
    // Start is called before the first frame update
    void Start()
    {
        RegisterPlayers();
        
    }


    /** Detect and register all valid players */
    public void RegisterPlayers()
    {
        Players = ReInput.players.AllPlayers;
        foreach (Player anyPlayer in Players)
        {
            if (anyPlayer.name != "System")
            {
                anyPlayer.AddInputEventDelegate(HandleNavDown, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "NavDown");
                anyPlayer.AddInputEventDelegate(HandleNavUp, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "NavUp");
                anyPlayer.AddInputEventDelegate(HandleNavRight, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "NavRight");
                anyPlayer.AddInputEventDelegate(HandleNavLeft, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "NavLeft");

                anyPlayer.AddInputEventDelegate(HandleNavConfirm, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Confirm");
                anyPlayer.AddInputEventDelegate(HandleNavCancel, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Cancel");

                if (UICategoryLoadAtStart)
                {
                    anyPlayer.controllers.maps.LoadMap(ControllerType.Joystick, anyPlayer.id, "UI", "Default", true);
                    anyPlayer.controllers.maps.LoadMap(ControllerType.Keyboard, anyPlayer.id, "UI", "Default", true);
                }
            }    
        }

        //TODO I think these need to be de-registered too, unsure tho.
    }

    private void OnDestroy()
    {
        Players = ReInput.players.AllPlayers;
        foreach (Player anyPlayer in Players)
        {
            if (anyPlayer.name != "System")
            {
                // Unsubscribe from events when object is destroyed
                anyPlayer.RemoveInputEventDelegate(HandleNavDown);
                anyPlayer.RemoveInputEventDelegate(HandleNavUp);
                anyPlayer.RemoveInputEventDelegate(HandleNavRight);
                anyPlayer.RemoveInputEventDelegate(HandleNavLeft);

                anyPlayer.RemoveInputEventDelegate(HandleNavConfirm);
                anyPlayer.RemoveInputEventDelegate(HandleNavCancel);
            }
        }
    }

    protected void HandleNavDown(InputActionEventData data)
    {
        //Debug.Log("NavDown by " + data.playerId);
        //Navigation.Move(Vector3.down, (uint)data.playerId);
        if (NovaNavHacker.Instance != null)
        {
            NovaNavHacker.Instance.AttemptNav(Vector3.down, data.playerId);
        }
    }

    protected void HandleNavUp(InputActionEventData data)
    {
        //Debug.Log("NavUp");
        //Navigation.Move(Vector3.up, (uint)data.playerId);
        if (NovaNavHacker.Instance != null)
        {
            NovaNavHacker.Instance.AttemptNav(Vector3.up, data.playerId);
        }
    }


    protected void HandleNavRight(InputActionEventData data)
    {
        //Debug.Log("NavRight");
        //Navigation.Move(Vector3.right, (uint)data.playerId);
        if (NovaNavHacker.Instance != null)
        {
            NovaNavHacker.Instance.AttemptNav(Vector3.right, data.playerId);
        }
    }


    protected void HandleNavLeft(InputActionEventData data)
    {
        //Debug.Log("NavLeft");
        //Navigation.Move(Vector3.left, (uint)data.playerId);
        if (NovaNavHacker.Instance != null)
        {
            NovaNavHacker.Instance.AttemptNav(Vector3.left, data.playerId);
        }
    }

    protected void HandleNavConfirm(InputActionEventData data)
    {
        //Debug.Log("Nav CONFIRM");
        //Navigation.Select((uint)data.playerId);
        if (NovaNavHacker.Instance != null)
        {
            NovaNavHacker.Instance.SelectFocused(data.playerId);
        }
    }

    protected void HandleNavCancel(InputActionEventData data)
    {
        //Debug.Log("Nav Cancel");
        if (NovaNavHacker.Instance != null)
        {
            NovaNavHacker.Instance.CancelFocused(data.playerId);
        }
    }
}
