using UnityEngine;
/// <summary>
/// 配置数据类
/// </summary>
public class BaseData<T>
{
   public int ID;
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
