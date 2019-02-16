using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 创建角色页面
/// </summary>
public class CreateWnd : WindowRoot {

    public InputField iptRdName;
    protected override void InitWnd()
    {
        base.InitWnd();
        iptRdName.text = ResSvc.Instance.GetRdName(false);
    }

    public void OnRdNameBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        iptRdName.text = ResSvc.Instance.GetRdName(false);
    }

    public void OnEnterBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        if (iptRdName.text != "")
        {
            //TODO 发送角色名称至服务器
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqRename,
                reqRename = new ReqRename
                {
                    name = iptRdName.text
                }
            };

            NetSvc.Instance.SendMsg(msg);
        }
        else
        {
            GameRoot.Instance.AddTips("角色名称不合法");
        }
    }
}
