using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    public MainCityWnd mainCityWnd;
    private PlayerController playerCtrl;

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
            //设置Camera
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
    }

    public void SetMoveDir(Vector2 _dir)
    {
        if (_dir != Vector2.zero)
        {
            playerCtrl.Dir = _dir;
            playerCtrl.SetBlend(Constants.BlendWalk);
        }
        else
        {
            playerCtrl.Dir = Vector2.zero;
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }
}
