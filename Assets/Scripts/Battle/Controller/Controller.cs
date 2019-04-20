using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {
    public CharacterController ctrl;
    public Animator anim;
    public Transform hpRoot;
    protected Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();

    protected bool isMove = false;
    private Vector2 dir = Vector2.zero;
    public Vector2 Dir
    {
        get
        {
            return dir;

        }
        set
        {
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }

    }

    protected bool isSkillMove = false;
    protected float skillMoveSpeed = 0f;


    public virtual void SetBlend(float blend)
    {
        anim.SetFloat("Blend", blend);
    }

    public virtual void SetAction(int action)
    {
        anim.SetInteger("Action", action);
    }

    public virtual void SetFX(string name ,float destroy)
    {

    }

    public void SetSkillMoveState(bool isSkillMove , float skillMoveSpeed)
    {
        this.isSkillMove = isSkillMove;
        this.skillMoveSpeed = skillMoveSpeed;
    }
}
