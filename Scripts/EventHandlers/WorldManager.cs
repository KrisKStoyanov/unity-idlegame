using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    public delegate void WorldEventHandler();

    public event WorldEventHandler EventUnlockCashMineshaft;
    public event WorldEventHandler EventUnlockSuperCashMineshaft;
    public event WorldEventHandler EventActivateElevator;
    public event WorldEventHandler EventActivateWarehouse;

    public void CallEventUnlockCashMineshaft()
    {
        if (EventUnlockCashMineshaft != null)
        {
            EventUnlockCashMineshaft();
        }
    }

    public void CallEventUnlockSuperCashMineshaft()
    {
        if (EventUnlockSuperCashMineshaft != null)
        {
            EventUnlockSuperCashMineshaft();
        }
    }

    public void CallEventActivateElevator()
    {
        if(EventActivateElevator != null)
        {
            EventActivateElevator();
        }
    }

    public void CallEventActivateWarehouse()
    {
        if (EventActivateWarehouse != null)
        {
            EventActivateWarehouse();
        }
    }
}
