using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineshaftOverseerManager : MonoBehaviour {

    public delegate void MineshaftOverseerEventHandler(GameObject managementTarget);

    private GameObject eventTarget; //Mineshaft being managed
    private GameObject eventCaller; //Overseer handling events

    public event MineshaftOverseerEventHandler EventToggleMenu;
    public event MineshaftOverseerEventHandler EventHireOverseer;
    public event MineshaftOverseerEventHandler EventAttemptBuff;

    public event MineshaftOverseerEventHandler EventSellOverseer;
    public event MineshaftOverseerEventHandler EventToggleManagement;

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

    public void CallEventHireOverseer()
    {
        if (EventHireOverseer != null)
        {
            EventHireOverseer(eventTarget);
        }
    }

    public void CallEventAttemptBuff(GameObject managementCaller)
    {
        if (EventAttemptBuff != null)
        {
            eventCaller = managementCaller;
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


    public void CallEventSellOverseer(GameObject managementCaller)
    {
        if (EventSellOverseer != null)
        {
            eventCaller = managementCaller;
            EventSellOverseer(eventTarget);
        }
    }
}
