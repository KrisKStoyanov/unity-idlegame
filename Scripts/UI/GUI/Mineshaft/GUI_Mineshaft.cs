using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Mineshaft : Mineshaft {

    public Text gui_mIndex;
    public Text gui_mMoney;
    public Text gui_mTravelTime;
    public Text gui_mMineTime;
    public Text gui_mLevel;

    public Text gui_mRequirement;

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
        m_MineshaftUpgradeManager.CallEventToggleMenu(gameObject);
    }

    public void ToggleManagementMenu()
    {
        m_MineshaftOverseerManager.CallEventToggleMenu(gameObject);
    }
}
