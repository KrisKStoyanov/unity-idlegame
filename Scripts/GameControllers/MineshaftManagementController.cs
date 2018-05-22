using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineshaftManagementController : MonoBehaviour {

    private MineshaftOverseerManager mo_MineshaftOverseerManager;

    public GameObject mo_Menu;
    public GameObject mo_UnassignedOverseersMenu;

    public int mo_HireCost = 10;
    public Text mo_HireCostText;

    public GameObject mo_OverseerTemplate;

    public Transform mo_AssignedTemplateTransform;
    public Transform mo_UnassignedTemplateTransform;

    private Vector2 mo_DefaultUnassignedSpawnPos;
    private float mo_TemplateSpacing;

    public GameObject mo_AssignedOverseer;
    private GameObject mo_NewOverseer;

    public List<GameObject> mo_AssignedOverseers = new List<GameObject>();
    public List<GameObject> mo_UnassignedOverseers = new List<GameObject>();

    private void OnEnable()
    {
        SetInitialReferences();
        if (mo_MineshaftOverseerManager != null)
        {
            mo_MineshaftOverseerManager.EventToggleMenu += ToggleMenu;
            mo_MineshaftOverseerManager.EventHireOverseer += HireOverseer;
            mo_MineshaftOverseerManager.EventSellOverseer += SellOverseer;
            mo_MineshaftOverseerManager.EventAttemptBuff += ToggleBuff;

            mo_MineshaftOverseerManager.EventToggleManagement += ToggleManagement;
        }
    }

    private void OnDisable()
    {
        mo_MineshaftOverseerManager.EventToggleMenu -= ToggleMenu;
        mo_MineshaftOverseerManager.EventHireOverseer -= HireOverseer;
        mo_MineshaftOverseerManager.EventSellOverseer -= SellOverseer;
        mo_MineshaftOverseerManager.EventAttemptBuff -= ToggleBuff;

        mo_MineshaftOverseerManager.EventToggleManagement -= ToggleManagement;

        mo_AssignedOverseer = null;
        mo_NewOverseer = null;
    }

    private void Start()
    {
        if(mo_MineshaftOverseerManager == null)
        {
            SetInitialReferences();
            mo_MineshaftOverseerManager.EventToggleMenu += ToggleMenu;
            mo_MineshaftOverseerManager.EventHireOverseer += HireOverseer;
            mo_MineshaftOverseerManager.EventSellOverseer += SellOverseer;
            mo_MineshaftOverseerManager.EventAttemptBuff += ToggleBuff;

            mo_MineshaftOverseerManager.EventToggleManagement += ToggleManagement;
        }
    }

    private void SetInitialReferences()
    {
        if(mo_MineshaftOverseerManager == null)
        {
            mo_MineshaftOverseerManager = GameMaster.gm_MineshaftOverseerManager;
        }
        if(mo_OverseerTemplate != null)
        {
            mo_TemplateSpacing = mo_OverseerTemplate.GetComponent<RectTransform>().rect.height;
        }
        mo_DefaultUnassignedSpawnPos = mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition;
    }

    public void ToggleMenu(GameObject managementTarget)
    {
        if (!mo_Menu.activeInHierarchy)
        {
            mo_Menu.SetActive(true);
            RefreshMenu(managementTarget);
        }
        else
        {
            for(int i = 0; i < mo_AssignedOverseers.Count; i++)
            {
                mo_AssignedOverseers[i].SetActive(false);
            }
            mo_Menu.SetActive(false);
        }
    }

    public void RefreshMenu(GameObject managementTarget)
    {
        for(int i = 0; i < mo_AssignedOverseers.Count; i++)
        {
            if(mo_AssignedOverseers[i].GetComponent<MineshaftOverseer>().GetAssociatedIndex() == managementTarget.GetComponent<Mineshaft>().GetIndex())
            {
                mo_AssignedOverseers[i].SetActive(true);
            }
        }

        if (mo_UnassignedOverseers.Count > 0)
        {
            mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = mo_DefaultUnassignedSpawnPos;
            for (int i = 0; i < mo_UnassignedOverseers.Count; i++)
            {
                mo_UnassignedOverseers[i].transform.position = mo_UnassignedTemplateTransform.position;
                mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.x, mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.y - mo_TemplateSpacing);
                mo_UnassignedOverseers[i].GetComponent<MineshaftOverseer>().RefreshOverseerGUI();
            }
        }
        mo_HireCostText.text = mo_HireCostText.name + ": " + mo_HireCost;
    }

    public void HireOverseer(GameObject managementTarget)
    {
        if(GameMaster.instance.GetCash() >= mo_HireCost)
        {
            CreateOverseer(managementTarget);
            GameMaster.instance.SetCash(GameMaster.instance.GetCash() - mo_HireCost);
            mo_HireCost = Mathf.ClosestPowerOfTwo(GameMaster.instance.GetHireCountMO() * mo_HireCost);  
        }
    }

    private void CreateOverseer(GameObject managementTarget)
    {
        if (!managementTarget.GetComponent<Mineshaft>().GetManagedStatus() && managementTarget.GetComponent<Mineshaft>().GetLevel() >= 5)
        {
            mo_AssignedOverseer = Instantiate(mo_OverseerTemplate, mo_AssignedTemplateTransform.position, Quaternion.identity, mo_Menu.transform);
            mo_AssignedOverseer.transform.position = mo_AssignedTemplateTransform.position;
            mo_AssignedOverseer.GetComponent<MineshaftOverseer>().Generate();

            mo_AssignedOverseers.Add(mo_AssignedOverseer);
            managementTarget.GetComponent<Mineshaft>().AssignOverseer(mo_AssignedOverseer);
            GameMaster.instance.SetHireCountMO(GameMaster.instance.GetHireCountMO() + 1);
        }
        else if (managementTarget.GetComponent<Mineshaft>().GetLevel() >= 5)
        {
            mo_NewOverseer = Instantiate(mo_OverseerTemplate, mo_UnassignedTemplateTransform.position, Quaternion.identity, mo_UnassignedOverseersMenu.transform);
            mo_NewOverseer.transform.position = mo_UnassignedTemplateTransform.position;
            mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.x, mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.y - mo_TemplateSpacing);
            mo_NewOverseer.GetComponent<MineshaftOverseer>().Generate();

            mo_UnassignedOverseers.Add(mo_NewOverseer);
            GameMaster.instance.SetHireCountMO(GameMaster.instance.GetHireCountMO() + 1);
            RefreshMenu(managementTarget);
        }
    }

    public void AssignOverseer(GameObject managementTarget)
    {
        if (mo_AssignedOverseer != null)
        {
            //Put assigned one down
            managementTarget.GetComponent<Mineshaft>().UnassignOverseer(mo_AssignedOverseer);
            mo_NewOverseer = Instantiate(mo_AssignedOverseer, mo_UnassignedTemplateTransform.position, Quaternion.identity, mo_UnassignedOverseersMenu.transform);
            mo_NewOverseer.transform.position = mo_UnassignedTemplateTransform.position;
            mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.x, mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.y - mo_TemplateSpacing);
            mo_UnassignedOverseers.Add(mo_NewOverseer);
            mo_NewOverseer.GetComponent<MineshaftOverseer>().RefreshOverseerGUI();

            mo_AssignedOverseers.Remove(mo_AssignedOverseer);
            GameMaster.instance.gm_mineshaftOverseers.Remove(mo_AssignedOverseer);
            Destroy(mo_AssignedOverseer);
            GameMaster.instance.gm_mineshaftOverseers.TrimExcess();

            GameMaster.instance.gm_mineshaftOverseers.Add(mo_NewOverseer);
            mo_NewOverseer.GetComponent<MineshaftOverseer>().RefreshOverseerGUI();

            //Move event caller up
            mo_UnassignedOverseers.Remove(mo_MineshaftOverseerManager.GetEventCaller());
            managementTarget.GetComponent<Mineshaft>().AssignOverseer(mo_MineshaftOverseerManager.GetEventCaller());
            mo_AssignedOverseer = Instantiate(mo_MineshaftOverseerManager.GetEventCaller(), mo_AssignedTemplateTransform.position, Quaternion.identity, mo_Menu.transform);
            mo_AssignedOverseer.transform.position = mo_AssignedTemplateTransform.position;
            mo_AssignedOverseers.Add(mo_AssignedOverseer);
            mo_AssignedOverseer.GetComponent<MineshaftOverseer>().RefreshOverseerGUI();

            GameMaster.instance.gm_mineshaftOverseers.Remove(mo_MineshaftOverseerManager.GetEventCaller());
            Destroy(mo_MineshaftOverseerManager.GetEventCaller());
            GameMaster.instance.gm_mineshaftOverseers.TrimExcess();

            GameMaster.instance.gm_mineshaftOverseers.Add(mo_AssignedOverseer);
            mo_AssignedOverseer.GetComponent<MineshaftOverseer>().RefreshOverseerGUI();

        }
        else
        {
            mo_UnassignedOverseers.Remove(mo_MineshaftOverseerManager.GetEventCaller());
            managementTarget.GetComponent<Mineshaft>().AssignOverseer(mo_MineshaftOverseerManager.GetEventCaller());
            mo_AssignedOverseer = Instantiate(mo_MineshaftOverseerManager.GetEventCaller(), mo_AssignedTemplateTransform.position, Quaternion.identity, mo_Menu.transform);
            mo_AssignedOverseer.transform.position = mo_AssignedTemplateTransform.position;
            mo_AssignedOverseers.Add(mo_AssignedOverseer);
            mo_AssignedOverseer.GetComponent<MineshaftOverseer>().RefreshOverseerGUI();

            GameMaster.instance.gm_mineshaftOverseers.Remove(mo_MineshaftOverseerManager.GetEventCaller());
            Destroy(mo_MineshaftOverseerManager.GetEventCaller());
            GameMaster.instance.gm_mineshaftOverseers.TrimExcess();

            GameMaster.instance.gm_mineshaftOverseers.Add(mo_AssignedOverseer);
        }
        RefreshMenu(managementTarget);
    }

    public void UnassignOverseer(GameObject managementTarget)
    {
        if(mo_MineshaftOverseerManager.GetEventCaller() == mo_AssignedOverseer)
        {

            managementTarget.GetComponent<Mineshaft>().UnassignOverseer(mo_AssignedOverseer);
            mo_AssignedOverseers.Remove(mo_AssignedOverseer);

            //Create a new copy 
            mo_NewOverseer = Instantiate(mo_AssignedOverseer, mo_UnassignedTemplateTransform.position, Quaternion.identity, mo_UnassignedOverseersMenu.transform);
            mo_NewOverseer.transform.position = mo_UnassignedTemplateTransform.position;
            mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.x, mo_UnassignedTemplateTransform.GetComponent<RectTransform>().anchoredPosition.y - mo_TemplateSpacing);

            //Add it to the unassigned group
            mo_UnassignedOverseers.Add(mo_NewOverseer);
            mo_NewOverseer.GetComponent<MineshaftOverseer>().RefreshOverseerGUI();

            //Destroy the old one
            GameMaster.instance.gm_mineshaftOverseers.Remove(mo_AssignedOverseer);
            Destroy(mo_AssignedOverseer);
            GameMaster.instance.gm_mineshaftOverseers.TrimExcess();

            GameMaster.instance.gm_mineshaftOverseers.Add(mo_NewOverseer);

            RefreshMenu(managementTarget);
        }
    }


    public void SellOverseer(GameObject managementTarget)
    {
        if(mo_MineshaftOverseerManager.GetEventCaller() != mo_AssignedOverseer)
        {

            mo_MineshaftOverseerManager.GetEventCaller().GetComponent<MineshaftOverseer>().RefundValue();

            mo_UnassignedOverseers.Remove(mo_MineshaftOverseerManager.GetEventCaller());
            GameMaster.instance.gm_mineshaftOverseers.Remove(mo_MineshaftOverseerManager.GetEventCaller());
            Destroy(mo_MineshaftOverseerManager.GetEventCaller());
            GameMaster.instance.gm_mineshaftOverseers.TrimExcess();
            RefreshMenu(managementTarget);
        }
    }

    public void ToggleBuff(GameObject managementTarget)
    {
        mo_MineshaftOverseerManager.GetEventCaller().GetComponent<MineshaftOverseer>().ToggleBuff();
    }

    public void ToggleManagement(GameObject managementTarget)
    {
        if (!managementTarget.GetComponent<Mineshaft>().GetManagedStatus() || mo_MineshaftOverseerManager.GetEventCaller() != mo_AssignedOverseer)
        {
            AssignOverseer(managementTarget);
        }
        else
        {
            UnassignOverseer(managementTarget);
        }
    }
}
