﻿using PEProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战场管理器(管理某一场具体的战斗（副本）)
/// </summary>
public class BattleMgr : MonoBehaviour
{
    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;
    private GameObject player;

    public PlayerEntity playerEntity;
    private MapCfg mapcfg;
    private Dictionary<string, MonsterEntity> monsterEntityDic = new Dictionary<string, MonsterEntity>();

    public bool triggerCheck = true;
    public bool isPauseGame = false;
    public void Init(int mapid,Action cb = null)
    {
        //初始化各管理器
        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();

        //加载战场地图
        mapcfg = ResSvc.Instance.GetMapCfg(mapid);
        ResSvc.Instance.AsyncLoadScene(mapcfg.sceneName, () =>
        {
            //初始化地图数据
            mapMgr = GameObject.FindGameObjectWithTag("MapRoot").GetComponent<MapMgr>();
            mapMgr.Init(this);

            mapMgr.transform.localPosition = Vector3.zero;
            mapMgr.transform.localScale =Vector3.one;
            Camera.main.transform.position = mapcfg.mainCamPos;
            Camera.main.transform.localEulerAngles = mapcfg.mainCamRotate;

            LoadPlayer(mapcfg);
            playerEntity.Idle();
            //激活第一批次怪物
            ActiveCurrentBatchMonster();

            AudioSvc.Instance.PlayBGAudio(Constants.BGHuangYe);
            BattleSys.Instance.SetPlayerCtrlWndState(true);

            if (cb != null)
            {
                cb();
            }
        });
        PECommon.Log("Init BattleMgr Done...");

    }

    private void Update()
    {
        foreach (var item in monsterEntityDic)
        {
            MonsterEntity me = item.Value;
            me.TickAILogic();
        }

        //检测当前批次的怪物是否被打死
        if (mapMgr!=null)
        {
            if (triggerCheck&& monsterEntityDic.Count == 0)
            {
                bool isExist = mapMgr.SetNextTriggerOn();
                triggerCheck = false;
                if (!isExist)
                {
                    //战斗结束
                    EndBattle(true, playerEntity.Hp);
                    
                }
            }
        }
    }

    public void EndBattle(bool isWin , int restHp)
    {
        isPauseGame = true;
        AudioSvc.Instance.StopBGAudio();
        BattleSys.Instance.EndBattle(isWin, restHp);

    }

    private  void LoadPlayer(MapCfg mapcfg)
    {
        player = ResSvc.Instance.LoadPrefab(PathDefine.AssassinBattle);
        player.transform.position = mapcfg.playerBornPos;
        player.transform.localEulerAngles = mapcfg.playerBornRotate;
        player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        playerEntity = new PlayerEntity
        {
            battleMgr = this,
            stateMgr = stateMgr,
            skillMgr = skillMgr,
        };
        PlayerData pd = GameRoot.Instance.PlayerData;
        BattleProps props = new BattleProps
        {
            hp = pd.hp,
            ad = pd.ad,
            ap = pd.ap,
            addef = pd.addef,
            apdef = pd.apdef,
            dodge = pd.dodge,
            pierce = pd.pierce,
            critical = pd.critical,
        };
        playerEntity.SetBattleProps(props);
        playerEntity.Name = "AssassinBattle";
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.Init();
        playerEntity.SetController(playerController);
    }

    public void LoadMonsterByWaveID(int wave)
    {
        for (int i = 0; i < mapcfg.monsterLst.Count; i++)
        {
            MonsterData md = mapcfg.monsterLst[i];
            if (md.mWave == wave)
            {
                GameObject m = ResSvc.Instance.LoadPrefab(md.monsterCfg.resPath,true);
                m.transform.position = md.monsterBornPos;
                m.transform.localEulerAngles = md.monsterBornRote;
                m.transform.localScale = Vector3.one;
                m.name = "m_" + md.mWave + "_" + md.mIndex;

                MonsterEntity monsterEntity = new MonsterEntity
                {
                    battleMgr = this,
                    stateMgr = stateMgr,
                    skillMgr = skillMgr,
                };

                monsterEntity.md = md;
                monsterEntity.SetBattleProps(md.monsterCfg.battleProps);
                monsterEntityDic.Add(m.name, monsterEntity);
                monsterEntity.Name = m.name;

                MonsterController monsterController = m.GetComponent<MonsterController>();
                monsterController.Init();
                monsterEntity.SetController (monsterController);

                m.SetActive(false);
                if (monsterEntity.md.monsterCfg.monsterType == MonsterType.Normal)
                {
                    GameRoot.Instance.dynamicWnd.AddHpItemInfo(m.name, monsterController.hpRoot, monsterEntity.Hp);
                }
                else if (monsterEntity.md.monsterCfg.monsterType == MonsterType.Boss)
                {
                    BattleSys.Instance.playerCtrlWnd.SetBossHpState(true);
                }
            }
        }
    }

    public void ActiveCurrentBatchMonster()
    {
        TimeSvc.Instance.AddTimeTask((int tid) =>
        {
            foreach (var item in monsterEntityDic)
            {
                item.Value.SetActive(true);
                item.Value.Born();
                TimeSvc.Instance.AddTimeTask((int id)=>
                {
                    item.Value.Idle();
                }, 1000);
            }
        }, 500);
    }

    public List<MonsterEntity> GetMonsterEntity()
    {
        List<MonsterEntity> monsterLst = new List<MonsterEntity>();
        foreach (var item in monsterEntityDic)
        {
            monsterLst.Add(item.Value);
        }
        return monsterLst;
    }

    #region Skill and Move
    public void SetPlayerMoveDir(Vector2 _dir)
    {
        if (playerEntity.canMove ==false)
        {
            return;
        }
        //PECommon.Log(_dir.ToString());
        if (playerEntity.currentAniState == AniState.Idle || playerEntity.currentAniState == AniState.Move)
        {
            if (_dir == Vector2.zero)
            {
                playerEntity.Idle();
                playerEntity.SetDir(Vector2.zero);
            }
            else
            {
                playerEntity.Move();
                playerEntity.SetDir(_dir);
            }

        }

    }
    public void ReqReleaseSkill(int index)
    {
        switch (index)
        {
            case 0:
                ReleaseNormalAtk();
                break;
            case 1:
                ReleaseSkill1();
                break;
            case 2:
                ReleaseSkill2();
                break;
            case 3:
                ReleaseSkill3();
                break;
        }
    }

    private int[] comboArr = new int[] { 111, 112, 113, 114, 115 };
    public double lastAtkTime = 0;
    public int comboIndex = 0;
    public void ReleaseNormalAtk()
    {
        //PECommon.Log("Click Normal Atk");
        if (playerEntity.currentAniState == AniState.Attack)
        {
            //再500ms以内进行第二次点击
            double nowAtkTime = TimeSvc.Instance.GetCurTime();
            if (nowAtkTime - lastAtkTime< Constants.ComboMaxSpace && nowAtkTime - lastAtkTime > Constants.ComboMinSpace && lastAtkTime !=0)
            {
                if (comboArr[comboIndex]!= comboArr[comboArr.Length-1])
                {
                    comboIndex += 1;
                    playerEntity.comboQue.Enqueue(comboArr[comboIndex]);
                    lastAtkTime = nowAtkTime;
                }
                else
                {
                    lastAtkTime = 0;
                    comboIndex = 0 ;
                }

            }
        }
        else if (playerEntity.currentAniState == AniState.Idle || playerEntity.currentAniState == AniState.Move)
        {
            comboIndex = 0;
            lastAtkTime = TimeSvc.Instance.GetCurTime();
            playerEntity.Attack(111);
        }

    }
    public void ReleaseSkill1()
    {
        //PECommon.Log("Click Skill1");
        playerEntity.Attack(101);
    }
    public void ReleaseSkill2()
    {
       //PECommon.Log("Click Skill2");
        playerEntity.Attack(102);

    }
    public void ReleaseSkill3()
    {
       //PECommon.Log("Click Skill3");
        playerEntity.Attack(103);

    }
    #endregion

    public Vector2 GetDirInput()
    {
        return BattleSys.Instance.GetDirInput();
    }
    
    public void RemoveMonster(string mname)
    {
        MonsterEntity monsterEntity = null;
        if (monsterEntityDic.TryGetValue(mname,out monsterEntity))
        {
            monsterEntityDic.Remove(mname);
            GameRoot.Instance.dynamicWnd.DelHpItemInfo(mname);
        }
    }

    public bool CanRlsSkill()
    {
        return playerEntity.canRlsSkill;
    }
}

