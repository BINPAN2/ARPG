using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocol;
/// <summary>
/// 对话窗口
/// </summary>
public class GuideWnd : WindowRoot {

    public Text txtName;
    public Text txtTalk;
    public Image imgIcon;

    private PlayerData pd;
    private AutoGuideCfg curTaskData;
    private string[] dialogArr;
    private int index;

    protected override void InitWnd()
    {
        base.InitWnd();
        curTaskData = MainCitySys.Instance.GetCurTaskData();
        dialogArr = curTaskData.dialogArr.Split('#');
        pd = GameRoot.Instance.PlayerData;
        index = 1;
        SetTalk();
    }

    private void SetTalk()
    {
        string[] talkArr = dialogArr[index].Split('|');
        //设置对话人图片和名字
        if (talkArr[0] == "0")
        {
            //自己
            SetText(txtName, pd.name);
            SetSprite(imgIcon, PathDefine.SelfIcon);
        }
        else{
            //NPC
            switch (curTaskData.npcID)
            {
                case 0:
                    SetText(txtName, "智者");
                    SetSprite(imgIcon, PathDefine.WiseManIcon);
                    break;
                case 1:
                    SetText(txtName, "将军");
                    SetSprite(imgIcon, PathDefine.GeneralIcon);
                    break;
                case 2:
                    SetText(txtName, "工匠");
                    SetSprite(imgIcon, PathDefine.ArtisanIcon);
                    break;
                case 3:
                    SetText(txtName, "商人");
                    SetSprite(imgIcon, PathDefine.TraderIcon);
                    break;
                default:
                    SetText(txtName, "梁非凡");
                    SetSprite(imgIcon, PathDefine.GuideIcon);
                    break;
            }
        }

        imgIcon.SetNativeSize();

        SetText(txtTalk, talkArr[1].Replace("$name", pd.name));
    }

    public void OnNextBtnClick()
    {
        index++;
        if (index == dialogArr.Length )
        {
            //没有对话了，向服务器发送请求，获取经验和金币
            MainCitySys.Instance.SendGuideMsg();
            //关闭对话栏
            SetWndState(false);
        }
        else
        {
            SetTalk();
        }
    }


}
