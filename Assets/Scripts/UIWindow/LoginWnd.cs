﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 登录页面
/// </summary>
public class LoginWnd : WindowRoot {
    public InputField iptAccount;
    public InputField iptPassword;
    public Button btnEnter;
    public Button btnNotice;

    protected override void InitWnd()
    {
        if (PlayerPrefs.HasKey("Account")&&PlayerPrefs.HasKey("Password"))
        {
            iptAccount.text = PlayerPrefs.GetString("Account");
            iptPassword.text = PlayerPrefs.GetString("Password");
        }
        else
        {
            iptAccount.text = "";
            iptPassword.text = "";
        }
        
    }

    public void OnEnterBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UILoginBtn);
        if (iptAccount.text!=null|| iptPassword.text!= null)
        {
            PlayerPrefs.SetString("Account", iptAccount.text);
            PlayerPrefs.SetString("Password", iptPassword.text);
            LoginSys.Instance.RsLogin();
        }
        else
        {
            GameRoot.Instance.AddTips("用户名和密码不能为空！");
        }
    }

    public void OnNoticeBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        GameRoot.Instance.AddTips("还在制作中...");
    }

}