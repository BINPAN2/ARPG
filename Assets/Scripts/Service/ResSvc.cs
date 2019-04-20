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
        InitRDNameCfg(PathDefine.RDNameCfg);
        InitMonsterCfg(PathDefine.MonsterCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitAutoGuideCfg(PathDefine.AutoGuideCfg);
        InitStrongCfg(PathDefine.StrongCfg);
        InitTaskRewardCfg(PathDefine.TaskRewardCfg);
        InitSkillCfg(PathDefine.SkillCfg);
        InitSkillMoveCfg(PathDefine.SkillMoveCfg);
        InitSkillActionCfg(PathDefine.SkillActionCfg);
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

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path, bool cache = false)
    {
        GameObject prefab;
        if (!goDic.TryGetValue(path,out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path, prefab);
            }
        }

        GameObject go = null;
        if (prefab !=null)
        {
            go = Instantiate(prefab);
        }
        return go;
    }

    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path,bool cache = false)
    {
        Sprite sp = null;
        if (!spDic.TryGetValue(path, out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spDic.Add(path, sp);
            }
        }

        return sp;
    }


    #region InitCfgs

    #region RandomName
    private List<string> surName = new List<string>();
    private List<string> manName = new List<string>();
    private List<string> womanName = new List<string>();

    public void InitRDNameCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("Xml fail :" + path + " is not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
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
        string Rdname = surName[PETools.GetRdInt(0, surName.Count - 1, rd)];
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

    #region MapCfgs
    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();


    public void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("Xml fail :" + path + " is not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                MapCfg mapCfg = new MapCfg
                {
                    ID = ID,
                    monsterLst = new List<MonsterData>(),
                };
                foreach (XmlElement item in ele.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "mapName":
                            mapCfg.mapName = item.InnerText;
                            break;
                        case "sceneName":
                            mapCfg.sceneName = item.InnerText;
                            break;
                        case "power":
                            mapCfg.power = int.Parse(item.InnerText);
                            break;
                        case "mainCamPos":
                            {
                            string[] arr = item.InnerText.Split(',');
                            mapCfg.mainCamPos = new Vector3(float.Parse(arr[0]), float.Parse(arr[1]), float.Parse(arr[2]));
                            }
                            break;
                        case "mainCamRote":
                            {
                                string[] arr = item.InnerText.Split(',');
                                mapCfg.mainCamRotate = new Vector3(float.Parse(arr[0]), float.Parse(arr[1]), float.Parse(arr[2]));
                            }
                            break;
                        case "playerBornPos":
                            {
                                string[] arr = item.InnerText.Split(',');
                                mapCfg.playerBornPos = new Vector3(float.Parse(arr[0]), float.Parse(arr[1]), float.Parse(arr[2]));
                            }
                            break;
                        case "playerBornRote":
                            {
                                string[] arr = item.InnerText.Split(',');
                                mapCfg.playerBornRotate = new Vector3(float.Parse(arr[0]), float.Parse(arr[1]), float.Parse(arr[2]));
                            }
                            break;
                        case "monsterLst":
                            {
                                string[] valArr = item.InnerText.Split('#');
                                for (int waveindex = 0; waveindex < valArr.Length; waveindex++)
                                {
                                    if (waveindex ==0)
                                    {
                                        continue;
                                    }
                                    string[] tempArr = valArr[waveindex].Split('|');
                                    for (int j = 0; j < tempArr.Length; j++)
                                    {
                                        if (j == 0)
                                        {
                                            continue;
                                        }
                                        string[] arr = tempArr[j].Split(',');
                                        MonsterData md = new MonsterData
                                        {
                                            ID = int.Parse(arr[0]),
                                            mWave = waveindex,
                                            mIndex = j,
                                            monsterCfg = GetMonsterCfg(int.Parse(arr[0])),
                                            monsterBornPos = new Vector3(float.Parse(arr[1]), float.Parse(arr[2]), float.Parse(arr[3])),
                                            monsterBornRote = new Vector3(0, float.Parse(arr[4]), 0),
                                            level = int.Parse(arr[5]),
                                        };
                                        mapCfg.monsterLst.Add(md);
                                    }
                                }
                            }
                            break;
                    }
                }
                mapCfgDataDic.Add(ID, mapCfg);
            }
        }
    }

    public MapCfg GetMapCfg(int id)
    {
        MapCfg data;
        if (mapCfgDataDic.TryGetValue(id,out data))
        {
            return data;
        }
        return null;
    }
    #endregion

    #region MonsterCfgs
    private Dictionary<int, MonsterCfg> monsterCfgDic = new Dictionary<int, MonsterCfg>();


    public void InitMonsterCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("Xml fail :" + path + " is not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                MonsterCfg monsterCfg = new MonsterCfg
                {
                    ID = ID,
                    battleProps = new BattleProps(),
                };
                foreach (XmlElement item in ele.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "mName":
                            monsterCfg.mName = item.InnerText;
                            break;
                        case "resPath":
                            monsterCfg.resPath = item.InnerText;
                            break;
                        case "hp":
                            monsterCfg.battleProps.hp = int.Parse(item.InnerText);
                            break;
                        case "ad":
                            monsterCfg.battleProps.ad = int.Parse(item.InnerText);
                            break;
                        case "ap":
                            monsterCfg.battleProps.ap = int.Parse(item.InnerText);
                            break;
                        case "addef":
                            monsterCfg.battleProps.addef = int.Parse(item.InnerText);
                            break;
                        case "apdef":
                            monsterCfg.battleProps.apdef = int.Parse(item.InnerText);
                            break;
                        case "dodge":
                            monsterCfg.battleProps.dodge = int.Parse(item.InnerText);
                            break;
                        case "pierce":
                            monsterCfg.battleProps.pierce = int.Parse(item.InnerText);
                            break;
                        case "critical":
                            monsterCfg.battleProps.critical = int.Parse(item.InnerText);
                            break;
                    }
                }
                monsterCfgDic.Add(ID, monsterCfg);
            }
        }
    }

    public MonsterCfg GetMonsterCfg(int id)
    {
        MonsterCfg data;
        if (monsterCfgDic.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }
    #endregion

    #region AutoGuideCfg

    private Dictionary<int, AutoGuideCfg> autoGuideCfgDic = new Dictionary<int, AutoGuideCfg>();

    public void InitAutoGuideCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("Xml fail :" + path + " is not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                AutoGuideCfg autoGuideCfg = new AutoGuideCfg
                {
                    ID = ID,

                };
                foreach (XmlElement item in ele.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "npcID":
                            autoGuideCfg.npcID = int.Parse(item.InnerText);
                            break;
                        case "dilogArr":
                            autoGuideCfg.dialogArr = item.InnerText;
                            break;
                        case "actID":
                            autoGuideCfg.actID = int.Parse(item.InnerText);
                            break;
                        case "coin":
                            autoGuideCfg.coin = int.Parse(item.InnerText);
                            break;
                        case "exp":
                            autoGuideCfg.exp = int.Parse(item.InnerText);
                            break;
                    }
                }
                autoGuideCfgDic.Add(ID, autoGuideCfg);
            }
        }
    }

    public AutoGuideCfg GetAutoGuideCfg(int ID)
    {
        AutoGuideCfg autoGuideCfg = null;
        if (autoGuideCfgDic.TryGetValue(ID,out autoGuideCfg))
        {
            return autoGuideCfg;
        }
        return null;
    }


    #endregion

    #region strongCfg
    private Dictionary<int, Dictionary<int, StrongCfg>> strongCfgDic = new Dictionary<int, Dictionary<int, StrongCfg>>();

    public void InitStrongCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("Xml fail :" + path + " is not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                StrongCfg strongCfg = new StrongCfg
                {
                    ID = ID,

                };
                foreach (XmlElement item in ele.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "pos":
                            strongCfg.pos = int.Parse(item.InnerText);
                            break;
                        case "starlv":
                            strongCfg.startlv = int.Parse(item.InnerText);
                            break;
                        case "addhp":
                            strongCfg.addhp = int.Parse(item.InnerText);
                            break;
                        case "adddef":
                            strongCfg.adddef = int.Parse(item.InnerText);
                            break;
                        case "addhurt":
                            strongCfg.addhurt = int.Parse(item.InnerText);
                            break;
                        case "minlv":
                            strongCfg.minlv = int.Parse(item.InnerText);
                            break;
                        case "coin":
                            strongCfg.coin = int.Parse(item.InnerText);
                            break;
                        case "crystal":
                            strongCfg.crystal = int.Parse(item.InnerText);
                            break;
                    }
                }
                Dictionary<int, StrongCfg> dic = null;
                if (strongCfgDic.TryGetValue(strongCfg.pos,out dic))
                {
                    dic.Add(strongCfg.startlv, strongCfg);
                }
                else
                {
                    dic = new Dictionary<int, StrongCfg>();
                    dic.Add(strongCfg.startlv, strongCfg);
                    strongCfgDic.Add(strongCfg.pos,dic);
                }
            }

        }
    }

    public StrongCfg GetStrongCfg(int pos,int startlv)
    {
        StrongCfg sd = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongCfgDic.TryGetValue(pos,out dic))
        {
            if (dic.ContainsKey(startlv))
            {
                sd = dic[startlv];
            }
        }
        return sd;
    }

    public int GetPropAddValPreLv(int pos,int starlv,int type)
    {
        Dictionary<int, StrongCfg> posDic = null;
        int val=0;
        if (strongCfgDic.TryGetValue(pos,out posDic))
        {
            for (int i = 0; i <=starlv; i++)
            {
                StrongCfg sd;
                if (posDic.TryGetValue(i,out sd))
                {
                    switch (type)
                    {
                        case 1://HP
                            val += sd.addhp;
                            break;
                        case 2://Damage
                            val += sd.addhurt;
                            break;
                        case 3://Def
                            val += sd.adddef;
                            break;
                    } 
                }
            }
        }
        return val;
    }

    #endregion

    #region TaskRewardCfg

    private Dictionary<int, TaskRewardCfg> taskRewardCfgDic = new Dictionary<int, TaskRewardCfg>();

    public void InitTaskRewardCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("Xml fail :" + path + " is not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                TaskRewardCfg taskRewardCfg = new TaskRewardCfg
                {
                    ID = ID,

                };
                foreach (XmlElement item in ele.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "taskName":
                            taskRewardCfg.taskName = item.InnerText;
                            break;
                        case "count":
                            taskRewardCfg.count = int.Parse(item.InnerText);
                            break;
                        case "coin":
                            taskRewardCfg.coin = int.Parse(item.InnerText);
                            break;
                        case "exp":
                            taskRewardCfg.exp = int.Parse(item.InnerText);
                            break;
                    }
                }
                taskRewardCfgDic.Add(ID, taskRewardCfg);
            }
        }
    }

    public TaskRewardCfg GetTaskRewardCfg(int ID)
    {
        TaskRewardCfg taskRewardCfg = null;
        if (taskRewardCfgDic.TryGetValue(ID, out taskRewardCfg))
        {
            return taskRewardCfg;
        }
        return null;
    }


    #endregion

    #region SkillCfg

    private Dictionary<int, SkillCfg> skillCfgDic = new Dictionary<int, SkillCfg>();

    public void InitSkillCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("Xml fail :" + path + " is not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                SkillCfg skillCfg = new SkillCfg
                {
                    ID = ID,
                    skillMoveLst = new List<int>(),
                    skillActionLst = new List<int>(),
                    skillDamageLst = new List<int>(),
                };
                foreach (XmlElement item in ele.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "skillName":
                            skillCfg.skillName = item.InnerText;
                            break;
                        case "skillTime":
                            skillCfg.skillTime = int.Parse(item.InnerText);
                            break;
                        case "cdTime":
                            skillCfg.cdTime = int.Parse(item.InnerText);
                            break;
                        case "aniAction":
                            skillCfg.aniAction = int.Parse(item.InnerText);
                            break;
                        case "fx":
                            skillCfg.fx = item.InnerText;
                            break;
                        case "dmgType":
                            if (item.InnerText.Equals("1"))
                            {
                                skillCfg.damageType = DamageType.Ad;

                            }
                            else if (item.InnerText.Equals("2"))
                            {
                                skillCfg.damageType = DamageType.Ap;

                            }
                            break;
                        case "skillMoveLst":
                            string[] skillMoveArr = item.InnerText.Split('|');
                            for (int j = 0; j < skillMoveArr.Length; j++)
                            {
                                if (skillMoveArr[j]!="")
                                {
                                    skillCfg.skillMoveLst.Add(int.Parse(skillMoveArr[j]));
                                }
                            }
                            break;
                        case "skillActionLst":
                            string[] skillActionArr = item.InnerText.Split('|');
                            for (int j = 0; j < skillActionArr.Length; j++)
                            {
                                if (skillActionArr[j] != "")
                                {
                                    skillCfg.skillActionLst.Add(int.Parse(skillActionArr[j]));
                                }
                            }
                            break;
                        case "skillDamageLst":
                            string[] skillDamageArr = item.InnerText.Split('|');
                            for (int j = 0; j < skillDamageArr.Length; j++)
                            {
                                if (skillDamageArr[j] != "")
                                {
                                    skillCfg.skillDamageLst.Add(int.Parse(skillDamageArr[j]));
                                }
                            }
                            break;
                    }
                }
                skillCfgDic.Add(ID, skillCfg);
            }
        }
    }

    public SkillCfg GetSkillCfg(int ID)
    {
        SkillCfg skillCfg = null;
        if (skillCfgDic.TryGetValue(ID, out skillCfg))
        {
            return skillCfg;
        }
        return null;
    }


    #endregion

    #region SkillMoveCfg

    private Dictionary<int, SkillMoveCfg> skillMoveCfgDic = new Dictionary<int, SkillMoveCfg>();

    public void InitSkillMoveCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("Xml fail :" + path + " is not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                SkillMoveCfg skillMoveCfg = new SkillMoveCfg
                {
                    ID = ID,

                };
                foreach (XmlElement item in ele.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "moveTime":
                            skillMoveCfg.moveTime =int.Parse( item.InnerText);
                            break;
                        case "moveDis":
                            skillMoveCfg.moveDis = float.Parse(item.InnerText);
                            break;
                        case "delayTime":
                            skillMoveCfg.delayTime = int.Parse(item.InnerText);
                            break;
                    }
                }
                skillMoveCfgDic.Add(ID, skillMoveCfg);
            }
        }
    }

    public SkillMoveCfg GetSkillMoveCfg(int ID)
    {
        SkillMoveCfg skillMoveCfg = null;
        if (skillMoveCfgDic.TryGetValue(ID, out skillMoveCfg))
        {
            return skillMoveCfg;
        }
        return null;
    }


    #endregion

    #region SkillActionCfg

    private Dictionary<int, SkillActionCfg> skillActionCfgDic = new Dictionary<int, SkillActionCfg>();

    public void InitSkillActionCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("Xml fail :" + path + " is not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                SkillActionCfg skillActionCfg = new SkillActionCfg
                {
                    ID = ID,

                };
                foreach (XmlElement item in ele.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "delayTime":
                            skillActionCfg.delayTime = int.Parse(item.InnerText);
                            break;
                        case "radius":
                            skillActionCfg.radius = float.Parse(item.InnerText);
                            break;
                        case "angle":
                            skillActionCfg.angle = int.Parse(item.InnerText);
                            break;
                    }
                }
                skillActionCfgDic.Add(ID, skillActionCfg);
            }
        }
    }

    public SkillActionCfg GetSkillActionCfg(int ID)
    {
        SkillActionCfg skillActionCfg = null;
        if (skillActionCfgDic.TryGetValue(ID, out skillActionCfg))
        {
            return skillActionCfg;
        }
        return null;
    }


    #endregion


    #endregion

}
