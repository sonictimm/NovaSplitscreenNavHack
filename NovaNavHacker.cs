using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nova;

/**
 * The goal of this class is to hack Nova's focus system to support four player splitscreen
 * While there is support for things like multi-touch, allowing the system to know WHICH input device focused something,
 * There seems to be only one focus at a time ever.
 * I need four things focused for my game,
 * So I hack that together.
 * */
public class NovaNavHacker : MonoBehaviour
{

    public static NovaNavHacker Instance;

    static public void SetPlayerIndexOnChildren(GameObject inObject, int inPlayerIdx, bool includeSelf = true)
    {
        List<ISplitscreenPlayerFocusable> childFocusables = new List<ISplitscreenPlayerFocusable>();

        //add all children of object
        Component[] compsOnChildren = inObject.GetComponentsInChildren(typeof(ISplitscreenPlayerFocusable));

        foreach(Component comp in compsOnChildren)
        {
            ISplitscreenPlayerFocusable foc = (ISplitscreenPlayerFocusable)comp.gameObject.GetComponent(typeof(ISplitscreenPlayerFocusable));
            if (foc != null)
            {
                childFocusables.Add(foc);
            }
        }    

        //childFocusables.AddRange((ISplitscreenPlayerFocusable[])inObject.GetComponentsInChildren(typeof(ISplitscreenPlayerFocusable)));

        //Add Object if it has one
        if (includeSelf)
        {
            ISplitscreenPlayerFocusable parent = (ISplitscreenPlayerFocusable)inObject.GetComponent(typeof(ISplitscreenPlayerFocusable));
            if (parent != null)
            {
                childFocusables.Add(parent);
            }
        }

        //Now that we got 'em all, do it.

        foreach (ISplitscreenPlayerFocusable foc in childFocusables)
        {
            foc.SetPlayerIndex(inPlayerIdx);
        }

    }


    //What blocks should we focus on to start.  Array items can be null, but NEVER REMOVE THEM!  Must keep 4 items.
    public GestureRecognizer[] InitialFocus = new GestureRecognizer[4];

    protected GestureRecognizer[] CurrentFocus = new GestureRecognizer[4];

    //System player is zero and 1-4 are real players??? Since I'm using rewired???
    //Seems sketchy.  
    public const int MAX_PLAYERS = 4;

    //For things with focus logic, either use a Component or an Interface to make functions
    //to be called when FakeFocus OR realFocus happens.

    //Consider even having a comp[onent for UIBlocks that restricts which player idx can focus it, so that players never hop focus to another player's blocks.

    private void Awake()
    {
        CurrentFocus = new GestureRecognizer[4];

        if (Instance != null)
        {
            Debug.LogWarning("New NovaNavHacker is taking over, even tho one is in place!");
        }    

        Instance = this;

        //Set initial focus
        ResetFocus();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public GestureRecognizer GetFocus(int inPlayer)
    {
        if (inPlayer >= MAX_PLAYERS)
        {
            Debug.LogWarning("Cannot get focus for player number " + inPlayer);
        }

        return CurrentFocus[inPlayer];

    }

    ///Set the fake focus to a certain target for a certain player
    public void SetFocus(GestureRecognizer inFocused, int inPlayer = 0)
    {
        //unfocus current one
        if (CurrentFocus[inPlayer] != null)
        {
            ISplitscreenPlayerFocusable old = (ISplitscreenPlayerFocusable)CurrentFocus[inPlayer].GetComponent(typeof(ISplitscreenPlayerFocusable));

            if (old != null)
            {
                old.HandlePlayerUnfocus();
            }
        }


        /**
         * This supposedly works
         * Transform targetT = Instantiate(targetPrefab[i], RandomSpawnGen(), transform.rotation) as Transform;
            iTarget targetScript = (iTarget)targetT.GetComponent(typeof(iTarget));
        https://forum.unity.com/threads/getcomponent-type-with-an-interface.104044/
            TRY IT!
         * */

        CurrentFocus[inPlayer] = inFocused; //may be null

        if (inFocused != null)
        {
            //focus new one and save it
            ISplitscreenPlayerFocusable newFocus = (ISplitscreenPlayerFocusable)inFocused.GetComponent(typeof(ISplitscreenPlayerFocusable));

            if (newFocus != null)
            {
                newFocus.HandlePlayerFocus();
            }
            Debug.Log("Player " + inPlayer + " focused on " + inFocused);
        }
        
    }

    ///Resets focus for each player to initial focus block
    public void ResetFocus()
    {
        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            if (InitialFocus.Length > i)
            {
                SetFocus(InitialFocus[i], i);
            }
        }
    }

    public void AttemptNav(Vector3 inDir, int inPlayer = 0)
    {

        //Tie into Nova's systems to CHECK if there's something navigable from A to B in that direction
        //If so, move our fake focus for this player to that place, and call any relevant funcs in fakefocus interface or comp.


        if (GetFocus(inPlayer) == null)
        {
            //Debug.Log("Player is trying to navigate, but has no focus yet!  Player " + inPlayer);
            return;
        }

        GestureRecognizer nextFocus = null;

        nextFocus = GetFocus(inPlayer).Navigation.GetLink(inDir).Target;

        if (nextFocus == null)
        {
            Debug.Log("No target set in that nav dir");
            //nextFocus = GetFocus(inPlayer).Navigation.GetLink(inDir).Auto;
        }

        /*
        if (inDir == Vector3.up)
        {
            nextFocus = GetFocus(inPlayer).Navigation.Up.Target;
        }
        else if (inDir == Vector3.down)
        {
             Down.Target;
        }
        else if (inDir == Vector3.left)
        {
            nextFocus = GetFocus(inPlayer).Navigation.Left.Target;
        }
        else if (inDir == Vector3.right)
        {
            nextFocus = GetFocus(inPlayer).Navigation.Right.Target;
        }//*/

        if (nextFocus != null)
        {
            ISplitscreenPlayerFocusable next = (ISplitscreenPlayerFocusable)nextFocus.GetComponent(typeof(ISplitscreenPlayerFocusable));
            if (next != null
                && nextFocus.isActiveAndEnabled)
            {
                if (next.GetPlayerIndex() == inPlayer)
                {
                    SetFocus(nextFocus, inPlayer);
                }
            }
        }

        //Maybe make another func: AttemptNavAction for Confirm-Cancel-Cat6egoryChange.
    }

    public void SelectFocused (int inPlayer)
    {
        if (CurrentFocus.Length <= inPlayer)
        {
            Debug.Log("Tried selecting UI on player idx that no exist");
            return;
        }

        ISplitscreenPlayerFocusable focus = (ISplitscreenPlayerFocusable)CurrentFocus[inPlayer].GetComponent(typeof(ISplitscreenPlayerFocusable));

        if (focus != null)
        {
            focus.HandlePlayerSelect();
        }

    }

    public void CancelFocused(int inPlayer)
    {
        if (CurrentFocus.Length <= inPlayer)
        {
            Debug.Log("Tried cancel UI on player idx that no exist");
            return;
        }

        ISplitscreenPlayerFocusable focus = CurrentFocus[inPlayer] as ISplitscreenPlayerFocusable;

        if (focus != null)
        {
            focus.HandlePlayerCancel();
        }

    }

}
