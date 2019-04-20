using UnityEngine;

public class BattleSys:MonoBehaviour
{
    public PlayerCtrlWnd playerCtrlWnd;
    public BattleMgr battleMgr;

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
        GameObject go = new GameObject
        {
            name = "BattleRoot",
        };

        go.transform.SetParent(GameRoot.Instance.transform);
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapid);
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
}

