using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Noninteractive : MonoBehaviour {

    private GeneralManager ui_generalManager;

    public Text ui_cash;
    public Text ui_idleCash;
    public Text ui_superCash;

    void OnEnable()
    {
        SetInitialReferences();
        if (ui_generalManager != null)
        {
            ui_generalManager.EventUpdateCash += RefreshCash;
            ui_generalManager.EventUpdateIdleCash += RefreshIdleCash;
            ui_generalManager.EventUpdateSuperCash += RefreshSuperCash;
        }
    }

    void OnDisable()
    {
        ui_generalManager.EventUpdateCash -= RefreshCash;
        ui_generalManager.EventUpdateIdleCash -= RefreshIdleCash;
        ui_generalManager.EventUpdateSuperCash -= RefreshSuperCash;
    }

    private void Start()
    {
        if (ui_generalManager == null)
        {
            SetInitialReferences();
            ui_generalManager.EventUpdateCash += RefreshCash;
            ui_generalManager.EventUpdateIdleCash += RefreshIdleCash;
            ui_generalManager.EventUpdateSuperCash += RefreshSuperCash;
        }
    }

    private void SetInitialReferences()
    {
        if (ui_generalManager == null)
        {
            ui_generalManager = GameMaster.gm_generalManager;
        }
    }

    protected void Refresh(Text ui_text, float value)
    {
        ui_text.text = ui_text.name + ": " + value;
    }

    protected void RefreshCash()
    {
        Refresh(ui_cash, GameMaster.instance.GetCash());
    }

    protected void RefreshIdleCash()
    {
        Refresh(ui_idleCash, GameMaster.instance.GetIdleCash());
    }

    protected void RefreshSuperCash()
    {
        Refresh(ui_superCash, GameMaster.instance.GetSuperCash());
    }
}
