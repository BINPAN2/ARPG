using PEProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MissionSys : MonoBehaviour {
    private static MissionSys instance = null;
    private MissionSys() { }
    public static MissionSys Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public MissionWnd missionWnd;

    public void InitSys()
    {
        Debug.Log("Init MissionSys");
    }

    public void EnterMission()
    {
        OpenMissionWnd();
    }

    public void OpenMissionWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIOpenPage);
        missionWnd.SetWndState(true);
    }

    public void CloseMissionWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIOpenPage);
        missionWnd.SetWndState(false);
    }



    public void EnterBattle(int fbid)
    {
        NetSvc.Instance.SendMsg(new GameMsg
        {
            cmd = (int)CMD.ReqFBFight,
            reqFBFight = new ReqFBFight
            {
                fbid = fbid,
            },
        });
    }

    public void RspFBFight(GameMsg msg)
    {
        RspFBFight data = msg.rspFBFight;
        GameRoot.Instance.SetPlayerDataByFBFightStart(data);
        MainCitySys.Instance.mainCityWnd.SetWndState(false);
        CloseMissionWnd();

        BattleSys.Instance.StartBattle(msg.rspFBFight.fbid);
    }
}
