using System.Collections.Generic;
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

public class MonsterData : BaseData<MonsterData>
{
    public int mWave;//批次
    public int mIndex;//序号
    public MonsterCfg monsterCfg;
    public Vector3 monsterBornPos;
    public Vector3 monsterBornRote;
    public int level;
}

public class MonsterCfg : BaseData<MonsterCfg>
{
    public string mName;
    public string resPath;
    public BattleProps battleProps;
}


public class MapCfg : BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public int power;
    public Vector3 mainCamPos;
    public Vector3 mainCamRotate;
    public Vector3 playerBornPos;
    public Vector3 playerBornRotate;
    public List<MonsterData> monsterLst;
}


public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    public int npcID;
    public string dialogArr;
    public int actID;
    public int coin;
    public int exp;
}

public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public string taskName;
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}

public class SkillCfg : BaseData<SkillCfg>
{
    public string skillName;
    public int skillTime;
    public int cdTime;
    public int aniAction;
    public string fx;
    public DamageType damageType;
    public List<int> skillMoveLst;
    public List<int> skillActionLst;
    public List<int> skillDamageLst;
}
public class SkillMoveCfg : BaseData<SkillMoveCfg>
{
    public int moveTime;
    public float moveDis;
    public int delayTime;
}

public class SkillActionCfg : BaseData<SkillActionCfg>
{
    public int delayTime;
    public float radius;
    public int angle;
}

public class BattleProps
{
    public int critical;
    public int pierce;
    public int dodge;
    public int apdef;
    public int addef;
    public int ap;
    public int ad;
    public int hp;
}
