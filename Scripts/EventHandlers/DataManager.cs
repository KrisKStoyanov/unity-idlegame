using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public delegate void DataEventHandler();

    public event DataEventHandler EventSaveData;
    public event DataEventHandler EventLoadData;
    public event DataEventHandler EventResetData;

    public void CallEventSaveData()
    {
        if (EventSaveData != null)
        {
            EventSaveData();
        }
    }

    public void CallEventLoadData()
    {
        if (EventLoadData != null)
        {
            EventLoadData();
        }
    }

    public void CallEventResetData()
    {
        if (EventResetData != null)
        {
            EventResetData();
        }
    }

}
