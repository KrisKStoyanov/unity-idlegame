using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineshaftOverseer : Mineshaft {

    private GUI_Overseer mo_Gui = null;

    //Public due to not being displayed properly in Unity using the UI if set to protected/private
    public int mo_BuffIndex;
    public int mo_Rank;
    public float mo_BuffDuration;
    public float mo_BuffCooldown;

    protected int mo_Value;

    public bool mo_bBuffAvailable = true;
    public bool mo_bBuffActive = false;

    private float mo_BuffDurationReset;
    private float mo_BuffCooldownReset;

    private int mo_OverseerIndex = 0;
    private int mo_AssociatedIndex = 0;

    private IEnumerator mo_CoroutineTrackBuffCd;
    private IEnumerator mo_CoroutineTrackBuffDur;

    private void Awake()
    {
        mo_Gui = GetComponent<GUI_Overseer>();
        mo_Gui.RefreshText(mo_Gui.gui_Rank, mo_Rank);
        mo_Gui.RefreshText(mo_Gui.gui_BuffIndex, mo_BuffIndex);
        mo_Gui.RefreshText(mo_Gui.gui_BuffInfo, mo_BuffDuration);
    }

    public int GetOverseerIndex()
    {
        return mo_OverseerIndex;
    }

    public int GetAssociatedIndex()
    {
        return mo_AssociatedIndex;
    }

    public void SetOverseerIndex(int index)
    {
        mo_OverseerIndex = index;
    }

    public void RefreshOverseerGUI()
    {
        mo_Gui.RefreshText(mo_Gui.gui_Rank, mo_Rank);
        mo_Gui.RefreshText(mo_Gui.gui_BuffIndex, mo_BuffIndex);
        mo_Gui.RefreshText(mo_Gui.gui_BuffInfo, mo_BuffDuration);
    }

    public void SetManagedMineshaft(int mineshaftIndex)
    {
        mo_AssociatedIndex = mineshaftIndex;
        mo_Gui.gui_ManageButton.GetComponentInChildren<Text>().text = "Unassign";
        mo_Gui.RefreshText(mo_Gui.gui_Rank, mo_Rank);
        mo_Gui.RefreshText(mo_Gui.gui_BuffIndex, mo_BuffIndex);
        mo_Gui.RefreshText(mo_Gui.gui_BuffInfo, mo_BuffDuration);
    }

    public void RemoveManagedMineshaft()
    {
        mo_AssociatedIndex = 0;
        mo_Gui.gui_ManageButton.GetComponentInChildren<Text>().text = "Assign";
        mo_Gui.RefreshText(mo_Gui.gui_Rank, mo_Rank);
        mo_Gui.RefreshText(mo_Gui.gui_BuffIndex, mo_BuffIndex);
        mo_Gui.RefreshText(mo_Gui.gui_BuffInfo, mo_BuffDuration);
    }

    public void Generate()
    {
        mo_BuffIndex = Random.Range(1, 4);
        mo_Rank = Random.Range(1, 4);

        if (!GameMaster.instance.gm_mineshaftOverseers.Contains(gameObject))
        {

            GameMaster.instance.gm_mineshaftOverseers.Add(gameObject);
            for (int i = 0; i < GameMaster.instance.gm_mineshaftOverseers.Count; i++)
            {
                if (GameMaster.instance.gm_mineshaftOverseers[i].GetComponent<MineshaftOverseer>().GetOverseerIndex() != i)
                {
                    mo_OverseerIndex = i;
                    break;
                }
                else if (i == GameMaster.instance.gm_mineshaftOverseers.Count)
                {
                    mo_OverseerIndex = GameMaster.instance.gm_mineshaftOverseers.Count + 1;
                }
            }
        }

        switch (mo_Rank)
        {
            case 1:
                mo_BuffDuration = 60.0f;
                mo_BuffCooldown = 300.0f;

                mo_BuffDurationReset = mo_BuffDuration;
                mo_BuffCooldownReset = mo_BuffCooldown;

                break;

            case 2:
                mo_BuffDuration = 180.0f;
                mo_BuffCooldown = 900.0f;

                mo_BuffDurationReset = mo_BuffDuration;
                mo_BuffCooldownReset = mo_BuffCooldown;

                break;

            case 3:
                mo_BuffDuration = 600.0f;
                mo_BuffCooldown = 3000.0f;

                mo_BuffDurationReset = mo_BuffDuration;
                mo_BuffCooldownReset = mo_BuffCooldown;

                break;
        }

        mo_CoroutineTrackBuffCd = TrackBuffCooldown();
        mo_CoroutineTrackBuffDur = TrackBuffDuration();

        mo_Gui.RefreshText(mo_Gui.gui_Rank, mo_Rank);
        mo_Gui.RefreshText(mo_Gui.gui_BuffIndex, mo_BuffIndex);
        mo_Gui.RefreshText(mo_Gui.gui_BuffInfo, mo_BuffDuration);

        mo_Gui.RefreshText(mo_Gui.gui_ManageButton.GetComponentInChildren<Text>(), "Assign");
        //m_dataManager.CallEventSaveData();
    }

    public void AttempOverseerBuff()
    {
        m_MineshaftOverseerManager.CallEventAttemptBuff(gameObject);
    }

    public void ToggleBuff()
    {
        if (mo_bBuffAvailable && !mo_bBuffActive)
        {
            switch (mo_BuffIndex)
            {
                case 1:

                    switch (mo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetWalkingSpeed
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetWalkingSpeed() * 3.5f);
                            break;
                        case 2:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetWalkingSpeed
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetWalkingSpeed() * 5.5f);
                            break;
                        case 3:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetWalkingSpeed
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetWalkingSpeed() * 7.5f);
                            break;
                    }
                    break;

                case 2:

                    switch (mo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetUpgradeCost
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetUpgradeCost() - GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetUpgradeCost() * 0.4f);
                            break;
                        case 2:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetUpgradeCost
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetUpgradeCost() - GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetUpgradeCost() * 0.7f);
                            break;
                        case 3:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetUpgradeCost
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetUpgradeCost() - GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetUpgradeCost() * 0.8f);
                            break;
                    }
                    break;

                case 3:

                    switch (mo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetMiningSpeed
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetMiningSpeed() * 3);
                            break;
                        case 2:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetMiningSpeed
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetMiningSpeed() * 5);
                            break;
                        case 3:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetMiningSpeed
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetMiningSpeed() * 8);
                            break;
                    }
                    break;      
            }
            mo_bBuffActive = true;
            StartCoroutine(TrackBuffDuration());
            //m_dataManager.CallEventSaveData();
        }
        else if (!mo_bBuffAvailable && mo_bBuffActive)
        {
            switch (mo_BuffIndex)
            {
                case 1:

                    switch (mo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetWalkingSpeed
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetWalkingSpeed() / 3.5f);
                            break;
                        case 2:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetWalkingSpeed
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetWalkingSpeed() / 5.5f);
                            break;
                        case 3:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetWalkingSpeed
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetWalkingSpeed() / 7.5f);
                            break;
                    }
                    break;

                case 2:

                    switch (mo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetUpgradeCost
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetUpgradeCost() + GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetUpgradeCost() * 0.4f);
                            break;
                        case 2:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetUpgradeCost
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetUpgradeCost() + GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetUpgradeCost() * 0.7f);
                            break;
                        case 3:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetUpgradeCost
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetUpgradeCost() + GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetUpgradeCost() * 0.8f);
                            break;
                    }
                    break;

                case 3:

                    switch (mo_Rank)
                    {
                        case 1:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetMiningSpeed
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetMiningSpeed() / 3);
                            break;
                        case 2:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetMiningSpeed
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetMiningSpeed() / 5);
                            break;
                        case 3:
                            GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().SetMiningSpeed
                            (GameMaster.instance.gm_mineshafts[mo_AssociatedIndex].GetComponent<Mineshaft>().GetMiningSpeed() / 8);
                            break;
                    }
                    break;
            }
            mo_bBuffActive = false;
            StartCoroutine(TrackBuffCooldown());
            //m_dataManager.CallEventSaveData();
        }
    }

    protected IEnumerator TrackBuffDuration()
    {
        while (mo_bBuffActive)
        {
            mo_BuffDuration -= 1 * Time.deltaTime;
            mo_Gui.RefreshText(mo_Gui.gui_BuffInfo, mo_BuffDuration);
            if(mo_BuffDuration <= 0)
            {
                mo_BuffDuration = mo_BuffDurationReset;
                mo_bBuffAvailable = false;
                ToggleBuff();
                StopCoroutine(mo_CoroutineTrackBuffDur);
            }
            yield return null;
        }
    }

    protected IEnumerator TrackBuffCooldown()
    {
        while (!mo_bBuffAvailable)
        {
            mo_BuffCooldown -= 1 * Time.deltaTime;
            mo_Gui.RefreshText(mo_Gui.gui_BuffInfo, mo_BuffCooldown);
            if (mo_BuffCooldown <= 0)
            {
                mo_BuffCooldown = mo_BuffCooldownReset;
                mo_Gui.RefreshText(mo_Gui.gui_BuffInfo, mo_BuffDuration);
                mo_bBuffAvailable = true;
                StopCoroutine(mo_CoroutineTrackBuffCd);
            }
            yield return null;
        }
    }

    public void Sell()
    {
        //Return money
        m_MineshaftOverseerManager.CallEventSellOverseer(gameObject);
    }

    public void RefundValue()
    {
        GameMaster.instance.SetCash(GameMaster.instance.GetCash() + mo_Value);
    }

    public void ToggleManagement()
    {
        m_MineshaftOverseerManager.CallEventToggleManagement(gameObject);
        RefreshOverseerGUI();
    }
}
