using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 场景加载页面
/// </summary>
public class LoadingWnd : WindowRoot
{

    public Text txtPrg;
    public Text txtTips;
    public Image ImgFG;
    public Image ImgPoint;


    protected override void InitWnd()
    {
        SetText(txtTips, "这是一条Tips");
        SetText(txtPrg, "0 % ");
        ImgFG.fillAmount = 0;
        ImgPoint.transform.localPosition = new Vector3(-750, 0, 0);
    }

    public void SetProgress(float progress)
    {
        SetText(txtPrg, (int)(progress * 100) + "%");
        ImgFG.fillAmount = progress;
        ImgPoint.transform.localPosition = new Vector3(progress * 1500 - 750, 0, 0);
    }
}
