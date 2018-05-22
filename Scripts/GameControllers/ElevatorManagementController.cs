using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorManagementController : MonoBehaviour {

    private ElevatorOverseerManager eo_ElevatorOverseerManager;

    public GameObject eo_Menu;
    public GameObject eo_UnassignedOverseersMenu;

    public int eo_HireCost = 10;
    public Text eo_HireCostText;

    public GameObject eo_OverseerTemplate;

    public Transform eo_AssignedTemplateTransform;
    public Transform eo_UnassignedTemplateTransform;

    private Vector2 eo_DefaultUnassignedSpawnPos;
    private float eo_TemplateSpacing;

    public GameObject eo_AssignedOverseer;
    private GameObject eo_NewOverseer;

    public List<GameObject> eo_UnassignedOverseers = new List<GameObject>();

    private void OnEnable()
    {
        SetInitialReferences();
        if (eo_ElevatorOverseerManager != null)
        {
            eo_ElevatorOverseerManager.EventToggleMenu += ToggleMenu;
            eo_ElevatorOverseerManager.EventHireOverseer += HireOverseer;
            eo_ElevatorOverseerManager.EventSellOverseer += SellOverseer;
            eo_ElevatorOverseerManager.EventAttemptBuff += ToggleBuff;
            eo_ElevatorOverseerManager.EventToggleManagement += ToggleManagement;
        }
    }

    private void OnDisable()
    {
        eo_ElevatorOverseerManager.EventToggleMenu -= ToggleMenu;
        eo_ElevatorOverseerManager.EventHireOverseer -= HireOverseer;
        eo_ElevatorOverseerManager.EventSellOverseer -= SellOverseer;
        eo_ElevatorOverseerManager.EventAttemptBuff -= ToggleBuff;
        eo_ElevatorOverseerManager.EventToggleManagement -= ToggleManagement;

        eo_AssignedOverseer = null;
        eo_NewOverseer = null;
    }

    private void Start()
    {
        if (eo_ElevatorOverseerManager == null)
        {
            SetInitialReferences();
            eo_ElevatorOverseerManager.EventToggleMenu += ToggleMenu;
            eo_ElevatorOverseerManager.EventHireOverseer += HireOverseer;
            eo_ElevatorOverseerManager.EventSellOverseer += SellOverseer;
            eo_ElevatorOverseerManager.EventAttemptBuff += ToggleBuff;
            eo_ElevatorOverseerManager.EventToggleManagement += ToggleManagement;
        }
    }

    private void SetInitialReferences()
    {
        if (eo_ElevatorOverseerManager == null)
        {
            eo_ElevatorOverseerManager = GameMaster.gm_ElevatorOverseerManager;
        }
        if (eo_OverseerTemplate != null)
        {
            eo_TemplateSpacing = eo_OverseerTemplate.GetComponent<RectTransform>().rect.height;
        }
        eo_DefaultUnassignedSpawnPos = eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition;
    }

    public void ToggleMenu(GameObject managementTarget)
    {
        if (!eo_Menu.activeInHierarchy)
        {
            eo_Menu.SetActive(true);
            RefreshMenu(managementTarget);
        }
        else
        {
            eo_Menu.SetActive(false);
        }
    }

    public void RefreshMenu(GameObject managementTarget)
    {
        if (eo_UnassignedOverseers.Count > 0)
        {
            eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = eo_DefaultUnassignedSpawnPos;
            for (int i = 0; i < eo_UnassignedOverseers.Count; i++)
            {
                eo_UnassignedOverseers[i].transform.position = eo_UnassignedTemplateTransform.position;
                eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.x, eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.y - eo_TemplateSpacing);
                eo_UnassignedOverseers[i].GetComponent<ElevatorOverseer>().RefreshOverseerGUI();
            }
        }
        eo_HireCostText.text = eo_HireCostText.name + ": " + Mathf.ClosestPowerOfTwo(GameMaster.instance.GetHireCountEO() * eo_HireCost);
    }

    public void HireOverseer(GameObject managementTarget)
    {
        if (GameMaster.instance.GetCash() >= eo_HireCost)
        {
            CreateOverseer(managementTarget);
            GameMaster.instance.SetCash(GameMaster.instance.GetCash() - eo_HireCost);
            eo_HireCost = Mathf.ClosestPowerOfTwo(GameMaster.instance.GetHireCountMO() * eo_HireCost);
        }
    }

    private void CreateOverseer(GameObject managementTarget)
    {
        if (!managementTarget.GetComponent<Elevator>().GetManagedStatus() && managementTarget.GetComponent<Elevator>().GetLevel() >= 5)
        {
            eo_AssignedOverseer = Instantiate(eo_OverseerTemplate, eo_AssignedTemplateTransform.position, Quaternion.identity, eo_Menu.transform);
            eo_AssignedOverseer.transform.position = eo_AssignedTemplateTransform.position;
            eo_AssignedOverseer.GetComponent<ElevatorOverseer>().Generate();

            managementTarget.GetComponent<Elevator>().AssignOverseer(eo_AssignedOverseer);
            GameMaster.instance.gm_elevatorOverseers.Add(eo_AssignedOverseer);
            GameMaster.instance.SetHireCountEO(GameMaster.instance.GetHireCountEO() + 1);
        }
        else if (managementTarget.GetComponent<Elevator>().GetLevel() >= 5)
        {
            eo_NewOverseer = Instantiate(eo_OverseerTemplate, eo_UnassignedTemplateTransform.position, Quaternion.identity, eo_UnassignedOverseersMenu.transform);
            eo_NewOverseer.transform.position = eo_UnassignedTemplateTransform.position;
            eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.x, eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.y - eo_TemplateSpacing);
            eo_NewOverseer.GetComponent<ElevatorOverseer>().Generate();

            eo_UnassignedOverseers.Add(eo_NewOverseer);
            GameMaster.instance.gm_elevatorOverseers.Add(eo_NewOverseer);
            GameMaster.instance.SetHireCountEO(GameMaster.instance.GetHireCountEO() + 1);
            RefreshMenu(managementTarget);
        }   
    }

    public void AssignOverseer(GameObject managementTarget)
    {
        if (eo_AssignedOverseer != null)
        {
            //Put assigned one down
            managementTarget.GetComponent<Elevator>().UnassignOverseer(eo_AssignedOverseer);
            eo_NewOverseer = Instantiate(eo_AssignedOverseer, eo_UnassignedTemplateTransform.position, Quaternion.identity, eo_UnassignedOverseersMenu.transform);
            eo_NewOverseer.transform.position = eo_UnassignedTemplateTransform.position;
            eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.x, eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.y - eo_TemplateSpacing);
            eo_UnassignedOverseers.Add(eo_NewOverseer);
            eo_NewOverseer.GetComponent<ElevatorOverseer>().RefreshOverseerGUI();

            GameMaster.instance.gm_elevatorOverseers.Remove(eo_AssignedOverseer);
            Destroy(eo_AssignedOverseer);
            GameMaster.instance.gm_elevatorOverseers.TrimExcess();

            //Move event caller up
            managementTarget.GetComponent<Elevator>().AssignOverseer(eo_ElevatorOverseerManager.GetEventCaller());
            eo_AssignedOverseer = Instantiate(eo_ElevatorOverseerManager.GetEventCaller(), eo_UnassignedTemplateTransform.position, Quaternion.identity, eo_Menu.transform);
            eo_AssignedOverseer.transform.position = eo_AssignedTemplateTransform.position;
            eo_UnassignedOverseers.Remove(eo_ElevatorOverseerManager.GetEventCaller());
            eo_AssignedOverseer.GetComponent<ElevatorOverseer>().RefreshOverseerGUI();

            GameMaster.instance.gm_elevatorOverseers.Remove(eo_ElevatorOverseerManager.GetEventCaller());
            Destroy(eo_ElevatorOverseerManager.GetEventCaller());
            GameMaster.instance.gm_elevatorOverseers.TrimExcess();
        }
        else
        {
            eo_AssignedOverseer = Instantiate(eo_ElevatorOverseerManager.GetEventCaller(), eo_AssignedTemplateTransform.position, Quaternion.identity, eo_Menu.transform);
            eo_AssignedOverseer.transform.position = eo_AssignedTemplateTransform.position;

            eo_UnassignedOverseers.Remove(eo_ElevatorOverseerManager.GetEventCaller());
            managementTarget.GetComponent<Elevator>().AssignOverseer(eo_AssignedOverseer);
            eo_AssignedOverseer.GetComponent<ElevatorOverseer>().RefreshOverseerGUI();

            GameMaster.instance.gm_elevatorOverseers.Remove(eo_ElevatorOverseerManager.GetEventCaller());
            Destroy(eo_ElevatorOverseerManager.GetEventCaller());
            GameMaster.instance.gm_elevatorOverseers.TrimExcess();
        }

        RefreshMenu(managementTarget);

    }

    public void UnassignOverseer(GameObject managementTarget)
    {
        if (eo_ElevatorOverseerManager.GetEventCaller() == eo_AssignedOverseer)
        {
            managementTarget.GetComponent<Elevator>().UnassignOverseer(eo_AssignedOverseer);

            //Create a new copy of it
            eo_NewOverseer = Instantiate(eo_AssignedOverseer, eo_UnassignedTemplateTransform.position, Quaternion.identity, eo_UnassignedOverseersMenu.transform);
            eo_NewOverseer.transform.position = eo_UnassignedTemplateTransform.position;
            eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.x, eo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.y - eo_TemplateSpacing);

            //Add it to the unassigned group
            eo_UnassignedOverseers.Add(eo_NewOverseer);
            eo_NewOverseer.GetComponent<ElevatorOverseer>().RefreshOverseerGUI();

            //Destroy the old one
            GameMaster.instance.gm_elevatorOverseers.Remove(eo_AssignedOverseer);
            Destroy(eo_AssignedOverseer);
            GameMaster.instance.gm_elevatorOverseers.TrimExcess();
            
            RefreshMenu(managementTarget);
        }
    }


    public void SellOverseer(GameObject managementTarget)
    {
        if (eo_ElevatorOverseerManager.GetEventCaller() != eo_AssignedOverseer)
        {

            eo_ElevatorOverseerManager.GetEventCaller().GetComponent<ElevatorOverseer>().RefundValue();

            eo_UnassignedOverseers.Remove(eo_ElevatorOverseerManager.GetEventCaller());

            GameMaster.instance.gm_elevatorOverseers.Remove(eo_ElevatorOverseerManager.GetEventCaller());
            Destroy(eo_ElevatorOverseerManager.GetEventCaller());
            GameMaster.instance.gm_elevatorOverseers.TrimExcess();

            RefreshMenu(managementTarget);
        }
    }

    public void ToggleBuff(GameObject managementTarget)
    {
        eo_ElevatorOverseerManager.GetEventCaller().GetComponent<ElevatorOverseer>().ToggleBuff();
    }

    public void ToggleManagement(GameObject managementTarget)
    {
        if (!managementTarget.GetComponent<Elevator>().GetManagedStatus() || eo_ElevatorOverseerManager.GetEventCaller() != eo_AssignedOverseer)
        {
            AssignOverseer(managementTarget);
        }
        else
        {
            UnassignOverseer(managementTarget);
        }
    }
}
