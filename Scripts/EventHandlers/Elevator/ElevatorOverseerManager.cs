using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorOverseerManager : MonoBehaviour {

    public delegate void ElevatorOverseerEventHandler(GameObject managementTarget);

    private GameObject eventTarget; //Elevator being managed
    private GameObject eventCaller; //Overseer handling events

    public event ElevatorOverseerEventHandler EventToggleMenu;
    public event ElevatorOverseerEventHandler EventAttemptBuff;
    public event ElevatorOverseerEventHandler EventToggleManagement;
    public event ElevatorOverseerEventHandler EventHireOverseer;
    public event ElevatorOverseerEventHandler EventSellOverseer;

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
