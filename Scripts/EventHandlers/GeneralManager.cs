using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : MonoBehaviour {

    public delegate void GeneralEventHandler();

    //Noninteractive GUI Events
    public event GeneralEventHandler EventUpdateCash;
    public event GeneralEventHandler EventUpdateIdleCash;
    public event GeneralEventHandler EventUpdateSuperCash;

    //Interactive GUI Events
    public event GeneralEventHandler EventToggleMenu;

    //Attempt to call events through its designated delegated if they have any assigned listening functions
    public void CallEventUpdateCash()
    {
        if (EventUpdateCash != null)
        {
            EventUpdateCash();
        }
    }

    public void CallEventUpdateIdleCash()
    {
        if (EventUpdateIdleCash != null)
        {
            EventUpdateIdleCash();
        }
    }

    public void CallEventUpdateSuperCash()
    {
        if (EventUpdateSuperCash != null)
        {
            EventUpdateSuperCash();
        }
    }


    public void CallEventToggleMenu()
    {
        if (EventToggleMenu != null)
        {
            EventToggleMenu();
        }
    }

}
