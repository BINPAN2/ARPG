using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocol;
using UnityEngine.UI;

public class TaskRewardWnd : WindowRoot {
    public Transform scrollTrans;

    private PlayerData pd = null;
    private List<TaskRewardData> trdList = new List<TaskRewardData>();

    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    public void OnCloseBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    public void RefreshUI()
    {
        trdList.Clear();
        List<TaskRewardData> todoList = new List<TaskRewardData>();
        List<TaskRewardData> doneList = new List<TaskRewardData>();

        //1|0|0
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            TaskRewardData trd = new TaskRewardData
            {
                ID = int.Parse(taskInfo[0]),
                prgs = int.Parse(taskInfo[1]),
                taked = taskInfo[2].Equals("1")
            };

            if (trd.taked)
            {
                doneList.Add(trd);
            }
            else
            {
                todoList.Add(trd);
            }
        }

        trdList.AddRange(todoList);
        trdList.AddRange(doneList);

        //清除上一次实例化的taskItem
        for (int i = 0; i < scrollTrans.childCount; i++)
        {
            Destroy(scrollTrans.GetChild(i).gameObject);
        }


        //实例化taskItem
        for (int i = 0; i < trdList.Count; i++)
        {
            GameObject go = ResSvc.Instance.LoadPrefab(PathDefine.TaskItemPrefab);
            go.transform.SetParent(scrollTrans);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.name = "Itemtask_" +i;

            TaskRewardData trd = trdList[i];
            TaskRewardCfg trcfg = ResSvc.Instance.GetTaskRewardCfg(trd.ID);

            SetText( GetTrans(go.transform, "txtname"),trcfg.taskName);
            SetText(GetTrans(go.transform, "txtProg"), trd.prgs + "/" + trcfg.count);
            SetText(GetTrans(go.transform, "txtReward/txtExp"), "经验 "+trcfg.exp);
            SetText(GetTrans(go.transform, "txtReward/txtCoin"), "金币 " + trcfg.coin);
            Image imgPrg= GetTrans(go.transform, "ImgTaskProg/ImgExpPrg").GetComponent<Image>();
            imgPrg.fillAmount = trd.prgs * 1.0f / trcfg.count;

            Button btnTake = GetTrans(go.transform, "BtnReward").GetComponent<Button>();
            btnTake.onClick.AddListener(()=> { OnBtnTakeClick(go.name); });

            Transform transComp = GetTrans(go.transform, "BtnReward/ImgGotten");
            if (trd.taked)
            {
                btnTake.interactable = false;
                SetActive(transComp, true);
            }
            else
            {
                SetActive(transComp, false);
                if (trd.prgs == trcfg.count)
                {
                    btnTake.interactable = true;
                }
                else
                {
                    btnTake.interactable = false;
                }
            }
        }
    }

    private void OnBtnTakeClick(string name)
    {
        string[] nameArr = name.Split('_');
        int index = int.Parse(nameArr[1]);
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.ReqTakeTaskReward,
            reqTakeTaskReward = new ReqTakeTaskReward
            {
                rid = trdList[index].ID,
            }
        };

        NetSvc.Instance.SendMsg(msg);

        TaskRewardCfg cfg = ResSvc.Instance.GetTaskRewardCfg(trdList[index].ID);
        int coin = cfg.coin;
        int exp = cfg.exp;
        GameRoot.Instance.AddTips(Constants.Color("获得奖励： ", TxtColor.Blue) + Constants.Color("金币+ " + coin + " 经验+ " + exp, TxtColor.Green));
    }
}
