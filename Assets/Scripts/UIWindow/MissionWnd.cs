using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocol;

public class MissionWnd : WindowRoot {

    public Transform[] ButtonsTrans;
    public Transform PointerTrans;

    private PlayerData pd;

    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    public void OnCloseBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    private void RefreshUI()
    {
        int missionID = pd.mission;
        for (int i = 0; i < ButtonsTrans.Length; i++)
        {
            if (i<missionID%10000)
            {
                SetActive(ButtonsTrans[i], true);
                if (i== missionID % 10000-1)
                {
                    PointerTrans.SetParent(ButtonsTrans[i]);
                    PointerTrans.localPosition = new Vector3(35, 110);
                }
            }
            else
            {
                SetActive(ButtonsTrans[i], false);
            }
        }
    }

    public void OnFbFightBtnClick(int fbid)
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        int power = ResSvc.Instance.GetMapCfg(fbid).power;

        if (power>pd.power)
        {
            GameRoot.Instance.AddTips("体力不足");
        }
        else
        {
            MissionSys.Instance.EnterBattle(fbid);
        }
    }
}
