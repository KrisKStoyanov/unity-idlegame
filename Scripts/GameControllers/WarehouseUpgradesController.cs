using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseUpgradesController : MonoBehaviour {

    private WarehouseUpgradeManager wu_WarehouseUpgradeManager;

    public GameObject wu_Menu;

    public Text wu_Level;

    public Text wu_Total;
    public Text wu_Transporters;
    public Text wu_LoadPerTransporter;
    public Text wu_WalkingSpeed;
    public Text wu_LoadingSpeed;

    public Text wu_UpgradeCost;

    private void OnEnable()
    {
        SetInitialReferences();
        if (wu_WarehouseUpgradeManager != null)
        {
            wu_WarehouseUpgradeManager.EventToggleMenu += ToggleMenu;
            wu_WarehouseUpgradeManager.EventHandleUpgrade += UpgradeLevel;
        }
    }

    private void OnDisable()
    {
        wu_WarehouseUpgradeManager.EventToggleMenu -= ToggleMenu;
        wu_WarehouseUpgradeManager.EventHandleUpgrade -= UpgradeLevel;
    }

    private void Start()
    {
        if (wu_WarehouseUpgradeManager == null)
        {
            SetInitialReferences();
            wu_WarehouseUpgradeManager.EventToggleMenu += ToggleMenu;
            wu_WarehouseUpgradeManager.EventHandleUpgrade += UpgradeLevel;
        }
    }

    private void SetInitialReferences()
    {
        if (wu_WarehouseUpgradeManager == null)
        {
            wu_WarehouseUpgradeManager = GameMaster.gm_WarehouseUpgradeManager;
        }
    }

    public void ToggleMenu()
    {
        if (!wu_Menu.activeInHierarchy)
        {
            wu_Menu.SetActive(true);
            RefreshMenu();
        }
        else
        {
            wu_Menu.SetActive(false);
        }
    }

    public void RefreshMenu()
    {
        wu_Level.text = wu_Level.name + ": " + GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetLevel();

        wu_Total.text = wu_Total.name + ": " + GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetTotal();
        wu_Transporters.text = wu_Transporters.name + ": " + GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetTransporters();
        wu_LoadPerTransporter.text = wu_Level.name + ": " + GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetLoadPerTransporter();
        wu_WalkingSpeed.text = wu_Level.name + ": " + GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed();
        wu_LoadingSpeed.text = wu_Level.name + ": " + GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetLoadingSpeed();

        wu_UpgradeCost.text = wu_UpgradeCost.name + ": " + GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetUpgradeCost();
    }

    public void UpgradeLevel()
    {
        GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().Upgrade();
        RefreshMenu();
    }
}
