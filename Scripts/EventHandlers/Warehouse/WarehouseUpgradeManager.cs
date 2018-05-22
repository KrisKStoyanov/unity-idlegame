using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseUpgradeManager : MonoBehaviour {

    public delegate void WarehouseUpgradeEventHandler();

    public event WarehouseUpgradeEventHandler EventToggleMenu;
    public event WarehouseUpgradeEventHandler EventHandleUpgrade;

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
