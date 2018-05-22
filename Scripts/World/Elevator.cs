using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    protected ElevatorUpgradeManager e_ElevatorUpgradeManager;
    protected ElevatorOverseerManager e_ElevatorOverseerManager;

    private WorldManager e_WorldManager;
    private DataManager e_DataManager;

    private GUI_Elevator e_Gui = null;

    public int e_FloorIndex = 0;

    public float e_Money = 0;
    protected int e_Level = 1;

    protected float e_UpgradeCost = 480;

    protected float e_Total = 30;

    protected int e_Load = 400;
    protected float e_MovementSpeed = 0.5f;
    protected int e_LoadingSpeed = 150;

    private float e_TravelTime = 2;
    private float e_TransportTime = 2;

    private float e_TravelTimeReset;
    private float e_TransportTimeReset;

    public bool e_bManaged = false;
    private bool e_bManual = false;

    private bool e_bTraveled = false;
    private bool e_bTransported = false;

    private bool e_bFinishedOperation = false;

    private void OnEnable()
    {
        SetInitialReferences();
    }

    private void Start()
    {
        if (e_ElevatorUpgradeManager == null || e_ElevatorOverseerManager == null || e_DataManager == null || e_WorldManager == null)
        {
            SetInitialReferences();
        }
    }

    public int GetFloorIndex()
    {
        return e_FloorIndex;
    }

    public int GetLevel()
    {
        return e_Level;
    }

    public bool GetManagedStatus()
    {
        return e_bManaged;
    }

    public void SetLevel(int level)
    {
        e_Level = level;
    }

    public void SetLoad(int load)
    {
        e_Load = load;
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        e_MovementSpeed = movementSpeed;
    }

    public void SetLoadingSpeed(int loadingSpeed)
    {
        e_LoadingSpeed = loadingSpeed;
    }

    public void SetUpgradeCost(float upgradeCost)
    {
        e_UpgradeCost = upgradeCost;
    }

    public void SetMoney(float money)
    {
        e_Money = money;
        e_Gui.RefreshText(e_Gui.gui_eMoney, e_Money);
    }

    public float GetMoney()
    {
        return e_Money;
    }

    public float GetTotal()
    {
        return e_Total;
    }

    public int GetLoad()
    {
        return e_Load;
    }

    public float GetMovementSpeed()
    {
        return e_MovementSpeed;
    }

    public int GetLoadingSpeed()
    {
        return e_LoadingSpeed;
    }


    public float GetUpgradeCost()
    {
        return e_UpgradeCost;
    }

    public void SetInitialReferences()
    {
        if (e_WorldManager == null)
        {
            e_WorldManager = GameMaster.gm_worldManager;
        }

        if (e_ElevatorUpgradeManager == null)
        {
            e_ElevatorUpgradeManager = GameMaster.gm_ElevatorUpgradeManager;
        }

        if (e_ElevatorOverseerManager == null)
        {
            e_ElevatorOverseerManager = GameMaster.gm_ElevatorOverseerManager;
        }

        if (e_DataManager == null)
        {
            e_DataManager = GameMaster.gm_dataManager;
        }

        e_Gui = GetComponent<GUI_Elevator>();

        e_TravelTimeReset = e_TravelTime;
        e_TransportTimeReset = e_TransportTime;

        CalculateTransportTime();
        CalculateTravelTime();
        CalculateTotal();
    }

    public void CalculateTravelTime()
    {
        //+1 is to ensure the coroutine can always be executed
        e_TravelTime = Mathf.Abs((e_Load - (e_MovementSpeed + e_LoadingSpeed))/e_MovementSpeed  * Time.deltaTime) + 1;
        e_TravelTimeReset = e_TravelTime;
    }

    public void CalculateTransportTime()
    {
        //+1 is to ensure the coroutine can always be executed
        e_TransportTime = Mathf.Abs((e_Load - (e_MovementSpeed + e_LoadingSpeed)) * Time.deltaTime) + 1;
        e_TransportTimeReset = e_TransportTime;
    }

    public void CalculateTotal()
    {
        e_Total = Mathf.Abs(e_Load / (e_TransportTime + e_TravelTime));
    }

    public void AttemptBuff()
    {
        if (e_bManaged)
        {
            e_ElevatorOverseerManager.CallEventAttemptBuff();
        }
    }

    public void AssignOverseer(GameObject elevatorOverseer)
    {
        if(!e_bManaged && e_Level >= 5)
        {
            elevatorOverseer.GetComponent<ElevatorOverseer>().SetManagedElevator();
            e_bManaged = true;
            StartCoroutine(Transport());
        }
    }

    public void UnassignOverseer(GameObject elevatorOverseer)
    {
        elevatorOverseer.GetComponent<ElevatorOverseer>().RemoveManagedElevator();
        e_bManaged = false;
    }

    public void ManualTransport()
    {
        e_bManual = true;
        StartCoroutine(Transport());
    }

    private IEnumerator Transport()
    {
        while (e_bManual || e_bManaged)
        {
            if (!e_bTraveled && !e_bTransported)
            {
                e_TravelTime -= 1 * Time.deltaTime;
                if (e_TravelTime <= 0)
                {
                    e_TravelTime = e_TravelTimeReset;
                    e_bTraveled = true;
                }
            }

            if (e_bTraveled && !e_bTransported)
            {
                e_TransportTime -= 1 * Time.deltaTime;
                if (e_TransportTime <= 0)
                {
                    e_TransportTime = e_TransportTimeReset;
                    e_bTransported = true;
                }
            }

            if (e_bTraveled && e_bTransported)
            {
                e_TravelTime -= 1 * Time.deltaTime;
                if (e_TravelTime <= 0)
                {
                    e_TravelTime = e_TravelTimeReset;
                    e_bFinishedOperation = true;
                    e_bTraveled = false;
                    e_bTransported = false;
                }
            }
            if (e_bFinishedOperation && e_bManaged)
            {
                for(int i = 0; i < GameMaster.instance.gm_mineshafts.Count; i++)
                {
                    if(e_Load <= GameMaster.instance.gm_mineshafts[i].GetComponent<Mineshaft>().GetMoney())
                    {
                        e_Money += GameMaster.instance.gm_mineshafts[i].GetComponent<Mineshaft>().GetMoney();
                        GameMaster.instance.gm_mineshafts[i].GetComponent<Mineshaft>().SetMoney(GameMaster.instance.gm_mineshafts[i].GetComponent<Mineshaft>().GetMoney() - e_Money);
                    }
                }

                e_TravelTime = e_TravelTimeReset;
                e_TransportTime = e_TransportTimeReset;

                e_bFinishedOperation = false;

                e_WorldManager.CallEventActivateWarehouse();

                e_Gui.RefreshText(e_Gui.gui_eMoney, e_Money);

                e_DataManager.CallEventSaveData();
            }
            else if (e_bFinishedOperation && !e_bManaged)
            {
                e_Money += GameMaster.instance.gm_mineshafts[e_FloorIndex].GetComponent<Mineshaft>().GetMoney();

                GameMaster.instance.gm_mineshafts[e_FloorIndex].GetComponent<Mineshaft>().SetMoney
                    (GameMaster.instance.gm_mineshafts[e_FloorIndex].GetComponent<Mineshaft>().GetMoney() - e_Money);

                e_TravelTime = e_TravelTimeReset;
                e_TransportTime = e_TransportTimeReset;

                e_bManual = false;
                e_bFinishedOperation = false;

                e_WorldManager.CallEventActivateWarehouse();

                e_Gui.RefreshText(e_Gui.gui_eMoney, e_Money);

                e_DataManager.CallEventSaveData();
            }
            e_Gui.RefreshText(e_Gui.gui_eTravelTime, e_TravelTime);
            e_Gui.RefreshText(e_Gui.gui_eTransportTime, e_TransportTime);
            yield return null;
        }
    }

    public void Upgrade()
    {
        if (GameMaster.instance.GetCash() >= e_UpgradeCost)
        {

            e_LoadingSpeed += 1;
            e_MovementSpeed += 1f;
            e_Load += 10;

            e_Level += 1;

            if (e_Level >= 5)
            {
                e_Gui.gui_eRequirement.gameObject.SetActive(false);
            }

            if (e_Level % 2 == 0)
            {
                GameMaster.instance.SetSuperCash(GameMaster.instance.GetSuperCash() + 10);
            }

            CalculateTransportTime();
            CalculateTravelTime();
            CalculateTotal();
            e_Gui.RefreshText(e_Gui.gui_eLevel, e_Level);
            GameMaster.instance.SetCash(GameMaster.instance.GetCash() - e_UpgradeCost);

            e_UpgradeCost += Mathf.NextPowerOfTwo(Mathf.RoundToInt(e_UpgradeCost));
        }
    }

    public void RefreshGUI()
    {
        e_Gui.RefreshText(e_Gui.gui_eIndex, e_FloorIndex);
        e_Gui.RefreshText(e_Gui.gui_eLevel, e_Level);
        e_Gui.RefreshText(e_Gui.gui_eMoney, e_Money);
        e_Gui.RefreshText(e_Gui.gui_eTransportTime, e_TransportTime);
        e_Gui.RefreshText(e_Gui.gui_eTravelTime, e_TravelTime);
    }
}
