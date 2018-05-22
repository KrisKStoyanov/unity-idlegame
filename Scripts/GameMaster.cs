using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public static GameMaster instance = null;

    public static GeneralManager gm_generalManager;
    public static WorldManager gm_worldManager;
    public static DataManager gm_dataManager;

    public static MineshaftOverseerManager gm_MineshaftOverseerManager;
    public static ElevatorOverseerManager gm_ElevatorOverseerManager;
    public static WarehouseOverseerManager gm_WarehouseOverseerManager;

    public static MineshaftUpgradeManager gm_MineshaftUpgradeManager;
    public static ElevatorUpgradeManager gm_ElevatorUpgradeManager;
    public static WarehouseUpgradeManager gm_WarehouseUpgradeManager;

    public GameObject gm_warehouse;
    public GameObject gm_elevator;

    public List<GameObject> gm_mineshafts = new List<GameObject>();

    public List<GameObject> gm_mineshaftOverseers = new List<GameObject>();
    public List<GameObject> gm_warehouseOverseers = new List<GameObject>();
    public List<GameObject> gm_elevatorOverseers = new List<GameObject>();

    private float gm_cash = 10;
    private float gm_idleCash = 0;
    private int gm_superCash = 0;

    //How many times the player hired a manager (overseer)
    private int gm_moHireCount = 0;
    private int gm_eoHireCount = 0;
    private int gm_woHireCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        gm_generalManager = GetComponent<GeneralManager>();
        gm_worldManager = GetComponent<WorldManager>();
        gm_dataManager = GetComponent<DataManager>();
        gm_MineshaftUpgradeManager = GetComponent<MineshaftUpgradeManager>();
        gm_MineshaftOverseerManager = GetComponent<MineshaftOverseerManager>();
        gm_ElevatorUpgradeManager = GetComponent<ElevatorUpgradeManager>();
        gm_ElevatorOverseerManager = GetComponent<ElevatorOverseerManager>();
        gm_WarehouseUpgradeManager = GetComponent<WarehouseUpgradeManager>();
        gm_WarehouseOverseerManager = GetComponent<WarehouseOverseerManager>();
    }

    private void Start()
    {
        gm_dataManager.CallEventLoadData();
        gm_generalManager.CallEventUpdateCash();
        gm_generalManager.CallEventUpdateIdleCash();
        gm_generalManager.CallEventUpdateSuperCash();
    }

    //Getters
    public float GetCash()
    {
        return gm_cash;
    }

    public float GetIdleCash()
    {
        return gm_idleCash;
    }

    public int GetSuperCash()
    {
        return gm_superCash;
    }

    public int GetHireCountMO()
    {
        return gm_moHireCount;
    }

    public int GetHireCountEO()
    {
        return gm_eoHireCount;
    }

    public int GetHireCountWO()
    {
        return gm_woHireCount;
    }


    //Setters
    public void SetCash(float updatedCash)
    {
        gm_cash = updatedCash;
        gm_generalManager.CallEventUpdateCash();
    }

    public void SetIdleCash(float updatedIdleCash)
    {
        gm_idleCash = updatedIdleCash;
        gm_generalManager.CallEventUpdateIdleCash();
    }

    public void SetSuperCash(int updatedSuperCash)
    {
        gm_superCash = updatedSuperCash;
        gm_generalManager.CallEventUpdateSuperCash();
    }

    public void SetHireCountMO(int moHireCount)
    {
        gm_moHireCount = moHireCount;
    }

    public void SetHireCountEO(int eoHireCount)
    {
        gm_eoHireCount = eoHireCount;
    }

    public void SetHireCountWO(int woHireCount)
    {
        gm_woHireCount = woHireCount;
    }
}