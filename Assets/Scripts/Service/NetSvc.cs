using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocol;
using PENet;
/// <summary>
/// 网络服务
/// </summary>
public class NetSvc : MonoBehaviour {
    private static NetSvc instance = null;
    PESocket<ClientSession, GameMsg> client = null;
    private NetSvc() { }
    public static NetSvc Instance
    {
        get
        {
            return instance;
        }
    }

    private static readonly string obj = "lock";
    private Queue<GameMsg> msgQue = new Queue<GameMsg>();

    private void Awake()
    {
        instance = this;
    }

    public void InitSvc()
    {
        PECommon.Log("Init NetSvc...");
        client = new PESocket<ClientSession, GameMsg>();
        client.StartAsClient(IPCfg.srvIP, IPCfg.srvPort);
        client.SetLog(true, (string msg, int lv) =>
        {
            switch (lv)
            {
                case 0:
                    msg = "Log:" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn:" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error:" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info:" + msg;
                    Debug.Log(msg);
                    break;
            }
        });
    }


    public void SendMsg(GameMsg msg)
    {
        if (client.session !=null)
        {
            client.session.SendMsg(msg);
        }
        else
        {
            GameRoot.Instance.AddTips("服务器未连接");
            InitSvc();
        }
    }


    public void AddNetPkg(GameMsg msg)
    {
        lock (obj)
        {
            msgQue.Enqueue(msg);
        }
    }

    private void Update()
    {
        if (msgQue.Count>0)
        {
            GameMsg msg = msgQue.Dequeue();
            ProcessMsg(msg);
        }
    }

    private void ProcessMsg(GameMsg msg)
    {
        if (msg.err != (int)ErrorCode.None)
        {
            switch ((ErrorCode)msg.err)
            {
                case ErrorCode.AcctIsOnLine:
                    GameRoot.Instance.AddTips("当前帐号已经上线");
                    break;
                case ErrorCode.WrongPass:
                    GameRoot.Instance.AddTips("密码错误");
                    break;
                case ErrorCode.UpdateDBError:
                    PECommon.Log("数据库更新异常");
                    GameRoot.Instance.AddTips("网络不稳定");//假装告诉用户网络不稳定
                    break;
            }
        }

        switch ((CMD)msg.cmd)
        {
            case CMD.RspLogin:
                LoginSys.Instance.RspLogin(msg);
                break;
            case CMD.RspRename:
                LoginSys.Instance.RspRename(msg);
                break;
        }


    }
}
