using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Overseer : MonoBehaviour {

    //Can replace GUI_Mineshaft with it on the Mienshaft Prefab
    public Text gui_Rank;
    public Text gui_BuffIndex;
    public Text gui_BuffInfo;

    public Button gui_ManageButton;
    public Button gui_BuffButton;
    public Button gui_SellButton;

    public void RefreshText(Text target, string sValue)
    {
        target.text = "" + sValue;
    }

    public void RefreshText(Text target, float fValue)
    {
        target.text = target.name + ": " + fValue;
    }

    public void RefreshText(Text target, int iValue)
    {
        target.text = target.name + ": " + iValue;
    }
}
