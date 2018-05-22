using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorUpgradesController : MonoBehaviour {

    private ElevatorUpgradeManager eu_ElevatorUpgradeManager;

    public GameObject eu_Menu;

    public Text eu_Index;
    public Text eu_Level;

    public Text eu_Total;
    public Text eu_Load;
    public Text eu_MovementSpeed;
    public Text eu_LoadingSpeed;

    public Text eu_UpgradeCost;

    private void OnEnable()
    {
        SetInitialReferences();
        if (eu_ElevatorUpgradeManager != null)
        {
            eu_ElevatorUpgradeManager.EventToggleMenu += ToggleMenu;
            eu_ElevatorUpgradeManager.EventHandleUpgrade += UpgradeLevel;
        }
    }

    private void OnDisable()
    {
        eu_ElevatorUpgradeManager.EventToggleMenu -= ToggleMenu;
        eu_ElevatorUpgradeManager.EventHandleUpgrade -= UpgradeLevel;
    }

    private void Start()
    {
        if (eu_ElevatorUpgradeManager == null)
        {
            SetInitialReferences();
            eu_ElevatorUpgradeManager.EventToggleMenu += ToggleMenu;
            eu_ElevatorUpgradeManager.EventHandleUpgrade += UpgradeLevel;
        }
    }

    private void SetInitialReferences()
    {
        if (eu_ElevatorUpgradeManager == null)
        {
            eu_ElevatorUpgradeManager = GameMaster.gm_ElevatorUpgradeManager;
        }
    }

    public void ToggleMenu()
    {
        if (!eu_Menu.activeInHierarchy)
        {
            eu_Menu.SetActive(true);
            RefreshMenu();
        }
        else
        {
            eu_Menu.SetActive(false);
        }
    }

    public void RefreshMenu()
    {
        eu_Index.text = eu_Index.name + ": " + GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetFloorIndex();
        eu_Level.text = eu_Level.name + ": " + GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetLevel();

        eu_Total.text = eu_Total.name + ": " + GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetTotal();
        eu_Load.text = eu_Total.name + ": " + GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetTotal();
        eu_MovementSpeed.text = eu_MovementSpeed.name + ": " + GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed();
        eu_LoadingSpeed.text = eu_LoadingSpeed.name + ": " + GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetLoadingSpeed();

        eu_UpgradeCost.text = eu_UpgradeCost.name + ": " + GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetUpgradeCost();
    }

    public void UpgradeLevel()
    {
        GameMaster.instance.gm_elevator.GetComponent<Elevator>().Upgrade();
        RefreshMenu();
    }
}
