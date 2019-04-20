using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 聊天窗口
/// </summary>
public class ChatWnd : WindowRoot {

    public Image imgWorld;
    public Image imgGroup;
    public Image imgFriend;

    public Text txtChat;

    public InputField iptChat;

    private int chatType = 0;//世界0 工会1 好友2
    private List<string> chatList = new List<string>();

    protected override void InitWnd()
    {
        base.InitWnd();

        RefreshUI();
    }

    public void RefreshUI()
    {
        string strChat = "";
        if (chatType == 0 )
        {
            for (int i = 0; i < chatList.Count; i++)
            {
                strChat += chatList[i] + "\n";
            }
            SetText(txtChat, strChat);

            SetSprite(imgWorld, PathDefine.ChatIconClick);
            SetSprite(imgGroup, PathDefine.ChatIconIdle);
            SetSprite(imgFriend, PathDefine.ChatIconIdle);

        }
        else if (chatType == 1)
        {
            SetText(txtChat, "尚未加入公会");

            SetSprite(imgWorld, PathDefine.ChatIconIdle);
            SetSprite(imgGroup, PathDefine.ChatIconClick);
            SetSprite(imgFriend, PathDefine.ChatIconIdle);
        }
        else if (chatType == 2)
        {
            SetText(txtChat, "暂无好友信息");

            SetSprite(imgWorld, PathDefine.ChatIconIdle);
            SetSprite(imgGroup, PathDefine.ChatIconIdle);
            SetSprite(imgFriend, PathDefine.ChatIconClick);
        }
    }

    public void OnCloseBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        SetWndState(false);
    }

    public void OnWorldBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        RefreshUI();
    }

    public void OnGroupBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        chatType = 1;
        RefreshUI();
    }

    public void OnFriendBtnClick()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.UIClickBtn);
        chatType = 2;
        RefreshUI();
    }


    private bool canSend = true;
    public void OnSendBtnClick()
    {
        if (!canSend)
        {
            GameRoot.Instance.AddTips("聊天信息每5秒发送一次");
            return;
        }

        if (iptChat.text !=""&& iptChat.text != " "&& iptChat.text !=null)
        {
            if (iptChat.text.Length>20)
            {
                GameRoot.Instance.AddTips("输入内容过长");
            }
            else
            {
                GameMsg msg = new GameMsg
                {
                    cmd = (int)CMD.SndChat,
                    sndChat = new SndChat
                    {
                        Chat = iptChat.text,
                    }

                };
                iptChat.text = "";
                NetSvc.Instance.SendMsg(msg);

                canSend = false;

                TimeSvc.Instance.AddTimeTask((int tid) =>
                {
                    canSend = true;
                },5,PETimeUnit.Second);
            }
        }
        else
        {
            return;
        }
    }

    public void AddChatMsg(string name ,string chat)
    {
        chatList.Add(Constants.Color(name + ":" ,TxtColor.Blue)+chat);
        if (chatList.Count>16)
        {
            chatList.RemoveAt(0);
        }
        RefreshUI();
    }

}
