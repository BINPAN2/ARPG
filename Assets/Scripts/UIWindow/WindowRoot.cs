﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UI界面基类
/// </summary>
public class WindowRoot : MonoBehaviour {

    public void SetWndState(bool isActive = true)
    {
        if (this.gameObject.activeSelf != isActive)
        {
            SetActive(this.gameObject,isActive);
        }
        if (isActive)
        {
            InitWnd();
        }
        else
        {
            ClearWnd();
        }
    }

    protected virtual void InitWnd()
    {

    }

    protected virtual void ClearWnd()
    {

    }

    #region Tool Function
    protected void SetActive(GameObject go,bool isActive = true)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans, bool isActive = true)
    {
        trans.gameObject.SetActive(isActive);
    }
    protected void SetActive(RectTransform rectTrans, bool isActive = true)
    {
        rectTrans.gameObject.SetActive(isActive);
    }
    protected void SetActive(Image img, bool isActive = true)
    {
        img.gameObject.SetActive(isActive);
    }
    protected void SetActive(Text txt, bool isActive = true)
    {
        txt.gameObject.SetActive(isActive);
    }


    protected void SetText(Text txt,string context = "")
    {
        txt.text = context;
    }
    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }
    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num.ToString());
    }
    #endregion

}