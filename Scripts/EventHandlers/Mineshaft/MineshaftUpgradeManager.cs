using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineshaftUpgradeManager : MonoBehaviour {

    public delegate void MineshaftUpgradeEventHandler(GameObject upgradeTarget);

    private GameObject eventTarget;

    public event MineshaftUpgradeEventHandler EventToggleMenu;
    public event MineshaftUpgradeEventHandler EventHandleUpgrade;

    public void CallEventToggleMenu(GameObject upgradeTarget)
    {
        if (EventToggleMenu != null)
        {
            eventTarget = upgradeTarget;
            EventToggleMenu(eventTarget);
        }
    }


    public void CallEventHandleUpgrade()
    {
        if (EventHandleUpgrade != null)
        {
            EventHandleUpgrade(eventTarget);
        }
    }

}
