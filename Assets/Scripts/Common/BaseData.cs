using UnityEngine;
/// <summary>
/// 配置数据类
/// </summary>
public class BaseData<T>
{
   public int ID;
}

public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;
    public int startlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}


public class MapCfg : BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public Vector3 mainCamPos;
    public Vector3 mainCamRotate;
    public Vector3 playerBornPos;
    public Vector3 playerBornRotate;
}


public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    public int npcID;
    public string dialogArr;
    public int actID;
    public int coin;
    public int exp;
}
