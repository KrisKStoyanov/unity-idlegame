using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorUpgradeManager : MonoBehaviour {

    public delegate void ElevatorUpgradeEventHandler();

    public event ElevatorUpgradeEventHandler EventToggleMenu;
    public event ElevatorUpgradeEventHandler EventHandleUpgrade;

    public void CallEventToggleMenu()
    {
        if (EventToggleMenu != null)
        {
            EventToggleMenu();
        }
    }



    public void CallEventHandleUpgrade()
    {
        if (EventHandleUpgrade != null)
        {
            EventHandleUpgrade();
        }
    }
}
