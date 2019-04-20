using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 血条物体
/// </summary>
public class ItemEntityHp : MonoBehaviour {
    #region UIDefine
    public Image imgHpGrey;
    public Image imgHpRed;

    public Animation criticalAni;
    public Text txtCritical;

    public Animation dodgeAni;
    public Text txtDodge;

    public Animation HpAni;
    public Text txtHp;
    #endregion

    private int maxHpVal;
    private RectTransform rect;
    private Transform rootTrans;
    private float scaleRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SetCritical(999);
        //    SetHurt(333);
        //}

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    SetDodge();
        //}

        rect.anchoredPosition = Camera.main.WorldToScreenPoint(rootTrans.position)*scaleRate;
        UpdateMixBlend();
        imgHpGrey.fillAmount = currentHpPrg;
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

    }

    public void SetCritical(int critical)
    {
        criticalAni.Stop();
        txtCritical.text = "暴击 " + critical;
        criticalAni.Play();
    }

    public void SetDodge()
    {
        dodgeAni.Stop();
        txtDodge.text = "闪避";
        dodgeAni.Play();
    }

    public void SetHurt(int hurt)
    {
        HpAni.Stop();
        txtHp.text = "-" + hurt;
        HpAni.Play();
    }

    public void SetItemInfo(Transform trans, int hp)
    {
        rect = GetComponent<RectTransform>();
        rootTrans = trans;
        maxHpVal = hp;
        imgHpGrey.fillAmount = 1;
        imgHpRed.fillAmount = 1;
    }

    private float currentHpPrg;
    private float targetHpPrg;

    public void SetHpVal(int oldVal,int newVal)
    {
        currentHpPrg = oldVal * 1.0f / maxHpVal;
        targetHpPrg = newVal * 1.0f / maxHpVal;
        imgHpRed.fillAmount = targetHpPrg;
    }
}
