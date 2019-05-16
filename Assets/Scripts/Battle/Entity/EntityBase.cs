
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 逻辑实体基类
/// </summary>
public abstract class EntityBase{
    public AniState currentAniState = AniState.None;

    public BattleMgr battleMgr = null;
    public StateMgr stateMgr = null;
    public SkillMgr skillMgr = null;

    public EntityType entityType = EntityType.None;
    public EntityState entityState = EntityState.None;

    public bool canMove = true;
    public bool canRlsSkill = true;

    protected Controller controller = null;

    private BattleProps props;
    private string name;

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }


    public BattleProps Props
    {
        get
        {
            return props;
        }

        protected set
        {
            props = value;
        }
    }

    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            //通知UI层TODO
            PECommon.Log("hp change:" + hp + "to" + value);
            SetHpVal(hp, value);
            hp = value;
        }
    }
    private int hp;
    public Queue<int> comboQue = new Queue<int>();
    public int nextSkillID = 0;
    public SkillCfg curSkillCfg;

    //技能位移回调ID（TimeTask）
    public List<int> skillMoveCBLst = new List<int>();
    //技能伤害回调ID（TimeTask）
    public List<int> skillActionCBLst = new List<int>();

    public int skillEndCB = -1;

    public virtual void SetBattleProps(BattleProps props)
    {
        Hp = props.hp;
        Props = props;
    }

    public void Born()
    {
        stateMgr.ChangeState(this, AniState.Born);
    }

    public void Die()
    {
        stateMgr.ChangeState(this, AniState.Die);
    }

    public void Hit()
    {
        stateMgr.ChangeState(this, AniState.Hit);
    }

    public void Idle()
    {
        stateMgr.ChangeState(this, AniState.Idle);
    }

    public void Move()
    {
        stateMgr.ChangeState(this, AniState.Move);
    }

    public void Attack(int skillID)
    {
        stateMgr.ChangeState(this, AniState.Attack,skillID);
    }

    public virtual void TickAILogic() { }

    public virtual Vector2 CalcTargetDir()
    {
        return Vector2.zero;
    }

    public virtual void SetBlend(float blend)
    {
        if (controller!=null)
        {
            controller.SetBlend(blend);
        }
    }

    public virtual void SetDir(Vector2 dir)
    {
        if (controller != null)
        {
            controller.Dir = dir;
        }
    }

    public virtual void SetAction(int action)
    {
        if (controller != null)
        {
            controller.SetAction(action);
        }
    }

    public  void SkillAttack(int skillID)
    {
        skillActionCBLst.Clear();
        skillMoveCBLst.Clear();
        AttackEffect(skillID);
        AttackDamage(skillID);
    }

    public virtual void AttackEffect(int skillID)
    {
        skillMgr.AttackEffect(this, skillID);
    }

    public virtual void AttackDamage(int skillID)
    {
        skillMgr.AttackDamage(this, skillID);
    }

    public virtual void SetFX(string name,float destroy)
    {
        if (controller != null)
        {
            controller.SetFX(name,destroy);
        }
    }

    public void SetSkillMoveState(bool move, float speed = 0.0f)
    {
        if (controller != null)
        {
            controller.SetSkillMoveState(move, speed);
        }
    }

    public virtual Vector2 GetDirInput()
    {
        return Vector2.zero;
    }
    
    public virtual void SetAtkDir(Vector2 dir,bool offset = false)
    {
        if (controller != null)
        {
            if (offset)
            {
                controller.SetAtkDirCam(dir);
            }
            else
            {
                controller.SetAtkDirLocal(dir);
            }
        }
    }

    public Vector3 GetPos()
    {
        return controller.transform.position;
    }

    public Transform GetTrans()
    {
        return controller.transform;
    }

    public virtual void SetDodge()
    {
        if (controller !=null)
        {
            GameRoot.Instance.dynamicWnd.SetDodge(Name);
        }
    }

    public void SetCritical(int critical)
    {
        if (controller != null)
        {
            GameRoot.Instance.dynamicWnd.SetCritical(Name, critical);
        }
    }

    public void SetHurt(int hurt)
    {
        if (controller != null)
        {
            GameRoot.Instance.dynamicWnd.SetHurt(Name, hurt);
        }
    }

    public virtual void SetHpVal(int oldVal,int newVal)
    {
        if (controller != null)
        {
            GameRoot.Instance.dynamicWnd.SetHpVal(Name, oldVal, newVal);
        }
    }
    public AnimationClip[] GetAniClips()
    {
        if (controller!= null)
        {
            return controller.anim.runtimeAnimatorController.animationClips;
        }
        return null;
    }

    public void SetController(Controller ctrl)
    {
        controller = ctrl;
    }

    public void SetActive(bool isActive = true)
    {
        if (controller !=null)
        {
            controller.gameObject.SetActive(isActive);
        }
    }

    public void ExitCurSkill()
    {
        if (curSkillCfg !=null)
        {
            if (curSkillCfg.isCombo)
            {
                if (comboQue.Count > 0)
                {
                    nextSkillID = comboQue.Dequeue();
                }
                else
                {
                    nextSkillID = 0;
                }
            }

            if (entityState == EntityState.BatiState)
            {
                entityState = EntityState.None;
            }

            curSkillCfg = null;
        }



        SetAction(Constants.DefaultAction);
        canMove = true;
    }

    public AudioSource GetAudioSource()
    {
        return controller.GetComponent<AudioSource>();
    }

    public void RemoveMoveCB(int tid)
    {
        int index = -1;
        for (int i = 0; i < skillMoveCBLst.Count; i++)
        {
            if (skillMoveCBLst[i] == tid)
            {
                index = i;
                break;
            }
        }
        if (index != -1)
        {
            skillMoveCBLst.RemoveAt(index);
        }
    }

    public void RemoveActionCB(int tid)
    {
        int index = -1;
        for (int i = 0; i < skillActionCBLst.Count; i++)
        {
            if (skillActionCBLst[i] == tid)
            {
                index = i;
                break;
            }
        }
        if (index != -1)
        {
            skillActionCBLst.RemoveAt(index);
        }
    }

    public virtual bool GetBreakState()
    {
        return true;
    }

    public void RemoveSkillCB()
    {
        for (int i = 0; i < skillMoveCBLst.Count; i++)
        {
            int tid = skillMoveCBLst[i];
            TimeSvc.Instance.DelTask(tid);
        }

        for (int i = 0; i < skillActionCBLst.Count; i++)
        {
            int tid = skillActionCBLst[i];
            TimeSvc.Instance.DelTask(tid);
        }

        if (skillEndCB != -1)
        {
            TimeSvc.Instance.DelTask(skillEndCB);
            skillEndCB = -1;
        }


        //清空连招队列
        if (nextSkillID != 0 || comboQue.Count > 0)
        {
            nextSkillID = 0;
            comboQue.Clear();
            battleMgr.lastAtkTime = 0;
            battleMgr.comboIndex = 0;
        }
    }
}
