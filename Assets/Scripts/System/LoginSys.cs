using PEProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 登录业务系统
/// </summary>
public class LoginSys : MonoBehaviour
{
    private static LoginSys instance = null;
    private LoginSys() { }
    public static LoginSys Instance
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

    public LoginWnd loginWnd;
    public CreateWnd createWnd;

    public void InitSys()
    {
        Debug.Log("Init LoginSys");
    }

    /// <summary>
    /// 进入登陆场景
    /// </summary>
    public void Enterlogin()
    {
        //TODO
        //异步加载登陆场景
        ResSvc.Instance.AsyncLoadScene(Constants.SceneLogin, () =>
         {
            //加载完成后打开注册登陆界面
            loginWnd.SetWndState(true);
         });
        //播放登录界面背景音乐
        AudioSvc.Instance.PlayBGAudio(Constants.BGLogin, true);

    }

    public void RsLogin(GameMsg msg)
    {
        GameRoot.Instance.AddTips("登陆成功");
        GameRoot.Instance.SetPlayerData(msg.rspLogin.playerData);

        if (msg.rspLogin.playerData.name == "")//没有角色
        {
            //打开角色创建面板
            createWnd.SetWndState(true);
        }
        else
        {
            //进入主城 TODO
        }
        //关闭登陆页面
        loginWnd.SetWndState(false);
    }


}
