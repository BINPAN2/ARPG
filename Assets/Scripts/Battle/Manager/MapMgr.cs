using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : MonoBehaviour {

    private BattleMgr battleMgr;
    private int waveIndex = 1;

    public TriggerData[] triggerDataArr;
    
    public void Init(BattleMgr btMgr)
    {
        battleMgr = btMgr;

        //实例化第一批怪物
        battleMgr.LoadMonsterByWaveID(waveIndex);
        PECommon.Log("Init MapMgr Done...");
    }

    public void TriggerMonsterBorn(TriggerData trigger,int waveIndex)
    {
        if (battleMgr!=null)
        {
            BoxCollider co = trigger.gameObject.GetComponent<BoxCollider>();
            co.isTrigger = false;

            battleMgr.LoadMonsterByWaveID(waveIndex);
            battleMgr.ActiveCurrentBatchMonster();
            battleMgr.triggerCheck = true;
        }
    }

    public bool SetNextTriggerOn()
    {
        waveIndex += 1;
        for (int i = 0; i < triggerDataArr.Length; i++)
        {
            if (triggerDataArr[i].triggerWave == waveIndex)
            {
                BoxCollider co = triggerDataArr[i].GetComponent<BoxCollider>();
                co.isTrigger = true;
                return true;
            }
        }
        return false;
    }
}
