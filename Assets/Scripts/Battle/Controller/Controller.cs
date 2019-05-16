using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {
    public CharacterController ctrl;
    public Animator anim;
    public Transform hpRoot;
    protected Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();
    protected Transform CamTrans;
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

    public virtual void SetAtkDirCam(Vector2 dir)
    {
        float angle = Vector2.SignedAngle(dir, new Vector2(0, 1)) + CamTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    public virtual void SetAtkDirLocal(Vector2 dir)
    {
        float angle = Vector2.SignedAngle(dir, new Vector2(0, 1));
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }
}
