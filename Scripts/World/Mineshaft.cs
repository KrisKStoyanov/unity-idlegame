using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mineshaft : MonoBehaviour {

    //Manage actions inside the object

    protected MineshaftUpgradeManager m_MineshaftUpgradeManager;
    protected MineshaftOverseerManager m_MineshaftOverseerManager;

    private WorldManager m_WorldManager;
    private DataManager m_DataManager;

    private GUI_Mineshaft m_Gui = null;

    protected int m_Index = 0;
    protected int m_Level = 1;

    private float m_Money = 0;

    protected float m_UpgradeCost = 11;

    protected float m_Total = 3.33f;
    protected int m_Miners = 1;
    protected float m_WalkingSpeed = 2;
    protected float m_MiningSpeed = 5;
    protected int m_WorkerCapacity = 20;

    protected float m_TravelTime = 5;
    protected float m_MineTime = 5;

    private float m_TravelTimeReset;
    private float m_MineTimeReset;

    private bool m_bManaged = false;
    private bool m_bManual = false;

    private bool m_bTraveled = false;
    private bool m_bMined = false;

    private bool m_bFinishedOperation = false;

    private void OnEnable()
    {
        SetInitialReferences();
    }

    private void Start()
    {
        if (m_MineshaftUpgradeManager == null || m_MineshaftOverseerManager == null || m_DataManager == null || m_WorldManager == null)
        {
            SetInitialReferences();
        }
    }

    public int GetIndex()
    {
        return m_Index;
    }

    public int GetLevel()
    {
        return m_Level;
    }

    public bool GetManagedStatus()
    {
        return m_bManaged;
    }

    public void SetIndex(int index)
    {
        m_Index = index;
    }

    public void SetLevel(int level)
    {
        m_Level = level;
    }

    public void SetMiners(int miners)
    {
        m_Miners = miners;
    }

    public void SetWalkingSpeed(float walkingSpeed)
    {
        m_WalkingSpeed = walkingSpeed;
    }

    public void SetMiningSpeed(float miningSpeed)
    {
        m_MiningSpeed = miningSpeed;
    }

    public void SetWorkerCapacity(int workerCapacity)
    {
        m_WorkerCapacity = workerCapacity;
    }

    public void SetUpgradeCost(float upgradeCost)
    {
        m_UpgradeCost = upgradeCost;
    }

    public void SetMoney(float money)
    {
        m_Money = money;
        m_Gui.RefreshText(m_Gui.gui_mMoney, m_Money);
    }

    public float GetMoney()
    {
        return m_Money;
    }

    public float GetTotal()
    {
        return m_Total;
    }

    public int GetMiners()
    {
        return m_Miners;
    }

    public float GetWalkingSpeed()
    {
        return m_WalkingSpeed;
    }

    public float GetMiningSpeed()
    {
        return m_MiningSpeed;
    }

    public int GetWorkerCapacity()
    {
        return m_WorkerCapacity;
    }

    public float GetUpgradeCost()
    {
        return m_UpgradeCost;
    }

    private void SetInitialReferences()
    {
        if (m_WorldManager == null)
        {
            m_WorldManager = GameMaster.gm_worldManager;
        }

        if (m_MineshaftUpgradeManager == null)
        {
            m_MineshaftUpgradeManager = GameMaster.gm_MineshaftUpgradeManager;
        }

        if (m_MineshaftOverseerManager == null)
        {
            m_MineshaftOverseerManager = GameMaster.gm_MineshaftOverseerManager;
        }

        if(m_DataManager == null)
        {
            m_DataManager = GameMaster.gm_dataManager;
        }

        m_Gui = GetComponent<GUI_Mineshaft>();

        m_TravelTimeReset = m_TravelTime;
        m_MineTimeReset = m_MineTime;

        CalculateMineTime();
        CalculateTravelTime();
        CalculateTotal();
    }

    public void CalculateTravelTime()
    {
        //+1 is to ensure the coroutine can always be executed
        m_TravelTime = Mathf.Abs(m_Miners - (m_WalkingSpeed * Time.deltaTime)) + 1;
        m_TravelTimeReset = m_TravelTime;
    }

    public void CalculateMineTime()
    {
        //+1 is to ensure the coroutine can always be executed
        m_MineTime = Mathf.Abs(m_Miners - (m_MiningSpeed * Time.deltaTime)) + 1;
        m_MineTimeReset = m_MineTime;
    }

    public void CalculateTotal()
    {
        m_Total = Mathf.Abs((m_WorkerCapacity * m_Miners) / (m_MineTime + m_TravelTime));
    }

    public void AttemptBuff()
    {
        if (m_bManaged)
        {
            m_MineshaftOverseerManager.CallEventAttemptBuff(GameMaster.instance.gm_mineshaftOverseers[m_Index]);
        }
    }

    public void AssignOverseer(GameObject mineshaftOverseer)
    {
        mineshaftOverseer.GetComponent<MineshaftOverseer>().SetManagedMineshaft(m_Index);
        m_bManaged = true;
        StartCoroutine(Mine());
    }

    public void UnassignOverseer(GameObject mineshaftOverseer)
    {
        mineshaftOverseer.GetComponent<MineshaftOverseer>().RemoveManagedMineshaft();
        m_bManaged = false;
    }

    public void ManualMine()
    {
        if (!m_bManaged)
        {
            m_bManual = true;
            StartCoroutine(Mine());
        }
    }

    private IEnumerator Mine()
    {
        while (m_bManual || m_bManaged)
        {
            if (!m_bTraveled && !m_bMined)
            {
                m_TravelTime -= 1 * Time.deltaTime;
                if (m_TravelTime <= 0)
                {
                    m_TravelTime = m_TravelTimeReset;
                    m_bTraveled = true;
                }
            }

            if(m_bTraveled && !m_bMined)
            {
                m_MineTime -= 1 * Time.deltaTime;
                if(m_MineTime <= 0)
                {
                    m_MineTime = m_MineTimeReset;
                    m_bMined = true;
                }
            }

            if (m_bTraveled && m_bMined)
            {
                m_TravelTime -= 1 * Time.deltaTime;
                if(m_TravelTime <= 0)
                {
                    m_TravelTime = m_TravelTimeReset;
                    m_bFinishedOperation = true;
                    m_bTraveled = false;
                    m_bMined = false;
                }
            }
            if (m_bFinishedOperation && m_bManaged)
            {
                m_Money += Mathf.RoundToInt(m_Total);
                
                m_TravelTime = m_TravelTimeReset;
                m_MineTime = m_MineTimeReset;

                m_bFinishedOperation = false;

                m_WorldManager.CallEventActivateElevator();

                m_Gui.RefreshText(m_Gui.gui_mMoney, m_Money);

                m_DataManager.CallEventSaveData();
            }
            else if(m_bFinishedOperation && !m_bManaged)
            {
                m_Money += Mathf.RoundToInt(m_Total);

                m_TravelTime = m_TravelTimeReset;
                m_MineTime = m_MineTimeReset;

                m_bManual = false;
                m_bFinishedOperation = false;

                m_WorldManager.CallEventActivateElevator();

                m_Gui.RefreshText(m_Gui.gui_mMoney, m_Money);

                m_DataManager.CallEventSaveData();
            }
            m_Gui.RefreshText(m_Gui.gui_mTravelTime, m_TravelTime);
            m_Gui.RefreshText(m_Gui.gui_mMineTime, m_MineTime);
            yield return null;
        }
    }

    public void Upgrade()
    {
        if(GameMaster.instance.GetCash() >= m_UpgradeCost)
        {
            m_MiningSpeed += 1f;
            m_Miners += 1;
            m_WalkingSpeed += 1f;
            m_WorkerCapacity += 10;

            m_Level += 1;
            if(m_Level >= 5)
            {
                m_Gui.gui_mRequirement.gameObject.SetActive(false);
            }

            if(m_Level % 2 == 0)
            {
                GameMaster.instance.SetSuperCash(GameMaster.instance.GetSuperCash() + 10);
            }

            CalculateMineTime();
            CalculateTravelTime();
            CalculateTotal();
            m_Gui.RefreshText(m_Gui.gui_mLevel, m_Level);
            GameMaster.instance.SetCash(GameMaster.instance.GetCash() - m_UpgradeCost);

            m_UpgradeCost += Mathf.NextPowerOfTwo(Mathf.RoundToInt(m_UpgradeCost));
        }
    }

    public void RefreshGUI()
    {
        m_Gui.RefreshText(m_Gui.gui_mIndex, m_Index);
        m_Gui.RefreshText(m_Gui.gui_mLevel, m_Level);
        m_Gui.RefreshText(m_Gui.gui_mMoney, m_Money);
        m_Gui.RefreshText(m_Gui.gui_mMineTime, m_MineTime);
        m_Gui.RefreshText(m_Gui.gui_mTravelTime, m_TravelTime);
    }
}
