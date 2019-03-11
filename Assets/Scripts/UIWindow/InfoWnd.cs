using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocol;
using UnityEngine.EventSystems;

public class InfoWnd : WindowRoot {
    #region UIDefine
    public RawImage rawimgCharacShow;

    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtCareer;
    public Text txtFight;
    public Text txtHP;
    public Text txtDamage;
    public Text txtDef;

    public Transform detailInfoTrans;
    public Text dtxtHP;
    public Text dtxtAd;
    public Text dtxtAp;
    public Text dtxtAdDef;
    public Text dtxtApDef;
    public Text dtxtdodge;
    public Text dtxtPierce;
    public Text dtxtCritical;
    #endregion

    private Vector2 StartPos;

    protected override void InitWnd()
    {
        base.InitWnd();
        RegTouchEvts();
        SetActive(detailInfoTrans, false);
        RefreshUI();
    }

    public void RegTouchEvts()
    {
        OnClickDown(rawimgCharacShow.gameObject, (PointerEventData evt) => {
            StartPos = evt.position;
            MainCitySys.Instance.SetStartRotate();
        });

        OnDrag(rawimgCharacShow.gameObject, (PointerEventData evt) => {
            float rotate = -(evt.position.x - StartPos.x) * 0.3f ;
            MainCitySys.Instance.SetPlayerRotate(rotate);
        });
    }

    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtInfo, pd.name + " LV." + pd.lv);
        SetText(txtExp, pd.exp + "/" + PECommon.GetExpUpValByLv(pd.lv));
        imgExpPrg.fillAmount = pd.exp*1.0f / PECommon.GetExpUpValByLv(pd.lv);
        SetText(txtPower, pd.power + "/" + PECommon.GetPowerLimit(pd.lv));
        imgPowerPrg.fillAmount = pd.power* 1.0f / PECommon.GetPowerLimit(pd.lv);

        SetText(txtCareer, "暗夜刺客");
        SetText(txtFight, PECommon.GetFightByProps(pd));
        SetText(txtHP, pd.hp);
        SetText(txtDamage, pd.ad+pd.ap);
        SetText(txtDef, pd.addef+pd.apdef);


        //详细面板
        SetText(dtxtHP, pd.hp);
        SetText(dtxtAd, pd.ad);
        SetText(dtxtAp, pd.ap);
        SetText(dtxtAdDef, pd.addef);
        SetText(dtxtApDef, pd.apdef);
        SetText(dtxtdodge, pd.dodge+"%");
        SetText(dtxtPierce, pd.pierce + "%");
        SetText(dtxtCritical, pd.critical + "%");
        
    }

    public void OnCloseBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.CloseInfoWnd();
    }

    public void OnDetailBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        SetActive(detailInfoTrans, true);
    }

    public void OnDetailCloseBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        SetActive(detailInfoTrans,false);
    }
}
