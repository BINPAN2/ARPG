using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocol;

/// <summary>
/// 游戏入口
/// </summary>
public class GameRoot : MonoBehaviour {

    private static GameRoot instance = null;
    private GameRoot() { }
    public static GameRoot Instance
    {
        get
        {
            return instance;
        }
    }


    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        Debug.Log("Game Start...");
        ClearUIRoot();
        Init();
    }

    private void Init()
    {
        //服务模块初始化
        NetSvc.Instance.InitSvc();
        ResSvc.Instance.InitSvc();
        AudioSvc.Instance.InitSvc();
        //业务系统初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        //进入登陆场景并加载相应UI
        login.Enterlogin();
    }

    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }

        dynamicWnd.SetWndState(true);
    }

    public  void  AddTips(string tips)
    {
        dynamicWnd.AddTips(tips);
    }

    private PlayerData playerData = null;
    public PlayerData PlayerData
    {
        get
        {
            return playerData;
        }
    }

    public void SetPlayerData(PlayerData pd)
    {
        playerData = pd;
    }

}
