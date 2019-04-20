using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocol;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 主城界面
/// </summary>
public class MainCityWnd : WindowRoot {
    #region UIDefine
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;


    public Animation anim;
    public Button btnMenu;

    public Text txtFight;
    public Text txtPower;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;

    public Image imgPowerPrg;
    public Transform expPrgTrans;

    public Button btnAutoGuide;
    public Button btnChat;
    #endregion

    private bool menuState= true;
    private float pointDis;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;
    private AutoGuideCfg curTaskData;

    #region MainFunctions
    protected override void InitWnd()
    {
        base.InitWnd();
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirPoint, false);
        RefreshUI();
        RegisterTouchEvts();
    }

    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtFight, PECommon.GetFightByProps(pd));
        SetText(txtPower, "体力" + pd.power + "/" + PECommon.GetPowerLimit(pd.lv));
        imgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.lv);
        SetText(txtLevel, pd.lv);
        SetText(txtName, pd.name);

        //expPrg
        int expPrg = (int)(pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv) * 100);
        SetText(txtExpPrg, expPrg + "%");
        int index = expPrg / 10;

        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        float globalRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;
        float screenwidth = Screen.width * globalRate;
        float width = (screenwidth - 220) / 10;

        grid.cellSize = new Vector2(width, 10);

        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index)
            {
                img.fillAmount = (expPrg % 10) * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }


        //设置自动任务图标
        curTaskData = ResSvc.Instance.GetAutoGuideCfg(pd.guideID);
        if (curTaskData != null)
        {
            SetGuideBtnIcon(curTaskData.npcID);
        }
        else
        {
            SetGuideBtnIcon(-1);
        }
    }


    private void SetGuideBtnIcon(int npcID)
    {
        string path = "";
        Image img = btnAutoGuide.GetComponent<Image>();
        switch (npcID)
        {
            case Constants.NPCWiseMan:
                path = PathDefine.WiseManHead;
                break;
            case Constants.NPCGeneral:
                path = PathDefine.GeneralHead;
                break;
            case Constants.NPCArtisan:
                path = PathDefine.ArtisanHead;
                break;
            case Constants.NPCTrader:
                path = PathDefine.TraderHead;
                break;
            case Constants.NPCNone:
                path = PathDefine.TaskHead;
                break;
        }

        SetSprite(img, path);
    }
    #endregion

    #region UIEvent
    public void OnTaskRewardBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenTaskRewardWnd();
    }

    public void OnMenuBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIExtenBtn);

        AnimationClip clip = null;
        menuState = !menuState;
        if (menuState ==true)
        {
            clip = anim.GetClip("OpenMCMenuAnim");
        }
        else
        {
            clip = anim.GetClip("CloseMCMenuAnim");
        }

        anim.Play(clip.name);
    }

    public void OnHeadBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
    }

    public void OnAutoGuideBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        if (curTaskData!=null)
        {
            MainCitySys.Instance.RunTask(curTaskData);
        }

        else
        {
            GameRoot.Instance.AddTips("更多引导任务正在开发中.....");
        }
    }

    public void OnStrongBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenStrongWnd();
    }

    public void OnChatBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenChatWnd();
    }

    public void OnMKCoinBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.OpenBuyWnd(1);
    }

    public void OnBuyPowerBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.OpenBuyWnd(0);
    }

    public void OnMissionBtnClick()
    {
        MainCitySys.Instance.EnterMission();
    }

    public void RegisterTouchEvts()
    {
        OnClickDown(imgTouch.gameObject, (PointerEventData evt) =>
        {
            startPos = evt.position;
            SetActive(imgDirPoint);
            imgDirBg.transform.position = evt.position;
        });

        OnClickUp(imgTouch.gameObject, (PointerEventData evt) =>
        {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirPoint, false);
            imgDirPoint.transform.localPosition = Vector2.zero;
            //方向信息传递
            MainCitySys.Instance.SetMoveDir(Vector2.zero);
            //Debug.Log(Vector2.zero);
        });

        OnDrag(imgTouch.gameObject, (PointerEventData evt) =>
        {
            Vector2 dir = evt.position - startPos;
            float len = dir.magnitude;
            if (len> pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            }
            else
            {
                imgDirPoint.transform.position = evt.position;
            }
            //TODO方向信息传递
            MainCitySys.Instance.SetMoveDir(dir);
            //Debug.Log(dir.normalized);

        });

    }


    #endregion
}
