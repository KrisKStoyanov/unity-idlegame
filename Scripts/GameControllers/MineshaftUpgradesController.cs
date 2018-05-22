using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineshaftUpgradesController : MonoBehaviour {

    private MineshaftUpgradeManager mu_MineshaftUpgradeManager;

    public GameObject mu_Menu;

    public Text mu_Index;
    public Text mu_Level;

    public Text mu_Total;
    public Text mu_Miners;
    public Text mu_WalkingSpeed;
    public Text mu_MiningSpeed;
    public Text mu_WorkerCapacity;

    public Text mu_UpgradeCost;

    private void OnEnable()
    {
        SetInitialReferences();
        if (mu_MineshaftUpgradeManager != null)
        {
            mu_MineshaftUpgradeManager.EventToggleMenu += ToggleMenu;
            mu_MineshaftUpgradeManager.EventHandleUpgrade += UpgradeLevel;
        }
    }

    private void OnDisable()
    {
        mu_MineshaftUpgradeManager.EventToggleMenu -= ToggleMenu;
        mu_MineshaftUpgradeManager.EventHandleUpgrade -= UpgradeLevel;
    }

    private void Start()
    {
        if (mu_MineshaftUpgradeManager == null)
        {
            SetInitialReferences();
            mu_MineshaftUpgradeManager.EventToggleMenu += ToggleMenu;
            mu_MineshaftUpgradeManager.EventHandleUpgrade += UpgradeLevel;
        }
    }

    private void SetInitialReferences()
    {
        if (mu_MineshaftUpgradeManager == null)
        {
            mu_MineshaftUpgradeManager = GameMaster.gm_MineshaftUpgradeManager;
        }
    }

    public void ToggleMenu(GameObject upgradeTarget)
    {
        if (!mu_Menu.activeInHierarchy)
        {
            mu_Menu.SetActive(true);
            RefreshMenu(upgradeTarget);
        }
        else
        {
            mu_Menu.SetActive(false);
        }
    }

    public void RefreshMenu(GameObject upgradeTarget)
    {
        mu_Index.text = mu_Index.name + ": " + upgradeTarget.GetComponent<Mineshaft>().GetIndex();
        mu_Level.text = mu_Level.name + ": " + upgradeTarget.GetComponent<Mineshaft>().GetLevel();

        mu_Total.text = mu_Total.name + ": " + upgradeTarget.GetComponent<Mineshaft>().GetTotal();
        mu_Miners.text = mu_Miners.name + ": " + upgradeTarget.GetComponent<Mineshaft>().GetMiners();
        mu_WalkingSpeed.text = mu_WalkingSpeed.name + ": " + upgradeTarget.GetComponent<Mineshaft>().GetWalkingSpeed();
        mu_MiningSpeed.text = mu_MiningSpeed.name + ": " + upgradeTarget.GetComponent<Mineshaft>().GetMiningSpeed();
        mu_WorkerCapacity.text = mu_WorkerCapacity.name + ": " + upgradeTarget.GetComponent<Mineshaft>().GetWorkerCapacity();

        mu_UpgradeCost.text = mu_UpgradeCost.name + ": " + upgradeTarget.GetComponent<Mineshaft>().GetUpgradeCost();
    }

    public void UpgradeLevel(GameObject upgradeTarget)
    {
        upgradeTarget.GetComponent<Mineshaft>().Upgrade();
        RefreshMenu(upgradeTarget);
    }
}
