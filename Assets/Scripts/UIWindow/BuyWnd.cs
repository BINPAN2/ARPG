using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocol;

public class BuyWnd : WindowRoot {

    public Text txtInfo;
    public Button btnConfirm;

    private int buyType=0;//0:体力 1:金币

    protected override void InitWnd()
    {
        base.InitWnd();
        btnConfirm.interactable = true;
        RefreshUI();
    }

    public void SetBuyType(int type)
    {
        buyType = type;
    }

    public void OnCloseBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    public void OnConfirmBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqBuy,
            reqBuy = new ReqBuy
            {
                buyType = buyType,
                cost = 10,
            }
        };

        NetSvc.Instance.SendMsg(msg);
        btnConfirm.interactable = false;
    }

    public void RefreshUI()
    {
        switch (buyType)
        {
         case 0:
                //体力
                txtInfo.text = "是否花费" + Constants.Color("10钻石", TxtColor.Red) + "购买" + Constants.Color("100体力", TxtColor.Green) + "?";
                break;
            case 1:
                //金币
                txtInfo.text = "是否花费" + Constants.Color("10钻石", TxtColor.Red) + "购买" + Constants.Color("1000金币", TxtColor.Green) + "?";
                break;
        }
    }
}
