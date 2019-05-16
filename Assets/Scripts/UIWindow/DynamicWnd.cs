using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 动态UI元素界面
/// </summary>
public class DynamicWnd : WindowRoot {
    public Animation tipsAnim;
    public Text txtTips;
    public Transform transHpItemRoot;

    public Animation playerDodgeAni;

    private Queue<string> tipsQueue = new Queue<string>();
    private Dictionary<string, ItemEntityHp> itemHpDic = new Dictionary<string, ItemEntityHp>();
    private bool isTipsPlay = false;


    protected override void InitWnd()
    {
        base.InitWnd();
        SetActive(txtTips, false);
    }

    private void Update()
    {
        DeTips();
    }

    #region Tips
    public void AddTips(string tips)
    {
        lock (tipsQueue)
        {
            tipsQueue.Enqueue(tips);
        }
    }

    public void DeTips()
    {
        if (tipsQueue.Count>0&&!isTipsPlay)
        {
            isTipsPlay = true;
            lock (tipsQueue)
            {
                string tips = tipsQueue.Dequeue();
                SetTips(tips);
            }
        }
    }

    public void SetTips(string tips)
    {
        SetActive(txtTips, true);
        SetText(txtTips, tips);

        AnimationClip clip = tipsAnim.clip;
        tipsAnim.Play();
        //播放完动画之后再将txt设置为false
        StartCoroutine(AfterAnimPlay(clip.length, () =>
        {
            SetActive(txtTips, false);
            isTipsPlay = false;
        }));
    }

    private IEnumerator AfterAnimPlay(float sec,Action cb)
    {
        yield return new WaitForSeconds(sec);
        if (cb !=null)
        {
            cb();
        }
    }
    #endregion

    public void AddHpItemInfo(string mname,Transform rootTrans, int hp)
    {
        ItemEntityHp itemHp = null;
        if (itemHpDic.TryGetValue(mname,out itemHp))
        {
            return;
        }

        else
        {
            GameObject go = ResSvc.Instance.LoadPrefab(PathDefine.ItemEntityHp,true);
            go.transform.SetParent(transHpItemRoot);
            go.transform.localPosition = new Vector3(-2000, 0, 0);
            ItemEntityHp ieh = go.GetComponent<ItemEntityHp>();
            ieh.SetItemInfo(rootTrans,hp);
            itemHpDic.Add(mname, ieh);
        }
    }

    public void DelHpItemInfo(string mname)
    {
        ItemEntityHp itemHp = null;
        if (itemHpDic.TryGetValue(mname, out itemHp))
        {
            Destroy(itemHp.gameObject);
            itemHpDic.Remove(mname);
        }
    }

    public void DelAllHpItemInfo()
    {
        foreach (var item in itemHpDic)
        {
            Destroy(item.Value.gameObject);
        }

        itemHpDic.Clear();
    }

    public void SetDodge(string mname)
    {
        ItemEntityHp item = null;
        if (itemHpDic.TryGetValue(mname,out item))
        {
            item.SetDodge();
        }
    }

    public void SetCritical(string mname,int critical)
    {
        ItemEntityHp item = null;
        if (itemHpDic.TryGetValue(mname, out item))
        {
            item.SetCritical(critical);
        }
    }

    public void SetHurt(string mname,int hurt)
    {
        ItemEntityHp item = null;
        if (itemHpDic.TryGetValue(mname, out item))
        {
            item.SetHurt(hurt);
        }
    }

    public void SetHpVal(string mname, int oldVal, int newVal)
    {
        ItemEntityHp item = null;
        if (itemHpDic.TryGetValue(mname, out item))
        {
            item.SetHpVal(oldVal,newVal);
        }
    }

    public void SetPlayerDodge()
    {
        playerDodgeAni.Stop();
        playerDodgeAni.Play();
    }
}
