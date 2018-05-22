using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseManagementController : MonoBehaviour {

    private WarehouseOverseerManager wo_WarehouseOverseerManager;

    public GameObject wo_Menu;
    public GameObject wo_UnassignedOverseersMenu;

    public int wo_HireCost = 10;
    public Text wo_HireCostText;

    public GameObject wo_OverseerTemplate;

    public Transform wo_AssignedTemplateTransform;
    public Transform wo_UnassignedTemplateTransform;

    private Vector2 wo_DefaultUnassignedSpawnPos;
    private float wo_TemplateSpacing;

    public GameObject wo_AssignedOverseer;
    private GameObject wo_NewOverseer;

    public List<GameObject> wo_UnassignedOverseers = new List<GameObject>();

    private void OnEnable()
    {
        SetInitialReferences();
        if (wo_WarehouseOverseerManager != null)
        {
            wo_WarehouseOverseerManager.EventToggleMenu += ToggleMenu;
            wo_WarehouseOverseerManager.EventHireOverseer += HireOverseer;
            wo_WarehouseOverseerManager.EventSellOverseer += SellOverseer;
            wo_WarehouseOverseerManager.EventAttemptBuff += ToggleBuff;
            wo_WarehouseOverseerManager.EventToggleManagement += ToggleManagement;
        }
    }

    private void OnDisable()
    {
        wo_WarehouseOverseerManager.EventToggleMenu -= ToggleMenu;
        wo_WarehouseOverseerManager.EventHireOverseer -= HireOverseer;
        wo_WarehouseOverseerManager.EventSellOverseer -= SellOverseer;
        wo_WarehouseOverseerManager.EventAttemptBuff -= ToggleBuff;
        wo_WarehouseOverseerManager.EventToggleManagement -= ToggleManagement;

        wo_AssignedOverseer = null;
        wo_NewOverseer = null;
    }

    private void Start()
    {
        if (wo_WarehouseOverseerManager == null)
        {
            SetInitialReferences();
            wo_WarehouseOverseerManager.EventToggleMenu += ToggleMenu;
            wo_WarehouseOverseerManager.EventHireOverseer += HireOverseer;
            wo_WarehouseOverseerManager.EventSellOverseer += SellOverseer;
            wo_WarehouseOverseerManager.EventAttemptBuff += ToggleBuff;
            wo_WarehouseOverseerManager.EventToggleManagement += ToggleManagement;
        }
    }

    private void SetInitialReferences()
    {
        if (wo_WarehouseOverseerManager == null)
        {
            wo_WarehouseOverseerManager = GameMaster.gm_WarehouseOverseerManager;
        }
        if (wo_OverseerTemplate != null)
        {
            wo_TemplateSpacing = wo_OverseerTemplate.GetComponent<RectTransform>().rect.height;
        }
        wo_DefaultUnassignedSpawnPos = wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition;
    }

    public void ToggleMenu(GameObject managementTarget)
    {
        if (!wo_Menu.activeInHierarchy)
        {
            wo_Menu.SetActive(true);
            RefreshMenu(managementTarget);
        }
        else
        {
            wo_Menu.SetActive(false);
        }
    }

    public void RefreshMenu(GameObject managementTarget)
    {
        if (wo_UnassignedOverseers.Count > 0)
        {
            wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = wo_DefaultUnassignedSpawnPos;
            for (int i = 0; i < wo_UnassignedOverseers.Count; i++)
            {
                wo_UnassignedOverseers[i].transform.position = wo_UnassignedTemplateTransform.position;
                wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.x, wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.y - wo_TemplateSpacing);
                wo_UnassignedOverseers[i].GetComponent<WarehouseOverseer>().RefreshOverseerGUI();
            }
        }
        wo_HireCostText.text = wo_HireCostText.name + ": " + Mathf.ClosestPowerOfTwo(GameMaster.instance.GetHireCountWO() * wo_HireCost);
    }

    public void HireOverseer(GameObject managementTarget)
    {
        if (GameMaster.instance.GetCash() >= wo_HireCost)
        {
            CreateOverseer(managementTarget);
            GameMaster.instance.SetCash(GameMaster.instance.GetCash() - wo_HireCost);
            wo_HireCost = Mathf.ClosestPowerOfTwo(GameMaster.instance.GetHireCountMO() * wo_HireCost);
        }
    }

    private void CreateOverseer(GameObject managementTarget)
    {
        if (!managementTarget.GetComponent<Warehouse>().GetManagedStatus() && managementTarget.GetComponent<Warehouse>().GetLevel() >= 5)
        {
            wo_AssignedOverseer = Instantiate(wo_OverseerTemplate, wo_AssignedTemplateTransform.position, Quaternion.identity, wo_Menu.transform);
            wo_AssignedOverseer.transform.position = wo_AssignedTemplateTransform.position;
            wo_AssignedOverseer.GetComponent<WarehouseOverseer>().Generate();

            managementTarget.GetComponent<Warehouse>().AssignOverseer(wo_AssignedOverseer);
            GameMaster.instance.gm_warehouseOverseers.Add(wo_AssignedOverseer);
            GameMaster.instance.SetHireCountWO(GameMaster.instance.GetHireCountWO() + 1);
        }
        else if (managementTarget.GetComponent<Warehouse>().GetLevel() >= 5)
        {
            wo_NewOverseer = Instantiate(wo_OverseerTemplate, wo_UnassignedTemplateTransform.position, Quaternion.identity, wo_UnassignedOverseersMenu.transform);
            wo_NewOverseer.transform.position = wo_UnassignedTemplateTransform.position;
            wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.x, wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.y - wo_TemplateSpacing);
            wo_NewOverseer.GetComponent<WarehouseOverseer>().Generate();

            wo_UnassignedOverseers.Add(wo_NewOverseer);
            GameMaster.instance.gm_warehouseOverseers.Add(wo_NewOverseer);
            GameMaster.instance.SetHireCountWO(GameMaster.instance.GetHireCountWO() + 1);
            RefreshMenu(managementTarget);
        }       
    }

    public void AssignOverseer(GameObject managementTarget)
    {
        if (wo_AssignedOverseer != null)
        {
            //Put assigned one down
            managementTarget.GetComponent<Warehouse>().UnassignOverseer(wo_AssignedOverseer);
            wo_NewOverseer = Instantiate(wo_AssignedOverseer, wo_UnassignedTemplateTransform.position, Quaternion.identity, wo_UnassignedOverseersMenu.transform);
            wo_NewOverseer.transform.position = wo_UnassignedTemplateTransform.position;
            wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.x, wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.y - wo_TemplateSpacing);
            wo_UnassignedOverseers.Add(wo_NewOverseer);
            wo_NewOverseer.GetComponent<WarehouseOverseer>().RefreshOverseerGUI();
            GameMaster.instance.gm_warehouseOverseers.Remove(wo_AssignedOverseer);
            Destroy(wo_AssignedOverseer);
            GameMaster.instance.gm_warehouseOverseers.TrimExcess();

            //Move event caller up
            managementTarget.GetComponent<Warehouse>().AssignOverseer(wo_WarehouseOverseerManager.GetEventCaller());
            wo_AssignedOverseer = Instantiate(wo_WarehouseOverseerManager.GetEventCaller(), wo_UnassignedTemplateTransform.position, Quaternion.identity, wo_Menu.transform);
            wo_AssignedOverseer.transform.position = wo_AssignedTemplateTransform.position;
            wo_UnassignedOverseers.Remove(wo_WarehouseOverseerManager.GetEventCaller());
            wo_AssignedOverseer.GetComponent<WarehouseOverseer>().RefreshOverseerGUI();

            GameMaster.instance.gm_warehouseOverseers.Remove(wo_WarehouseOverseerManager.GetEventCaller());
            Destroy(wo_WarehouseOverseerManager.GetEventCaller());
            GameMaster.instance.gm_warehouseOverseers.TrimExcess();
        }
        else
        {
            wo_AssignedOverseer = Instantiate(wo_WarehouseOverseerManager.GetEventCaller(), wo_AssignedTemplateTransform.position, Quaternion.identity, wo_Menu.transform);
            wo_AssignedOverseer.transform.position = wo_AssignedTemplateTransform.position;

            wo_UnassignedOverseers.Remove(wo_WarehouseOverseerManager.GetEventCaller());
            managementTarget.GetComponent<Warehouse>().AssignOverseer(wo_AssignedOverseer);
            wo_AssignedOverseer.GetComponent<WarehouseOverseer>().RefreshOverseerGUI();

            GameMaster.instance.gm_warehouseOverseers.Remove(wo_WarehouseOverseerManager.GetEventCaller());
            Destroy(wo_WarehouseOverseerManager.GetEventCaller());
            GameMaster.instance.gm_warehouseOverseers.TrimExcess();
        }

        RefreshMenu(managementTarget);

    }

    public void UnassignOverseer(GameObject managementTarget)
    {
        if (wo_WarehouseOverseerManager.GetEventCaller() == wo_AssignedOverseer)
        {
            managementTarget.GetComponent<Warehouse>().UnassignOverseer(wo_AssignedOverseer);

            //Create a new copy of it
            wo_NewOverseer = Instantiate(wo_AssignedOverseer, wo_UnassignedTemplateTransform.position, Quaternion.identity, wo_UnassignedOverseersMenu.transform);
            wo_NewOverseer.transform.position = wo_UnassignedTemplateTransform.position;
            wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.x, wo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.y - wo_TemplateSpacing);

            //Add it to the unassigned group
            wo_UnassignedOverseers.Add(wo_NewOverseer);
            wo_NewOverseer.GetComponent<WarehouseOverseer>().RefreshOverseerGUI();

            //Destroy the old one
            GameMaster.instance.gm_warehouseOverseers.Remove(wo_AssignedOverseer);
            Destroy(wo_AssignedOverseer);
            GameMaster.instance.gm_warehouseOverseers.TrimExcess();

            RefreshMenu(managementTarget);
        }
    }


    public void SellOverseer(GameObject managementTarget)
    {
        if (wo_WarehouseOverseerManager.GetEventCaller() != wo_AssignedOverseer)
        {

            wo_WarehouseOverseerManager.GetEventCaller().GetComponent<WarehouseOverseer>().RefundValue();

            wo_UnassignedOverseers.Remove(wo_WarehouseOverseerManager.GetEventCaller());

            GameMaster.instance.gm_warehouseOverseers.Remove(wo_WarehouseOverseerManager.GetEventCaller());
            Destroy(wo_WarehouseOverseerManager.GetEventCaller());
            GameMaster.instance.gm_warehouseOverseers.TrimExcess();

            RefreshMenu(managementTarget);
        }
    }

    public void ToggleBuff(GameObject managementTarget)
    {
        wo_WarehouseOverseerManager.GetEventCaller().GetComponent<WarehouseOverseer>().ToggleBuff();
    }

    public void ToggleManagement(GameObject managementTarget)
    {
        if (!managementTarget.GetComponent<Warehouse>().GetManagedStatus() || wo_WarehouseOverseerManager.GetEventCaller() != wo_AssignedOverseer)
        {
            AssignOverseer(managementTarget);
        }
        else
        {
            UnassignOverseer(managementTarget);
        }
    }
}
