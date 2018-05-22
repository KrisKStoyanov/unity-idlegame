using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Elevator : Elevator {

    public Text gui_eIndex;
    public Text gui_eMoney;
    public Text gui_eTravelTime;
    public Text gui_eTransportTime;
    public Text gui_eLevel;

    public Text gui_eRequirement;

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
        e_ElevatorUpgradeManager.CallEventToggleMenu();
    }

    public void ToggleManagementMenu()
    {
        e_ElevatorOverseerManager.CallEventToggleMenu(gameObject);
    }
}
