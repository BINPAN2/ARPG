using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PENet;
using PEProtocol;
/// <summary>
/// 主城业务系统
/// </summary>
public class MainCitySys :MonoBehaviour {

    private static MainCitySys instance = null;
    private MainCitySys() { }
    public static MainCitySys Instance
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

    public InfoWnd infoWnd;
    public MainCityWnd mainCityWnd;
    public GuideWnd guideWNd;
    public StrongWnd strongWnd;
    public ChatWnd chatWnd;
    public BuyWnd buyWnd;
    public TaskRewardWnd taskRewardWnd;
    private PlayerController playerCtrl;
    private Transform CharacCamTrans;
    private AutoGuideCfg curTaskData;
    private Transform[] NPCPosTrans;

    private NavMeshAgent nav;


    public void InitSys()
    {
        Debug.Log("Init MainCitySys");
    }


    public void EnterMainCity()
    {
        //异步加载主城场景
        MapCfg mapData = ResSvc.Instance.GetMapCfg(Constants.MainCityMapID);
        ResSvc.Instance.AsyncLoadScene(mapData.sceneName, () => {
            PECommon.Log("Enter MainCity");

            //加载游戏主角
            LoadPlayer(mapData);
            //打开主城场景UI
            mainCityWnd.SetWndState(true);
            //播放主城背景音乐
            AudioSvc.Instance.PlayBGAudio(Constants.BGMainCity);
            //GameRoot.Instance.GetComponent<AudioListener>().enabled = false;

            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            MainCityMap mainCityMap = map.GetComponent<MainCityMap>();
            NPCPosTrans = mainCityMap.NPCPosTrans;

            //设置展示Camera
            if (CharacCamTrans!=null)
            {
                CharacCamTrans.gameObject.SetActive(false);
            }
        });
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = ResSvc.Instance.LoadPrefab(PathDefine.AssassinCity,true);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRotate;

        //初始化相机
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRotate;

        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        nav = player.GetComponent<NavMeshAgent>();
    }

    public void SetMoveDir(Vector2 _dir)
    {
        StopNavTask();

        if (_dir != Vector2.zero)
        {
            playerCtrl.Dir = _dir;
            playerCtrl.SetBlend(Constants.BlendMove);
        }
        else
        {
            playerCtrl.Dir = Vector2.zero;
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }
    public void OpenInfoWnd()
    {
        StopNavTask();

        if (CharacCamTrans==null)
        {
            CharacCamTrans = GameObject.FindGameObjectWithTag("CharacShowCam").transform;
        }

        //设置人物展示相机相对位置
        CharacCamTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 2.5f + Vector3.up * 1.0f;
        CharacCamTrans.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
        CharacCamTrans.gameObject.SetActive(true);
        infoWnd.SetWndState(true);
    }
    public void OpenTaskRewardWnd()
    {
        StopNavTask();
        taskRewardWnd.SetWndState(true);
    }

    public void CloseInfoWnd()
    {
        if (CharacCamTrans!=null)
        {
            CharacCamTrans.gameObject.SetActive(false);
            infoWnd.SetWndState(false);
        }
    }


    private float startRotate = 0;

    public void SetStartRotate()
    {
        startRotate = playerCtrl.transform.localEulerAngles.y;
    }

    public void SetPlayerRotate(float rotate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRotate + rotate, 0);
    }

    private bool isNav = false;


    public void RunTask(AutoGuideCfg cfg)
    {
        if (cfg!=null)
        {
            curTaskData = cfg;
        }

        //解析任务数据
        nav.enabled = true;
        if (curTaskData.npcID !=-1)
        {
            float dis = Vector3.Distance(playerCtrl.transform.position, NPCPosTrans[cfg.npcID].position);

            if (dis<0.5)
            {
                isNav = false;
                nav.isStopped = true;
                nav.enabled = false;
                playerCtrl.SetBlend(Constants.BlendIdle);
                OpenGuideWnd();
            }
            else
            {
                isNav = true;
                nav.enabled = true;
                nav.speed = Constants.PlayerMoveSpeed;
                nav.SetDestination(NPCPosTrans[cfg.npcID].position);
                playerCtrl.SetBlend(Constants.BlendMove);
            }
        }
        else
        {
            //直接打开引导界面（无需找npc）
            OpenGuideWnd();
        }
    }

    private void Update()
    {
        if (isNav)
        {
            IsArriveNavPos();
            playerCtrl.SetCam();
        }
    }

    private void IsArriveNavPos()
    {
        float dis = Vector3.Distance(playerCtrl.transform.position, NPCPosTrans[curTaskData.npcID].position);

        if (dis < 0.5)
        {
            isNav = false;
            nav.isStopped = true;
            nav.enabled = false;
            playerCtrl.SetBlend(Constants.BlendIdle);
            OpenGuideWnd();
        }
    }

    public void StopNavTask()
    {
        if (isNav)
        {
            isNav = false;
            nav.isStopped = true;
            nav.enabled =  false;
            playerCtrl.SetBlend(Constants.BlendIdle);

        }
    }

    private void OpenGuideWnd()
    {
        guideWNd.SetWndState(true);
    }

    public void OpenStrongWnd()
    {
        StopNavTask();
        strongWnd.SetWndState(true);
    }

    public void OpenChatWnd()
    {
        StopNavTask();
        chatWnd.SetWndState(true);
    }

    public void PshChat(GameMsg msg)
    {
        chatWnd.AddChatMsg(msg.pshChat.name, msg.pshChat.Chat);
    }

    public void OpenBuyWnd(int type)
    {
        StopNavTask();
        buyWnd.SetBuyType(type);
        buyWnd.SetWndState(true);
    }


    public AutoGuideCfg GetCurTaskData()
    {
        return curTaskData;
    }

    public void SendGuideMsg()
    {
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqGuide,
            reqGuide = new ReqGuide
            {
                guideID = curTaskData.ID,
            }
        };

        NetSvc.Instance.SendMsg(msg);
    }

    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.rspGuide;

        GameRoot.Instance.AddTips(Constants.Color("任务奖励 金币+" + curTaskData.coin + " 经验+" + curTaskData.exp,TxtColor.Blue));

        switch (curTaskData.actID)
        {
            case 0:
                //与智者对话
                break;
            case 1:
                //进入副本
                EnterMission();
                break;
            case 2:
                //进入强化界面
                OpenStrongWnd();
                break;
            case 3:
                //进入体力购买
                OpenBuyWnd(0);
                break;
            case 4:
                //进入金币铸造
                OpenBuyWnd(1);
                break;
            case 5:
                //进入世界聊天
                OpenChatWnd();
                break;
        }
        GameRoot.Instance.SetPlayerDataByGuide(data);
        mainCityWnd.RefreshUI();
    }

    public void RspStrong(GameMsg msg)
    {
        int fightPre = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.Instance.SetPlayerDataByStrong(msg.rspStrong);
        int fightNow = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.Instance.AddTips(Constants.Color("战力增加" + (fightNow- fightPre), TxtColor.Blue));
        strongWnd.RefreshUI();
    }

    public void RspBuy (GameMsg msg)
    {
        RspBuy data = msg.rspBuy;
        GameRoot.Instance.SetPlayerDataByBuy(data);
        GameRoot.Instance.AddTips("购买成功");
        buyWnd.SetWndState(false);
        mainCityWnd.RefreshUI();
    }
    public void PshPower(GameMsg msg)
    {
        PshPower data = msg.pshPower;
        GameRoot.Instance.SetPlayerDataByPower(data);
        mainCityWnd.RefreshUI();
    }

    public void RspTakeTaskReward(GameMsg msg)
    {
        RspTakeTaskReward data = msg.rspTakeTaskReward;
        GameRoot.Instance.SetPlayerDataByTakeTaskReward(data);
        taskRewardWnd.RefreshUI();
        mainCityWnd.RefreshUI();
    }

    public void PshTaskPrgs(GameMsg msg)
    {
        PshTaskPrgs data = msg.pshTaskPrgs;
        GameRoot.Instance.SetPlayerDataByPshTaskPrgs(data);
    }

    #region Mission
    public void EnterMission()
    {
        StopNavTask();
        MissionSys.Instance.EnterMission();
    }

    #endregion

}
