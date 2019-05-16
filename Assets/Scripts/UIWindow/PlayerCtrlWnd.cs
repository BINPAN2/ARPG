using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocol;
using UnityEngine.EventSystems;

public class PlayerCtrlWnd : WindowRoot {
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;

    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;

    public Transform expPrgTrans;

    private float pointDis;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;

    public Vector2 currentDir;

    public Text txtPlayerHp;
    public Image imgPlayerHpPrg;

    public Transform bossHpTrans;
    public Image imgHpRed;
    public Image imgHpYellow;

    private int MaxHp;

    protected override void InitWnd()
    {
        base.InitWnd();
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirPoint, false);

        RegisterTouchEvts();
        skilll1CDTime = ResSvc.Instance.GetSkillCfg(101).cdTime/1000.0f;
        skilll2CDTime = ResSvc.Instance.GetSkillCfg(102).cdTime / 1000.0f;
        skilll3CDTime = ResSvc.Instance.GetSkillCfg(103).cdTime / 1000.0f;

        MaxHp = GameRoot.Instance.PlayerData.hp;
        SetText(txtPlayerHp, MaxHp + "/" + MaxHp);
        imgPlayerHpPrg.fillAmount = 1;

        SetBossHpState(false);

        RefreshUI();
    }

    private void Update()
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnSkillBtnClick(0);
        }

        if (isSkill1CD)
        {
            skill1FillCount += Time.deltaTime;
            if (skill1FillCount>= skilll1CDTime)
            {
                isSkill1CD = false;
                SetActive(imgSkill1CD, false);
                skill1FillCount = 0;
                skill1NumCount = 0;
            }
            else
            {
                imgSkill1CD.fillAmount = 1 - skill1FillCount / skilll1CDTime;
            }

            skill1NumCount += Time.deltaTime;
            if (skill1NumCount>=1)
            {
                skill1NumCount -= 1;
                skill1Num -= 1;
                SetText(txtSKill1CD, skill1Num);
            }
        }

        if (isSkill2CD)
        {
            skill2FillCount += Time.deltaTime;
            if (skill2FillCount >= skilll2CDTime)
            {
                isSkill2CD = false;
                SetActive(imgSkill2CD, false);
                skill2FillCount = 0;
                skill2NumCount = 0;
            }
            else
            {
                imgSkill2CD.fillAmount = 1 - skill2FillCount / skilll2CDTime;
            }

            skill2NumCount += Time.deltaTime;
            if (skill2NumCount >= 1)
            {
                skill2NumCount -= 1;
                skill2Num -= 1;
                SetText(txtSKill2CD, skill2Num);
            }
        }

        if (isSkill3CD)
        {
            skill3FillCount += Time.deltaTime;
            if (skill3FillCount >= skilll3CDTime)
            {
                isSkill3CD = false;
                SetActive(imgSkill3CD, false);
                skill3FillCount = 0;
                skill3NumCount = 0;
            }
            else
            {
                imgSkill3CD.fillAmount = 1 - skill3FillCount / skilll3CDTime;
            }

            skill3NumCount += Time.deltaTime;
            if (skill3NumCount >= 1)
            {
                skill3NumCount -= 1;
                skill3Num -= 1;
                SetText(txtSKill3CD, skill3Num);
            }
        }

        UpdateMixBlend();
    }

    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
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
            currentDir = Vector2.zero;
            //方向信息传递
            BattleSys.Instance.SetPlayerMoveDir(currentDir);
            //Debug.Log(Vector2.zero);
        });

        OnDrag(imgTouch.gameObject, (PointerEventData evt) =>
        {
            Vector2 dir = evt.position - startPos;
            float len = dir.magnitude;
            if (len > pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            }
            else
            {
                imgDirPoint.transform.position = evt.position;
            }
            currentDir = dir.normalized;
            //TODO方向信息传递
            BattleSys.Instance.SetPlayerMoveDir(currentDir);
        });

    }


    #region Skill
    public Image imgSkill1CD;
    public Text txtSKill1CD;
    private bool isSkill1CD = false;
    private float skilll1CDTime;
    private int skill1Num;//显示的CD时间
    private float skill1FillCount=0;//skill1的扇形图片计时器
    private float skill1NumCount = 0;//skill1的CD时间文本计时器

    public Image imgSkill2CD;
    public Text txtSKill2CD;
    private bool isSkill2CD = false;
    private float skilll2CDTime;
    private int skill2Num;//显示的CD时间
    private float skill2FillCount = 0;//skill1的扇形图片计时器
    private float skill2NumCount = 0;//skill1的CD时间文本计时器

    public Image imgSkill3CD;
    public Text txtSKill3CD;
    private bool isSkill3CD = false;
    private float skilll3CDTime;
    private int skill3Num;//显示的CD时间
    private float skill3FillCount = 0;//skill1的扇形图片计时器
    private float skill3NumCount = 0;//skill1的CD时间文本计时器
    #endregion

    public void OnSkillBtnClick(int index)
    {
        switch (index)
        {
            case 0:
                BattleSys.Instance.ReqReleaseSkill(index);
                break;
            case 1:
                if (isSkill1CD == false&& GetCanRlsSkill())
                {
                    BattleSys.Instance.ReqReleaseSkill(index);
                    isSkill1CD = true;
                    SetActive(imgSkill1CD, true);
                    imgSkill1CD.fillAmount = 1;
                    skill1Num = (int)skilll1CDTime;
                    SetText(txtSKill1CD, skill1Num);
                }
                break;
            case 2:
                if (isSkill2CD == false && GetCanRlsSkill())
                {
                    BattleSys.Instance.ReqReleaseSkill(index);
                    isSkill2CD = true;
                    SetActive(imgSkill2CD, true);
                    imgSkill2CD.fillAmount = 1;
                    skill2Num = (int)skilll2CDTime;
                    SetText(txtSKill2CD, skill2Num);
                }
                break;
            case 3:
                if (isSkill3CD == false && GetCanRlsSkill())
                {
                    BattleSys.Instance.ReqReleaseSkill(index);
                    isSkill3CD = true;
                    SetActive(imgSkill3CD, true);
                    imgSkill3CD.fillAmount = 1;
                    skill3Num = (int)skilll3CDTime;
                    SetText(txtSKill3CD, skill3Num);
                }
                break;
        }
    }

    public void SetPlayerHpBarVal(int hp)
    {
        SetText(txtPlayerHp, hp + "/" + MaxHp);
        imgPlayerHpPrg.fillAmount = hp * 1.0f / MaxHp;
    }

    public bool GetCanRlsSkill()
    {
        return BattleSys.Instance.battleMgr.CanRlsSkill();
    }

    public void SetBossHpState(bool state,float prgs=1.0f)
    {
        SetActive(bossHpTrans, state);
        imgHpRed.fillAmount = prgs;
        imgHpYellow.fillAmount = prgs;
    }

    private float currentHpPrg;
    private float targetHpPrg;
    public void SetBossHpVal(int oldVal,int newVal,int sumVal)
    {
        currentHpPrg = oldVal * 1.0f / sumVal;
        targetHpPrg = newVal * 1.0f / sumVal;
        imgHpRed.fillAmount = targetHpPrg;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentHpPrg - targetHpPrg) < Constants.AccelerHpSpeed * Time.deltaTime)
        {
            currentHpPrg = targetHpPrg;
        }
        else if (currentHpPrg > targetHpPrg)
        {
            currentHpPrg -= Constants.AccelerHpSpeed * Time.deltaTime;
        }

        imgHpYellow.fillAmount = currentHpPrg;
    }

    public void OnHeadBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        BattleSys.Instance.SetBattleEndWndState(BattleEndType.Pause, true);
    }
}
