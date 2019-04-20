
using UnityEngine;
/// <summary>
/// 逻辑实体基类
/// </summary>
public abstract class EntityBase{
    public AniState currentAniState = AniState.None;

    public BattleMgr battleMgr = null;
    public StateMgr stateMgr = null;
    public SkillMgr skillMgr = null;

    public bool canMove = true;

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

    public Vector3 GetPos()
    {
        return controller.transform.position;
    }

    public Transform GetTrans()
    {
        return controller.transform;
    }

    public void SetDodge()
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

    public void SetHpVal(int oldVal,int newVal)
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
}
