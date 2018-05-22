using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIController : MonoBehaviour {

    //World builder and player action tracker

    private GeneralManager wui_GeneralManager;
    private WorldManager wui_WorldManager;
    private DataManager wui_DataManager;

    public GameObject wui_WarehouseObject;
    public GameObject wui_ElevatorObject;

    public ObjectPool wui_ObjectPool;
    public Transform wui_MineshaftSpawnLocation;
    private float wui_MineshaftSpacing;

    private int wui_MineshaftCashCost = 10;
    private int wui_MineshaftSuperCashCost = 5;

    public Text wui_MineshaftCashCostText;
    public Text wui_MineshaftSuperCashCostText;



    // Use this for initialization
    private void OnEnable() {
        SetInitialReferences();
        {
            wui_WorldManager.EventActivateElevator += Elevator_Activate;
            wui_WorldManager.EventActivateWarehouse += Warehouse_Activate;
            wui_WorldManager.EventUnlockCashMineshaft += Mineshaft_CashAddNew;
            wui_WorldManager.EventUnlockSuperCashMineshaft += Mineshaft_SuperCashAddNew;
        }
    }

    private void OnDisable()
    {
        wui_WorldManager.EventActivateElevator -= Elevator_Activate;
        wui_WorldManager.EventActivateWarehouse -= Warehouse_Activate;
        wui_WorldManager.EventUnlockCashMineshaft -= Mineshaft_CashAddNew;
        wui_WorldManager.EventUnlockSuperCashMineshaft -= Mineshaft_SuperCashAddNew;
    }


    private void SetInitialReferences()
    {
        if(wui_GeneralManager == null)
        {
            wui_GeneralManager = GameMaster.gm_generalManager;
        }
        if(wui_WorldManager == null)
        {
            wui_WorldManager = GameMaster.gm_worldManager;
        }
        if (wui_DataManager == null)
        {
            wui_DataManager = GameMaster.gm_dataManager;
        }

        if (wui_MineshaftSpawnLocation == null && wui_ObjectPool != null)
        {
            wui_MineshaftSpawnLocation = wui_ObjectPool.transform;
        }
        if (wui_ObjectPool.pooledObject != null)
        {
            wui_MineshaftSpacing = wui_ObjectPool.pooledObject.GetComponent<RectTransform>().rect.height/2;
        }

        wui_MineshaftCashCostText.text = "" + wui_MineshaftCashCost;
        wui_MineshaftSuperCashCostText.text = "" + wui_MineshaftSuperCashCost;
    }

    private void Start()
    {
        if (wui_GeneralManager == null || wui_WorldManager == null || wui_DataManager == null || wui_MineshaftSpawnLocation == null)
        {
            SetInitialReferences();
            wui_WorldManager.EventActivateElevator += Elevator_Activate;
            wui_WorldManager.EventActivateWarehouse += Warehouse_Activate;
            wui_WorldManager.EventUnlockCashMineshaft += Mineshaft_CashAddNew;
            wui_WorldManager.EventUnlockSuperCashMineshaft += Mineshaft_SuperCashAddNew;
        }
    }

    public void Mineshaft_CashAddNew()
    {
        if(GameMaster.instance.GetCash() >= wui_MineshaftCashCost)
        {
            GameObject _gameObject = wui_ObjectPool.GetPooledObject();
            _gameObject.transform.position = wui_MineshaftSpawnLocation.transform.position;
            _gameObject.SetActive(true);
            _gameObject.GetComponent<Mineshaft>().SetIndex(GameMaster.instance.gm_mineshafts.Count);
            _gameObject.GetComponent<Mineshaft>().RefreshGUI();
            GameMaster.instance.gm_mineshafts.Add(_gameObject);
            wui_MineshaftSpawnLocation.position = new Vector3(wui_MineshaftSpawnLocation.transform.position.x, wui_MineshaftSpawnLocation.transform.position.y - wui_MineshaftSpacing, wui_MineshaftSpawnLocation.transform.position.z);

            GameMaster.instance.SetCash(GameMaster.instance.GetCash() -  wui_MineshaftCashCost);
            wui_GeneralManager.CallEventUpdateCash();

            wui_MineshaftCashCost += Mathf.RoundToInt(Mathf.Pow(wui_MineshaftCashCost, 2));

            wui_MineshaftCashCostText.text = "" + wui_MineshaftCashCost;
            wui_DataManager.CallEventSaveData();
        }
    }

    public void Mineshaft_SuperCashAddNew()
    {
        if (GameMaster.instance.GetCash() >= wui_MineshaftSuperCashCost)
        {
            GameObject _gameObject = wui_ObjectPool.GetPooledObject();
            _gameObject.transform.position = wui_MineshaftSpawnLocation.transform.position;
            _gameObject.SetActive(true);
            _gameObject.GetComponent<Mineshaft>().RefreshGUI();
            GameMaster.instance.gm_mineshafts.Add(_gameObject);
            wui_MineshaftSpawnLocation.position = new Vector3(wui_MineshaftSpawnLocation.transform.position.x, wui_MineshaftSpawnLocation.transform.position.y - wui_MineshaftSpacing, wui_MineshaftSpawnLocation.transform.position.z);

            GameMaster.instance.SetSuperCash(GameMaster.instance.GetSuperCash() - wui_MineshaftSuperCashCost);
            wui_GeneralManager.CallEventUpdateSuperCash();

            wui_MineshaftSuperCashCost += Mathf.RoundToInt(Mathf.Pow(wui_MineshaftSuperCashCost,2));

            wui_MineshaftSuperCashCostText.text = "" + wui_MineshaftSuperCashCost;
            wui_DataManager.CallEventSaveData();
        }
    }

    public void Elevator_Activate()
    {
        if (wui_ElevatorObject != null)
        {
            wui_ElevatorObject.SetActive(true);
            wui_ElevatorObject.GetComponent<Elevator>().RefreshGUI();
            GameMaster.instance.gm_elevator = wui_ElevatorObject;
        }
    }

    public void Warehouse_Activate()
    {
        if (wui_WarehouseObject != null)
        {
            wui_WarehouseObject.SetActive(true);
            wui_WarehouseObject.GetComponent<Warehouse>().RefreshGUI();
            GameMaster.instance.gm_warehouse = wui_WarehouseObject;
        }
    }
}
