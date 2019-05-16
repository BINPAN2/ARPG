using UnityEngine;
/// <summary>
/// 怪物逻辑实体
/// </summary>
public class MonsterEntity : EntityBase
{
    public MonsterData md;
    private float checkTime = 2;
    private float checkCountTime = 0;

    private float atkTime = 1;
    private float atkCountTime = 0;

    public MonsterEntity()
    {
        entityType = EntityType.Monster;
    }

    public override void SetBattleProps(BattleProps props)
    {
        int level = md.level;
        BattleProps p = new BattleProps
        {
            hp = level*props.hp,
            ad = level*props.ad,
            ap = level*props.ap,
            addef = level*props.addef,
            apdef = level*props.apdef,
            dodge = level*props.dodge,
            critical = level * props.critical,
            pierce = level*props.pierce,
        };

        Props = p;
        Hp = p.hp;
    }

    private bool runAI = true;
    public override void TickAILogic()
    {
        if (!runAI)
        {
            return;
        }
        if (currentAniState == AniState.Idle ||currentAniState == AniState.Move)
        {
            if (battleMgr.isPauseGame)
            {
                Idle();
                return;
            }
            checkCountTime += Time.deltaTime;
            if (checkCountTime < checkTime)
            {
                return;
            }
            else
            {
                //计算目标方向
                Vector2 dir = CalcTargetDir();

                //判断目标是否再攻击范围
                if (!InAtkRange())
                {
                    //不在攻击范围，设置移动方向，进入移动状态
                    SetDir(dir);
                    Move();
                }
                else
                {
                    //在攻击范围，停止移动，进行攻击
                    SetDir(Vector2.zero);
                    atkCountTime += checkCountTime;
                    if (atkCountTime > atkTime)
                    {
                        SetAtkDir(dir);
                        Attack(md.monsterCfg.skillID);
                        atkCountTime = 0;
                    }
                    else
                    {
                        Idle();
                    }
                }
                checkCountTime = 0;
                checkTime = PETools.GetRdInt(1, 5) * 1.0f / 10;
            }
        }

       
    }

    public override Vector2 CalcTargetDir()
    {
        PlayerEntity playerEntity = battleMgr.playerEntity;
        if (playerEntity == null || playerEntity.currentAniState == AniState.Die)
        {
            runAI = false;
            return Vector2.zero;
        }
        else
        {
            Vector3 target = playerEntity.GetPos();
            Vector3 self = GetPos();
            return new Vector2(target.x - self.x, target.z - self.z).normalized;
        }
    }

    private bool InAtkRange()
    {
        PlayerEntity playerEntity = battleMgr.playerEntity;
        if (playerEntity == null || playerEntity.currentAniState == AniState.Die)
        {
            runAI = false;
            return false;
        }
        else
        {
            Vector3 target = playerEntity.GetPos();
            Vector3 self = GetPos();
            target.y = 0;
            self.y = 0;
            float dis = Vector3.Distance(target, self);
            if (dis <= md.monsterCfg.atkDis)
            {
                return true;
            }
            return false;
        }
    }

    public override bool GetBreakState()
    {
        if (md.monsterCfg.isStop)
        {
            if (curSkillCfg != null)
            {
                return curSkillCfg.isBreak;
            }
            return true;
        }
            return false;
    }

    public override void SetHpVal(int oldVal, int newVal)
    {
        if (md.monsterCfg.monsterType == MonsterType.Normal)
        {
            base.SetHpVal(oldVal, newVal);
        }
        else if (md.monsterCfg.monsterType == MonsterType.Boss)
        {
            BattleSys.Instance.playerCtrlWnd.SetBossHpVal(oldVal, newVal, Props.hp);

        }
    }
}
