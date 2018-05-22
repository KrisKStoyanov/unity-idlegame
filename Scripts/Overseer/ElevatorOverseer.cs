using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorOverseer : Elevator {

    private GUI_Overseer eo_Gui = null;

    //Public due to not being displayed properly in Unity using the UI if set to protected/private
    public int eo_BuffIndex;
    public int eo_Rank;
    public float eo_BuffDuration;
    public float eo_BuffCooldown;

    protected int eo_Value;

    protected bool eo_bBuffAvailable = true;
    protected bool eo_bBuffActive = false;

    private float eo_BuffDurationReset;
    private float eo_BuffCooldownReset;

    public int eo_OverseerIndex = 0;
    public int eo_AssociatedIndex = 0;

    private IEnumerator eo_CoroutineTrackBuffCd;
    private IEnumerator eo_CoroutineTrackBuffDur;

    private void Awake()
    {
        eo_Gui = GetComponent<GUI_Overseer>();
        eo_Gui.RefreshText(eo_Gui.gui_Rank, eo_Rank);
        eo_Gui.RefreshText(eo_Gui.gui_BuffIndex, eo_BuffIndex);
        eo_Gui.RefreshText(eo_Gui.gui_BuffInfo, eo_BuffDuration);
    }

    public int GetOverseerIndex()
    {
        return eo_OverseerIndex;
    }

    public void RefreshOverseerGUI()
    {
        eo_Gui.RefreshText(eo_Gui.gui_Rank, eo_Rank);
        eo_Gui.RefreshText(eo_Gui.gui_BuffIndex, eo_BuffIndex);
        eo_Gui.RefreshText(eo_Gui.gui_BuffInfo, eo_BuffDuration);
    }

    public void SetManagedElevator()
    {
        eo_Gui.gui_ManageButton.GetComponentInChildren<Text>().text = "Unassign";
        eo_AssociatedIndex = 1;
        eo_Gui.RefreshText(eo_Gui.gui_Rank, eo_Rank);
        eo_Gui.RefreshText(eo_Gui.gui_BuffIndex, eo_BuffIndex);
        eo_Gui.RefreshText(eo_Gui.gui_BuffInfo, eo_BuffDuration);
    }

    public void RemoveManagedElevator()
    {
        eo_Gui.gui_ManageButton.GetComponentInChildren<Text>().text = "Assign";
        eo_AssociatedIndex = 0;
        eo_Gui.RefreshText(eo_Gui.gui_Rank, eo_Rank);
        eo_Gui.RefreshText(eo_Gui.gui_BuffIndex, eo_BuffIndex);
        eo_Gui.RefreshText(eo_Gui.gui_BuffInfo, eo_BuffDuration);
    }

    public void Generate()
    {
        eo_BuffIndex = Random.Range(1, 4);
        eo_Rank = Random.Range(1, 4);

        if (!GameMaster.instance.gm_elevatorOverseers.Contains(gameObject))
        {
            GameMaster.instance.gm_elevatorOverseers.Add(gameObject);
            eo_OverseerIndex = GameMaster.instance.gm_elevatorOverseers.Count;
        }

        switch (eo_Rank)
        {
            case 1:
                eo_BuffDuration = 60.0f;
                eo_BuffCooldown = 300.0f;

                eo_BuffDurationReset = eo_BuffDuration;
                eo_BuffCooldownReset = eo_BuffCooldown;

                break;

            case 2:
                eo_BuffDuration = 180.0f;
                eo_BuffCooldown = 900.0f;

                eo_BuffDurationReset = eo_BuffDuration;
                eo_BuffCooldownReset = eo_BuffCooldown;

                break;

            case 3:
                eo_BuffDuration = 600.0f;
                eo_BuffCooldown = 3000.0f;

                eo_BuffDurationReset = eo_BuffDuration;
                eo_BuffCooldownReset = eo_BuffCooldown;

                break;
        }

        eo_CoroutineTrackBuffCd = TrackBuffCooldown();
        eo_CoroutineTrackBuffDur = TrackBuffDuration();

        eo_Gui.RefreshText(eo_Gui.gui_Rank, eo_Rank);
        eo_Gui.RefreshText(eo_Gui.gui_BuffIndex, eo_BuffIndex);
        eo_Gui.RefreshText(eo_Gui.gui_BuffInfo, eo_BuffDuration);

        eo_Gui.RefreshText(eo_Gui.gui_ManageButton.GetComponentInChildren<Text>(), "Assign");
        //m_dataManager.CallEventSaveData();
    }

    public void AttempOverseerBuff()
    {
        e_ElevatorOverseerManager.CallEventAttemptBuff();
    }

    public void ToggleBuff()
    {
        if (eo_bBuffAvailable && !eo_bBuffActive)
        {
            switch (eo_BuffIndex)
            {
                case 1:

                    switch (eo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                        case 2:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                        case 3:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                    }
                    break;

                case 2:

                    switch (eo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                        case 2:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                        case 3:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                    }
                    break;

                case 3:

                    switch (eo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                        case 2:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                        case 3:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                    }
                    break;
            }
            eo_bBuffActive = true;
            StartCoroutine(eo_CoroutineTrackBuffDur);
            //m_dataManager.CallEventSaveData();
        }
        else if (!eo_bBuffAvailable && eo_bBuffActive)
        {
            switch (eo_BuffIndex)
            {
                case 1:

                    switch (eo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                        case 2:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                        case 3:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                    }
                    break;

                case 2:

                    switch (eo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                        case 2:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                        case 3:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                    }
                    break;

                case 3:

                    switch (eo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                        case 2:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                        case 3:
                            GameMaster.instance.gm_elevator.GetComponent<Elevator>().SetMovementSpeed(GameMaster.instance.gm_elevator.GetComponent<Elevator>().GetMovementSpeed() * 2);
                            break;
                    }
                    break;
            }
            eo_bBuffActive = false;
            StartCoroutine(eo_CoroutineTrackBuffCd);
            //m_dataManager.CallEventSaveData();
        }
    }

    protected IEnumerator TrackBuffDuration()
    {
        while (eo_bBuffActive)
        {
            eo_BuffDuration -= 1 * Time.deltaTime;
            eo_Gui.RefreshText(eo_Gui.gui_BuffInfo, eo_BuffDuration);
            if (eo_BuffDuration <= 0)
            {
                eo_BuffDuration = eo_BuffDurationReset;
                eo_bBuffAvailable = false;
                ToggleBuff();
                StopCoroutine(eo_CoroutineTrackBuffDur);
            }
            yield return null;
        }
    }

    protected IEnumerator TrackBuffCooldown()
    {
        while (!eo_bBuffAvailable)
        {
            eo_BuffCooldown -= 1 * Time.deltaTime;
            eo_Gui.RefreshText(eo_Gui.gui_BuffInfo, eo_BuffCooldown);
            if (eo_BuffCooldown <= 0)
            {
                eo_BuffCooldown = eo_BuffCooldownReset;
                eo_Gui.RefreshText(eo_Gui.gui_BuffInfo, eo_BuffDuration);
                eo_bBuffAvailable = true;
                StopCoroutine(eo_CoroutineTrackBuffCd);
            }
            yield return null;
        }
    }

    public void Sell()
    {
        e_ElevatorOverseerManager.CallEventSellOverseer(gameObject);
    }

    public void RefundValue()
    {
        GameMaster.instance.SetCash(GameMaster.instance.GetCash() + eo_Value);
    }

    public void ToggleManagement()
    {
        e_ElevatorOverseerManager.CallEventToggleManagement(gameObject);
    }
}