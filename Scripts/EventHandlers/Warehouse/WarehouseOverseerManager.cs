using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseOverseerManager : MonoBehaviour {

    public delegate void WarehouseOverseerEventHandler(GameObject managementTarget);

    private GameObject eventTarget; //Warehouse being managed
    private GameObject eventCaller; //Overseer handling events

    public event WarehouseOverseerEventHandler EventToggleMenu;
    public event WarehouseOverseerEventHandler EventHireOverseer;
    public event WarehouseOverseerEventHandler EventAttemptBuff;

    public event WarehouseOverseerEventHandler EventToggleManagement;
    public event WarehouseOverseerEventHandler EventSellOverseer;

    public GameObject GetEventCaller()
    {
        return eventCaller;
    }

    public void CallEventToggleMenu(GameObject managementTarget)
    {
        if (EventToggleMenu != null)
        {
            eventTarget = managementTarget;
            EventToggleMenu(eventTarget);
        }
    }

    public void CallEventAttemptBuff()
    {
        if (EventAttemptBuff != null)
        {
            EventAttemptBuff(eventTarget);
        }
    }


    public void CallEventToggleManagement(GameObject managementCaller)
    {
        if (EventToggleManagement != null)
        {
            eventCaller = managementCaller;
            EventToggleManagement(eventTarget);
        }
    }


    public void CallEventHireOverseer()
    {
        if (EventHireOverseer != null)
        {
            EventHireOverseer(eventTarget);
        }
    }


    public void CallEventSellOverseer(GameObject managementCaller)
    {
        if (EventSellOverseer != null)
        {
            eventCaller = managementCaller;
            EventSellOverseer(eventTarget);
        }
    }
}
