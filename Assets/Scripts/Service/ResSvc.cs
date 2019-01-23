using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 资源加载服务
/// </summary>

public class ResSvc : MonoBehaviour {
    private static ResSvc instance = null;
    private ResSvc() { }
    public static ResSvc Instance
    {
        get
        {
            return instance;
        }
    }

    private Dictionary<string, AudioClip> AudioDic = new Dictionary<string, AudioClip>();

    private Action PrgCB = null;

    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        if (PrgCB != null)
        {
            PrgCB();
        }

    }

    public void InitSvc()
    {
        Debug.Log("Init ResSvc...");
        InitRDNameCfg();
    }

    public void AsyncLoadScene(string SceneName,Action Loaded)
    {
        //显示加载页面进度条
        GameRoot.Instance.loadingWnd.SetWndState(true);
        AsyncOperation  async =  SceneManager.LoadSceneAsync(SceneName);

        PrgCB = () =>//不断更新进度条进度
        {
            GameRoot.Instance.loadingWnd.SetProgress(async.progress);
            if (async.progress ==1)//加载完成，调用回调函数
            {
                if (Loaded!=null)
                {
                    Loaded();
                }
                PrgCB = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
            }
        };

    }

    public AudioClip LoadAudio(string path, bool iscache = false)
    {
        AudioClip clip = null;
        if (!AudioDic.TryGetValue(path,out clip))
        {
            clip = Resources.Load<AudioClip>(path);
            if (iscache)
            {
                AudioDic.Add(path, clip);
            }
        }
        return clip;
    }

    #region InitCfgs

    private List<string> surName = new List<string>();
    private List<string> manName = new List<string>();
    private List<string> womanName = new List<string>();

    public void InitRDNameCfg()
    {
        TextAsset xml = Resources.Load<TextAsset>(PathDefine.RDNameCfg);
        if (!xml)
        {
            Debug.Log("Xml fail :" + PathDefine.RDNameCfg + " is not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID")==null)
                {
                    continue;
                }
                int ID = Convert.ToInt32( ele.GetAttributeNode("ID").InnerText);
                foreach (XmlElement item in ele.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "surname":
                            surName.Add(item.InnerText);
                            break;
                        case "man":
                            manName.Add(item.InnerText);
                            break;
                        case "woman":
                            womanName.Add(item.InnerText);
                            break;
                    }
                }
            }
        }
    }

    public string GetRdName(bool isman = true)
    {
        System.Random rd = new System.Random();
        string Rdname = surName[PETools.GetRdInt(0, surName.Count - 1,rd)];
        if (isman)
        {
            Rdname += manName[PETools.GetRdInt(0, manName.Count - 1, rd)];
        }
        else
        {
            Rdname += womanName[PETools.GetRdInt(0, womanName.Count - 1, rd)];
        }

        return Rdname;
    }
    #endregion
}
