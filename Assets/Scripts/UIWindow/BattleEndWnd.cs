using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEndWnd : WindowRoot {

    #region UI Define
    public Transform rewardTrans;
    public Button btnClose;
    public Button btnExit;
    public Button btnConfirm;

    public Text txtTime;
    public Text txtRestHp;
    public Text txtReward;
    #endregion

    private BattleEndType endType = BattleEndType.None;

    protected override void InitWnd()
    {
        base.InitWnd();
        RefreshUI();
    }

    private void RefreshUI()
    {
        switch (endType)
        {
            case BattleEndType.Pause:
                SetActive(rewardTrans, false);
                SetActive(btnClose.gameObject, true);
                SetActive(btnExit.gameObject, true);
                break;
            case BattleEndType.Win:
                SetActive(rewardTrans, false);
                SetActive(btnClose.gameObject, false);
                SetActive(btnExit.gameObject, false);

                MapCfg mapcfg = ResSvc.Instance.GetMapCfg(fbid);
                int min = costtime / 60;
                int sec = costtime % 60;
                int coin = mapcfg.coin;
                int exp = mapcfg.exp;
                int crystal = mapcfg.crystal;

                SetText(txtTime, " " + min + ":" + sec);
                SetText(txtRestHp, " " + resthp);
                SetText(txtReward, Constants.Color(" 金币:" + coin, TxtColor.Yellow) + Constants.Color(" 经验:" + exp, TxtColor.Green) + Constants.Color(" 水晶:" + crystal, TxtColor.Blue));

                TimeSvc.Instance.AddTimeTask((int tid) =>
                {
                    SetActive(rewardTrans, true);
                },1000);

                break;
            case BattleEndType.Lose:
                AudioSvc.Instance.PlayUIAudio(Constants.FBLose);
                SetActive(rewardTrans, false);
                SetActive(btnClose.gameObject, false);
                SetActive(btnExit.gameObject, true);
                break;
        }
    }

    public void SetWndType(BattleEndType endType)
    {
        this.endType = endType;
    }

    public void OnCloseBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
        BattleSys.Instance.battleMgr.isPauseGame = false;
    }

    public void OnExitBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        BattleSys.Instance.battleMgr.isPauseGame = false;
        //进入主城
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
    }

    public void OnConfirmBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        BattleSys.Instance.battleMgr.isPauseGame = false;
        //进入主城，显示选择任务界面
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
        MissionSys.Instance.EnterMission();
    }


    private int fbid;
    private int costtime;
    private int resthp;
    public void SetBattleEndData(int fbid,int costtime,int resthp)
    {
        this.fbid = fbid;
        this.costtime = costtime;
        this.resthp = resthp;
    }

}

public enum BattleEndType
{
    None,
    Pause,
    Win,
    Lose,
}
