using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Warehouse : Warehouse {

    public Text gui_wTravelTime;
    public Text gui_wCollectTime;
    public Text gui_wLevel;

    public Text gui_wRequirement;

    public void RefreshText(Text target, string sValue = "")
    {
        target.text = "" + sValue;
    }

    public void RefreshText(Text target, float fValue)
    {
        target.text = target.name + ": " + Mathf.RoundToInt(fValue);
    }

    public void RefreshText(Text target, int iValue)
    {
        target.text = target.name + ": " + iValue;
    }

    public void ToggleUpgradesMenu()
    {
        w_WarehouseUpgradeManager.CallEventToggleMenu();
    }

    public void ToggleManagementMenu()
    {
        w_WarehouseOverseerManager.CallEventToggleMenu(gameObject);
    }
}
