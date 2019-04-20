using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : MonoBehaviour {

    private BattleMgr battleMgr;
    private int defaultWave = 1;
    public void Init(BattleMgr btMgr)
    {
        battleMgr = btMgr;

        //实例化第一批怪物
        battleMgr.LoadMonsterByWaveID(defaultWave);
        PECommon.Log("Init MapMgr Done...");
    }


}
