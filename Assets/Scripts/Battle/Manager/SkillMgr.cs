using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 技能管理器
/// </summary>
public class SkillMgr : MonoBehaviour
{

    public void Init()
    {
        PECommon.Log("Init SkillMgr Done...");

    }

    /// <summary>
    /// 技能伤害计算
    /// </summary>
    public void AttackDamage(EntityBase from, int skillID)
    {
        SkillCfg skillcfg = ResSvc.Instance.GetSkillCfg(skillID);
        List<int> skillActionLst = skillcfg.skillActionLst;
        int sumTime = 0;
        for (int i = 0; i < skillActionLst.Count; i++)
        {
            SkillActionCfg skillActionCfg = ResSvc.Instance.GetSkillActionCfg(skillActionLst[i]);
            sumTime += skillActionCfg.delayTime;
            int index = i;//第几个Action
            if (sumTime > 0)
            {
                TimeSvc.Instance.AddTimeTask((int tid) =>
                {
                    SkillAction(from, skillcfg,index);
                }, sumTime);
            }
            else
            {
                SkillAction(from, skillcfg, index);
            }
        }
    }

    public void SkillAction(EntityBase from, SkillCfg skillcfg,int index)
    {
        List<MonsterEntity> monsterLst = from.battleMgr.GetMonsterEntity();
        int damage = skillcfg.skillDamageLst[index];
        SkillActionCfg skillActionCfg = ResSvc.Instance.GetSkillActionCfg(skillcfg.skillActionLst[index]);

        for (int i = 0; i < monsterLst.Count; i++)
        {
            MonsterEntity em = monsterLst[i];
            //判断距离和角度
            if (IsInRange(from.GetPos(), em.GetPos(), skillActionCfg.radius)
                && IsInAngle(from.GetTrans(), em.GetPos(), skillActionCfg.angle))
            {
                //计算伤害
                CalcDamage(from, em, skillcfg, damage);
            }
        }
    }

    private System.Random rd = new System.Random();
    public void CalcDamage(EntityBase from,EntityBase target,SkillCfg skillCfg, int damage)
    {
        bool isCritical = false;
        int sumDamage = damage;
        //物理伤害
        if (skillCfg.damageType == DamageType.Ad)
        {
            //计算闪避
            int dodgeNum = PETools.GetRdInt(1, 100, rd);
            if (dodgeNum <= target.Props.dodge)
            {
                //显示闪避UI TODO
                PECommon.Log("闪避");
                target.SetDodge();
                return;
            }
            //计算属性加成
            sumDamage += from.Props.ad;

            //计算穿甲
            int addef = (int)((1 - from.Props.pierce / 100f) * target.Props.addef);
            sumDamage -= addef;

            //计算暴击
            int criticalNum = PETools.GetRdInt(1, 100, rd);
            if (criticalNum <= from.Props.critical)
            {
                sumDamage = sumDamage * 2;
                isCritical = true;
                PECommon.Log("暴击");
            }
        }
        //魔法伤害
        else if(skillCfg.damageType == DamageType.Ap)
        {
            //计算属性加成
            sumDamage += from.Props.ap;
            //计算魔法抗性
            sumDamage -= target.Props.apdef;
        }

        if (isCritical)
        {
            target.SetCritical(sumDamage);
        }
        else
        {
            target.SetHurt(sumDamage);
        }

        if (sumDamage < 0)
        {
            sumDamage = 0;
            return;
        }

        if (target.Hp<sumDamage)
        {
            target.Hp = 0;
            target.Die();
            target.battleMgr.RemoveMonster(target.Name);
        }
        else
        {
            target.Hp -= sumDamage;
            target.Hit();
        }


    }

    public bool IsInRange(Vector3 from, Vector3 to, float range)
    {
        float dis = Vector3.Distance(from, to);
        if (dis < range)
        {
            return true;
        }
        return false;
    }

    public bool IsInAngle(Transform trans, Vector3 to, float angle)
    {
        if (angle == 360)
        {
            return true;
        }
        else
        {
            Vector3 start = trans.forward;
            Vector3 dir = (to - trans.position).normalized;
            float ang = Vector3.Angle(start, dir);
            if (ang <= angle/2)
            {
                return true;
            }
            return false;
        }

    }

    /// <summary>
    /// 技能效果表现（动画与特效）
    /// </summary>
    public void AttackEffect(EntityBase entity, int skillID)
    {
        entity.canMove = false;
        entity.SetDir(Vector2.zero);
        SkillCfg skillcfg = ResSvc.Instance.GetSkillCfg(skillID);

        entity.SetAction(skillcfg.aniAction);
        entity.SetFX(skillcfg.fx, skillcfg.skillTime);

        CalcSkillMove(entity, skillcfg);

        TimeSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.Idle();
        }, skillcfg.skillTime);
    }


    private void CalcSkillMove(EntityBase entity, SkillCfg skillcfg)
    {
        int sumTime = 0;
        for (int i = 0; i < skillcfg.skillMoveLst.Count; i++)
        {
            SkillMoveCfg skillMoveCfg = ResSvc.Instance.GetSkillMoveCfg(skillcfg.skillMoveLst[i]);

            float speed = skillMoveCfg.moveDis / (skillMoveCfg.moveTime / 1000f);
            if (skillMoveCfg.delayTime > 0)
            {
                sumTime += skillMoveCfg.delayTime;
                TimeSvc.Instance.AddTimeTask((int tid) =>
                {
                    entity.SetSkillMoveState(true, speed);
                }, sumTime);
            }
            else
            {
                entity.SetSkillMoveState(true, speed);
            }
            sumTime += skillMoveCfg.moveTime;
            TimeSvc.Instance.AddTimeTask((int tid) =>
            {
                entity.SetSkillMoveState(false);
            }, sumTime);
        }
    }
}
