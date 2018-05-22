using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Interactive : MonoBehaviour {

    private GeneralManager ui_generalManager;

    public GameObject ui_menu;
    public GameObject ui_map;
    public GameObject ui_gameCurrencyShop;
    public GameObject ui_microtransactions;

    private void OnEnable()
    {
        SetInitialReferences();
        if (ui_generalManager != null)
        {
            ui_generalManager.EventToggleMenu += ToggleMenu;
        }
    }

    private void OnDisable()
    {
        ui_generalManager.EventToggleMenu -= ToggleMenu;
    }

    private void Start()
    {
        if(ui_generalManager == null)
        {
            SetInitialReferences();
            ui_generalManager.EventToggleMenu += ToggleMenu;
        }
    }

    private void SetInitialReferences()
    {
        if (ui_generalManager == null)
        {
            ui_generalManager = GameMaster.gm_generalManager;
        }
    }

    protected void ToggleMenu()
    {
        if (!ui_menu.activeInHierarchy)
        {
            ui_menu.SetActive(true);
        }
        else
        {
            ui_menu.SetActive(false);
        }
    }
}
