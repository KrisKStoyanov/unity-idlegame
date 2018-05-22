using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : MonoBehaviour {

    protected WarehouseUpgradeManager w_WarehouseUpgradeManager;
    protected WarehouseOverseerManager w_WarehouseOverseerManager;

    private WorldManager w_WorldManager;
    private DataManager w_DataManager;

    private GUI_Warehouse w_Gui = null;

    protected int w_Level = 1;

    protected float w_UpgradeCost = 480;

    protected float w_Total = 55.6f;

    protected int w_Transporters = 1;
    protected int w_LoadPerTransporter = 1000;
    protected int w_LoadingSpeed = 250;
    protected float w_WalkingSpeed = 2;

    private float w_CollectTime = 2;
    private float w_TravelTime = 2;

    private float w_TravelTimeReset;
    private float w_CollectTimeReset;

    protected bool w_bManaged = false;
    protected bool w_bManual = false;

    private bool w_bTraveled = false;
    private bool w_bCollected = false;

    private bool w_bFinishedOperation = false;

    //Prevent button spamming to accelerate coroutine speed
    private bool w_bCoroutineRunning = false;

    private void OnEnable()
    {
        SetInitialReferences();
    }

    private void Start()
    {
        if (w_WarehouseUpgradeManager == null || w_WarehouseOverseerManager == null || w_WorldManager == null || w_DataManager == null)
        {
            SetInitialReferences();
        }
    }

    private void SetInitialReferences()
    {
        if (w_WarehouseUpgradeManager == null)
        {
            w_WarehouseUpgradeManager = GameMaster.gm_WarehouseUpgradeManager;
        }
        if(w_DataManager == null)
        {
            w_WarehouseOverseerManager = GameMaster.gm_WarehouseOverseerManager;
        }
        if(w_WorldManager == null)
        {
            w_WorldManager = GameMaster.gm_worldManager;
        }
        if(w_DataManager == null)
        {
            w_DataManager = GameMaster.gm_dataManager;
        }

        w_Gui = gameObject.GetComponent<GUI_Warehouse>();

        w_TravelTimeReset = w_TravelTime;
        w_CollectTimeReset = w_CollectTime;

        CalculateCollectTime();
        CalculateTravelTime();
        CalculateTotal();
    }

    public int GetLevel()
    {
        return w_Level;
    }

    public bool GetManagedStatus()
    {
        return w_bManaged;
    }

    public void SetLevel(int level)
    {
        w_Level = level;
    }

    public void SetTransporters(int transporters)
    {
        w_Transporters = transporters;
    }

    public void SetWalkingSpeed(float walkingSpeed)
    {
        w_WalkingSpeed = walkingSpeed;
    }

    public void SetLoadingSpeed(int loadingSpeed)
    {
        w_LoadingSpeed = loadingSpeed;
    }

    public void SetLoadPerTransporter(int loadPerTransporter)
    {
        w_LoadPerTransporter = loadPerTransporter;
    }

    public void SetUpgradeCost(float upgradeCost)
    {
        w_UpgradeCost = upgradeCost;
    }

    public float GetTotal()
    {
        return w_Total;
    }

    public int GetTransporters()
    {
        return w_Transporters;
    }

    public float GetWalkingSpeed()
    {
        return w_WalkingSpeed;
    }

    public int GetLoadingSpeed()
    {
        return w_LoadingSpeed;
    }

    public int GetLoadPerTransporter()
    {
        return w_LoadPerTransporter;
    }

    public float GetUpgradeCost()
    {
        return w_UpgradeCost;
    }

    public void CalculateCollectTime()
    {
        //+1 is to ensure the coroutine can always be executed
        w_CollectTime = Mathf.Abs(w_LoadPerTransporter/w_LoadingSpeed) + (w_Transporters *Time.deltaTime) + 1;
        w_CollectTimeReset = w_CollectTime;
    }

    public void CalculateTravelTime()
    {
        //+1 is to ensure the coroutine can always be executed
        w_TravelTime = Mathf.Abs(w_WalkingSpeed/w_Transporters) + (w_WalkingSpeed * Time.deltaTime) + 1;
        w_TravelTimeReset = w_TravelTime;
    }

    public void CalculateTotal()
    {
        w_Total = Mathf.Abs((w_Transporters * w_LoadPerTransporter) / (w_CollectTime + w_TravelTime));
    }

    public void AttemptBuff()
    {
        if (w_bManaged)
        {
            w_WarehouseOverseerManager.CallEventAttemptBuff();
        }
    }

    public void AssignOverseer(GameObject warehouseOverseer)
    {
        warehouseOverseer.GetComponent<WarehouseOverseer>().SetManagedWarehouse();
        w_bManaged = true;
        if (!w_bCoroutineRunning)
        {
            StartCoroutine(Collect());
        }

        GameMaster.instance.SetIdleCash(w_Total);
    }

    public void UnassignOverseer(GameObject warehouseOverseer)
    {
        warehouseOverseer.GetComponent<WarehouseOverseer>().RemoveManagedWarehouse();
        w_bManaged = false;
    }

    public void ManualCollect()
    {
        if(!w_bManual && !w_bCoroutineRunning)
        {
            w_bManual = true;
            StartCoroutine(Collect());
        }
    }

    private IEnumerator Collect()
    {
        while (w_bManual || w_bManaged)
        {
            w_bCoroutineRunning = true;
            if (!w_bTraveled && !w_bCollected)
            {
                w_TravelTime -= 1 * Time.deltaTime;
                if (w_TravelTime <= 0)
                {
                    w_TravelTime = w_TravelTimeReset;
                    w_bTraveled = true;
                }
            }

            if (w_bTraveled && !w_bCollected)
            {
                w_CollectTime -= 1 * Time.deltaTime;
                if (w_CollectTime <= 0)
                {
                    w_CollectTime = w_CollectTimeReset;
                    w_bCollected = true;
                }
            }

            if (w_bTraveled && w_bCollected)
            {
                w_TravelTime -= 1 * Time.deltaTime;
                if (w_TravelTime <= 0)
                {
                    w_TravelTime = w_TravelTimeReset;
                    w_bFinishedOperation = true;
                    w_bTraveled = false;
                    w_bCollected = false;
                }
            }
            if (w_bFinishedOperation && w_bManaged)
            {
                GameMaster.instance.SetCash(GameMaster.instance.GetCash() + GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMoney());
                GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMoney(0);

                w_TravelTime = w_TravelTimeReset;
                w_CollectTime = w_CollectTimeReset;

                w_bFinishedOperation = false;

                w_DataManager.CallEventSaveData();

                w_bCoroutineRunning = false;
            }
            else if (w_bFinishedOperation && !w_bManaged)
            {
                GameMaster.instance.SetCash(GameMaster.instance.GetCash() + GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMoney());
                GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMoney(0);

                w_TravelTime = w_TravelTimeReset;
                w_CollectTime = w_CollectTimeReset;
                w_bManual = false;

                w_bFinishedOperation = false;

                w_DataManager.CallEventSaveData();

                w_bCoroutineRunning = false;
            }
            w_Gui.RefreshText(w_Gui.gui_wTravelTime, w_TravelTime);
            w_Gui.RefreshText(w_Gui.gui_wCollectTime, w_CollectTime);
            yield return null;
        }
    }

    public void Upgrade()
    {
        if (GameMaster.instance.GetCash() >= w_UpgradeCost)
        {
            w_CollectTime += 1f;
            w_Transporters += 1;
            w_WalkingSpeed += 1f;
            w_LoadPerTransporter += 10;

            w_Level += 1;

            if (w_Level >= 5)
            {
                w_Gui.gui_wRequirement.gameObject.SetActive(false);
            }

            if (w_Level % 2 == 0)
            {
                GameMaster.instance.SetSuperCash(GameMaster.instance.GetSuperCash() + 10);
            }

            CalculateCollectTime();
            CalculateTravelTime();
            CalculateTotal();
            w_Gui.RefreshText(w_Gui.gui_wLevel, w_Level);
            GameMaster.instance.SetCash(GameMaster.instance.GetCash() - w_UpgradeCost);

            w_UpgradeCost += Mathf.NextPowerOfTwo(Mathf.RoundToInt(w_UpgradeCost));
        }
    }

    public void RefreshGUI()
    {
        w_Gui.RefreshText(w_Gui.gui_wLevel, w_Level);
        w_Gui.RefreshText(w_Gui.gui_wCollectTime, w_CollectTime);
        w_Gui.RefreshText(w_Gui.gui_wTravelTime, w_TravelTime);
    }
}
