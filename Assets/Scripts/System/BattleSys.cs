using PEProtocol;
using UnityEngine;

public class BattleSys:MonoBehaviour
{
    public PlayerCtrlWnd playerCtrlWnd;
    public BattleEndWnd battleEndWnd;
    public BattleMgr battleMgr;

    private int fbid;
    private double startTime;

    private static BattleSys instance = null;
    private BattleSys() { }
    public static BattleSys Instance
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

    public void InitSys()
    {
        Debug.Log("Init BattleSys");
    }

    public void StartBattle(int mapid)
    {
        fbid = mapid;
        GameObject go = new GameObject
        {
            name = "BattleRoot",
        };

        go.transform.SetParent(GameRoot.Instance.transform);
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapid,()=> {
            startTime = TimeSvc.Instance.GetCurTime();
        });
    }


    public void SetPlayerCtrlWndState(bool isActive = true)
    {
        playerCtrlWnd.SetWndState(isActive);
    }

    public void SetPlayerMoveDir(Vector2 dir)
    {
        battleMgr.SetPlayerMoveDir(dir);
    }

    public void ReqReleaseSkill(int index)
    {
        battleMgr.ReqReleaseSkill(index);
    }

    public Vector2 GetDirInput()
    {
        return playerCtrlWnd.currentDir;
    }

    public void EndBattle(bool isWin,int restHp)
    {
        playerCtrlWnd.currentDir = Vector2.zero;
        playerCtrlWnd.SetWndState(false);
        GameRoot.Instance.dynamicWnd.DelAllHpItemInfo();

        if (isWin)
        {
            double endTime = TimeSvc.Instance.GetCurTime();
            //发送战斗结算请求
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqFBFightEnd,
                reqFBFightEnd = new ReqFBFightEnd
                {
                    iswin = isWin,
                    fbid = fbid,
                    resthp = restHp,
                    costtime = (int)((endTime - startTime)/1000),
                },
            };
            NetSvc.Instance.SendMsg(msg);
        }

        else
        {
            SetBattleEndWndState(BattleEndType.Lose);
        }
    }

    public void SetBattleEndWndState(BattleEndType endType, bool isActive = true)
    {
        battleEndWnd.SetWndType(endType);
        battleEndWnd.SetWndState(isActive);
        if (isActive) battleMgr.isPauseGame = true;
    }

    public void DestroyBattle()
    {
        SetPlayerCtrlWndState(false);
        SetBattleEndWndState(BattleEndType.None, false);
        GameRoot.Instance.dynamicWnd.DelAllHpItemInfo();
        Destroy(battleMgr.gameObject);
    }

    public void RspFBFightEnd(GameMsg msg)
    {
        RspFBFightEnd data = msg.rspFBFightEnd;
        GameRoot.Instance.SetPlayerDataByFBFightEnd(data);
        battleEndWnd.SetBattleEndData(data.fbid, data.costtime, data.resthp);
        SetBattleEndWndState(BattleEndType.Win);
    }
}

