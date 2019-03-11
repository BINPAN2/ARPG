using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PEProtocol;
/// <summary>
/// 强化页面
/// </summary>
public class StrongWnd : WindowRoot {

    #region UI Define
    public Image imgCurtPos;
    public Text txtStartLv;
    public Transform starTransGrp;
    public Text propHP1;
    public Text propHurt1;
    public Text propDef1;
    public Text propHP2;
    public Text propHurt2;
    public Text propDef2;
    public Image propArr1;
    public Image propArr2;
    public Image propArr3;

    public Text txtNeedLv;
    public Text txtCostCoin;
    public Text txtCostCrystal;

    public Text txtStrong1;//“强化后”
    public Text txtStrong2;//“强化后”
    public Text txtStrong3;//“强化后”

    public Transform costTransRoot;
    public Text txtCoin;
    #endregion


    public Transform posBtnTrans;
    private Image[] imgs = new Image[6];
    private int currentIndex=0;
    private PlayerData pd;
    private StrongCfg nextSd;

    protected override void InitWnd()
    {
        base.InitWnd();
        RegClickEvts();
        pd = GameRoot.Instance.PlayerData;
        ClickPosItem();
    }

    public void OnCloseBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    public void RegClickEvts()
    {
        for (int i = 0; i < posBtnTrans.childCount; i++)
        {
            Image img = posBtnTrans.GetChild(i).GetComponent<Image>();
            imgs[i] = img;
            OnClick(img.gameObject, (object args) =>
            {
                AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
                ClickPosItem((int)args);
            },i);
        }
    }

    private void ClickPosItem(int index=0)
    {
        PECommon.Log("Click Item"+index);
        currentIndex = index;
        for (int i = 0; i < imgs.Length; i++)
        {
            Transform trans = imgs[i].transform;
            if (i == currentIndex)
            {
                //用箭头面板表示
                SetSprite(imgs[i], PathDefine.ItemArrorBG);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(280, 120);
            }
            else
            {
                //普通面板显示
                SetSprite(imgs[i], PathDefine.ItemPlatBG);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 120);
            }
        }
        RefreshItem();
    }


    private void RefreshItem()
    {
        //金币
        SetText(txtCoin, pd.coin);
        switch (currentIndex)
        {
            case 0:
                SetSprite(imgCurtPos, PathDefine.ItemToukui);
                break;
            case 1:
                SetSprite(imgCurtPos, PathDefine.ItemBody);
                break;
            case 2:
                SetSprite(imgCurtPos, PathDefine.ItemYaobu);
                break;
            case 3:
                SetSprite(imgCurtPos, PathDefine.ItemHand);
                break;
            case 4:
                SetSprite(imgCurtPos, PathDefine.ItemLeg);
                break;
            case 5:
                SetSprite(imgCurtPos, PathDefine.ItemFoot);
                break;
        }
        SetText(txtStartLv, pd.strongArr[currentIndex] + "星级");

        int curtStarLv = pd.strongArr[currentIndex];
        for (int i = 0; i < starTransGrp.childCount; i++)
        {
            Image img = starTransGrp.GetChild(i).GetComponent<Image>();
            if (i < curtStarLv)
            {
                SetSprite(img, PathDefine.SpStar2);
            }
            else
            {
                SetSprite(img, PathDefine.SpStar1);
            }
        }

        int sumAddHp = ResSvc.Instance.GetPropAddValPreLv(currentIndex, curtStarLv, 1);
        int sumAddDamage = ResSvc.Instance.GetPropAddValPreLv(currentIndex, curtStarLv, 2);
        int sumAddDef = ResSvc.Instance.GetPropAddValPreLv(currentIndex, curtStarLv, 3);

        SetText(propHP1, "生命 +" + sumAddHp);
        SetText(propHurt1, "伤害 +" + sumAddDamage);
        SetText(propDef1, "防御 +" + sumAddDef);

        int nextStartLv = curtStarLv + 1;
        nextSd = ResSvc.Instance.GetStrongCfg(currentIndex, nextStartLv);
        if (nextSd != null)
        {
            SetActive(propHP2);
            SetActive(propHurt2);
            SetActive(propDef2);

            SetActive(costTransRoot);
            SetActive(propArr1);
            SetActive(propArr2);
            SetActive(propArr3);

            SetText(propHP2, "+" + nextSd.addhp);
            SetText(propHurt2, "+" + nextSd.addhurt);
            SetText(propDef2, "+" + nextSd.adddef);

            SetActive(txtStrong1);
            SetActive(txtStrong2);
            SetActive(txtStrong3);

            SetText(txtNeedLv, "需要等级：" + nextSd.minlv);
            SetText(txtCostCoin,  nextSd.coin);

            SetText(txtCostCrystal, nextSd.crystal + "/" + pd.crystal);
        }
        else
        {
            SetActive(propHP2, false);
            SetActive(propHurt2, false);
            SetActive(propDef2, false);

            SetActive(costTransRoot, false);
            SetActive(propArr1, false);
            SetActive(propArr2, false);
            SetActive(propArr3, false);

            SetActive(txtStrong1, false);
            SetActive(txtStrong2, false);
            SetActive(txtStrong3, false);
        }
    }

    public void OnStrongBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);

        if (pd.strongArr[currentIndex]<10)
        {
            if (pd.lv<nextSd.minlv)
            {
                GameRoot.Instance.AddTips("角色等级不够");
                return;
            }
            if (pd.coin<nextSd.coin)
            {
                GameRoot.Instance.AddTips("金币不够");
                return;
            }
            if (pd.crystal < nextSd.crystal)
            {
                GameRoot.Instance.AddTips("水晶不够");
                return;
            }
            NetSvc.Instance.SendMsg(new GameMsg
            {
                cmd = (int)CMD.ReqStrong,
                reqStrong = new ReqStrong
                {
                    pos = currentIndex,
                }
            });

        }
        else
        {
            GameRoot.Instance.AddTips("已到达满级");
        }
    }

    public void RefreshUI()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.FBItem);
        RefreshItem();
    }

}
