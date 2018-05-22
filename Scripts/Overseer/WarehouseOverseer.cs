using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseOverseer : Warehouse {

    private GUI_Overseer wo_Gui = null;

    //Public due to not being displayed properly in Unity using the UI if set to protected/private
    public int wo_BuffIndex;
    public int wo_Rank;
    protected float wo_BuffDuration;
    protected float wo_BuffCooldown;

    protected int wo_Value;

    protected bool wo_bBuffAvailable = true;
    protected bool wo_bBuffActive = false;

    private float wo_BuffDurationReset;
    private float wo_BuffCooldownReset;

    public int wo_OverseerIndex = 0;
    public int wo_AssociatedIndex = 0;

    private IEnumerator wo_CoroutineTrackBuffCd;
    private IEnumerator wo_CoroutineTrackBuffDur;

    private void Awake()
    {
        wo_Gui = GetComponent<GUI_Overseer>();
        wo_Gui.RefreshText(wo_Gui.gui_Rank, wo_Rank);
        wo_Gui.RefreshText(wo_Gui.gui_BuffIndex, wo_BuffIndex);
        wo_Gui.RefreshText(wo_Gui.gui_BuffInfo, wo_BuffDuration);
    }

    public int GetOverseerIndex()
    {
        return wo_OverseerIndex;
    }

    public void RefreshOverseerGUI()
    {
        wo_Gui.RefreshText(wo_Gui.gui_Rank, wo_Rank);
        wo_Gui.RefreshText(wo_Gui.gui_BuffIndex, wo_BuffIndex);
        wo_Gui.RefreshText(wo_Gui.gui_BuffInfo, wo_BuffDuration);
    }

    public void SetManagedWarehouse()
    {
        wo_Gui.gui_ManageButton.GetComponentInChildren<Text>().text = "Unassign";
        wo_AssociatedIndex = 1;
        wo_Gui.RefreshText(wo_Gui.gui_Rank, wo_Rank);
        wo_Gui.RefreshText(wo_Gui.gui_BuffIndex, wo_BuffIndex);
        wo_Gui.RefreshText(wo_Gui.gui_BuffInfo, wo_BuffDuration);
    }

    public void RemoveManagedWarehouse()
    {
        wo_Gui.gui_ManageButton.GetComponentInChildren<Text>().text = "Assign";
        wo_AssociatedIndex = 0;
        wo_Gui.RefreshText(wo_Gui.gui_Rank, wo_Rank);
        wo_Gui.RefreshText(wo_Gui.gui_BuffIndex, wo_BuffIndex);
        wo_Gui.RefreshText(wo_Gui.gui_BuffInfo, wo_BuffDuration);
    }

    public void Generate()
    {
        wo_BuffIndex = Random.Range(1, 4);
        wo_Rank = Random.Range(1, 4);

        if (!GameMaster.instance.gm_warehouseOverseers.Contains(gameObject))
        {
            GameMaster.instance.gm_warehouseOverseers.Add(gameObject);
            wo_OverseerIndex = GameMaster.instance.gm_warehouseOverseers.Count;
        }

        switch (wo_Rank)
        {
            case 1:
                wo_BuffDuration = 60.0f;
                wo_BuffCooldown = 300.0f;

                wo_BuffDurationReset = wo_BuffDuration;
                wo_BuffCooldownReset = wo_BuffCooldown;

                break;

            case 2:
                wo_BuffDuration = 180.0f;
                wo_BuffCooldown = 900.0f;

                wo_BuffDurationReset = wo_BuffDuration;
                wo_BuffCooldownReset = wo_BuffCooldown;

                break;

            case 3:
                wo_BuffDuration = 600.0f;
                wo_BuffCooldown = 3000.0f;

                wo_BuffDurationReset = wo_BuffDuration;
                wo_BuffCooldownReset = wo_BuffCooldown;

                break;
        }

        wo_CoroutineTrackBuffCd = TrackBuffCooldown();
        wo_CoroutineTrackBuffDur = TrackBuffDuration();

        wo_Gui.RefreshText(wo_Gui.gui_Rank, wo_Rank);
        wo_Gui.RefreshText(wo_Gui.gui_BuffIndex, wo_BuffIndex);
        wo_Gui.RefreshText(wo_Gui.gui_BuffInfo, wo_BuffDuration);

        wo_Gui.RefreshText(wo_Gui.gui_ManageButton.GetComponentInChildren<Text>(), "Assign");
        //m_dataManager.CallEventSaveData();
    }

    public void AttempOverseerBuff()
    {
        w_WarehouseOverseerManager.CallEventAttemptBuff();
    }

    public void ToggleBuff()
    {
        if (wo_bBuffAvailable && !wo_bBuffActive)
        {
            switch (wo_BuffIndex)
            {
                case 1:

                    switch (wo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                        case 2:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                        case 3:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                    }
                    break;

                case 2:

                    switch (wo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                        case 2:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                        case 3:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                    }
                    break;

                case 3:

                    switch (wo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                        case 2:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                        case 3:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                    }
                    break;
            }
            wo_bBuffActive = true;
            StartCoroutine(TrackBuffDuration());
            //m_dataManager.CallEventSaveData();
        }
        else if (!wo_bBuffAvailable && wo_bBuffActive)
        {
            switch (wo_BuffIndex)
            {
                case 1:

                    switch (wo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                        case 2:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                        case 3:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                    }
                    break;

                case 2:

                    switch (wo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                        case 2:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                        case 3:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                    }
                    break;

                case 3:

                    switch (wo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                        case 2:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                        case 3:
                            GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().SetWalkingSpeed(GameMaster.instance.gm_warehouse.GetComponent<Warehouse>().GetWalkingSpeed() * 2);
                            break;
                    }
                    break;
            }
            wo_bBuffActive = false;
            StartCoroutine(TrackBuffCooldown());
            //m_dataManager.CallEventSaveData();
        }
    }

    protected IEnumerator TrackBuffDuration()
    {
        while (wo_bBuffActive)
        {
            wo_BuffDuration -= 1 * Time.deltaTime;
            wo_Gui.RefreshText(wo_Gui.gui_BuffInfo, wo_BuffDuration);
            if (wo_BuffDuration <= 0)
            {
                wo_BuffDuration = wo_BuffDurationReset;
                wo_bBuffAvailable = false;
                ToggleBuff();
                StopCoroutine(wo_CoroutineTrackBuffDur);
            }
            yield return null;
        }
    }

    protected IEnumerator TrackBuffCooldown()
    {
        while (!wo_bBuffAvailable)
        {
            wo_BuffCooldown -= 1 * Time.deltaTime;
            wo_Gui.RefreshText(wo_Gui.gui_BuffInfo, wo_BuffCooldown);
            if (wo_BuffCooldown <= 0)
            {
                wo_BuffCooldown = wo_BuffCooldownReset;
                wo_Gui.RefreshText(wo_Gui.gui_BuffInfo, wo_BuffDuration);
                wo_bBuffAvailable = true;
                StopCoroutine(wo_CoroutineTrackBuffCd);
            }
            yield return null;
        }
    }

    public void Sell()
    {
        w_WarehouseOverseerManager.CallEventSellOverseer(gameObject);
    }

    public void RefundValue()
    {
        GameMaster.instance.SetCash(GameMaster.instance.GetCash() + wo_Value);
    }

    public void ToggleManagement()
    {
        w_WarehouseOverseerManager.CallEventToggleManagement(gameObject);
    }
}
